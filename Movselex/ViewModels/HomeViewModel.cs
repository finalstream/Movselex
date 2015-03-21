using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using System.Windows;
using FinalstreamCommons.Extentions;
using FinalstreamCommons.Models;
using FirstFloor.ModernUI;
using FirstFloor.ModernUI.Presentation;
using Livet;
using Livet.EventListeners;
using Movselex.Core;
using Movselex.Core.Models;
using NLog;
using Resources = Movselex.Properties.Resources;

namespace Movselex.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private readonly IMovselexClient _client;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     データベース情報。
        /// </summary>
        private ReadOnlyDispatcherCollection<DatabaseViewModel> _databases;

        /// <summary>
        ///     フィルタリング情報。
        /// </summary>
        private ReadOnlyDispatcherCollection<FilteringViewModel> _filterings;

        /// <summary>
        ///     グループ情報。
        /// </summary>
        private ReadOnlyDispatcherCollection<GroupViewModel> _groups;

        /// <summary>
        ///     ライブラリ情報。
        /// </summary>
        private ReadOnlyDispatcherCollection<LibraryViewModel> _libraries;

        public MovselexAppConfig AppConfig
        {
            get { return _client.AppConfig; }
        }

        public ReadOnlyDispatcherCollection<DatabaseViewModel> Databases
        {
            get { return _databases; }
        }

        public ReadOnlyDispatcherCollection<FilteringViewModel> Filterings
        {
            get { return _filterings; }
        }

        public ReadOnlyDispatcherCollection<GroupViewModel> Groups
        {
            get { return _groups; }
        }

        public ReadOnlyDispatcherCollection<LibraryViewModel> Libraries
        {
            get { return _libraries; }
        }

        /// <summary>
        ///     再生中情報。
        /// </summary>
        public INowPlayingInfo NowPlayingInfo
        {
            get { return _client.NowPlayingInfo; }
        }

        /// <summary>
        ///     再生中リスト情報。
        /// </summary>
        public IEnumerable<PlayingItem> Playings
        {
            get { return null; }
        }

        #region IsThrowable変更通知プロパティ

        private bool _isThrowable;

        public bool IsThrowable
        {
            get { return _isThrowable; }
            set
            {
                if (_isThrowable == value) return;
                _isThrowable = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region IsShufflable変更通知プロパティ

        private bool _isShufflable;

        public bool IsShufflable
        {
            get { return _isShufflable; }
            set
            {
                if (_isShufflable == value) return;
                _isShufflable = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region LibraryCount変更通知プロパティ

        private int _libraryCount;

        public int LibraryCount
        {
            get { return _libraryCount; }
            set
            {
                if (_libraryCount == value) return;
                _libraryCount = value;
                IsThrowable = !String.IsNullOrEmpty(AppConfig.MpcExePath) && value > 0 && value <= AppConfig.LimitNum;
                IsShufflable = value > 0;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region CurrentDatabase変更通知プロパティ

        private DatabaseViewModel _currentDatabase;

        public DatabaseViewModel CurrentDatabase
        {
            get { return _currentDatabase; }
            set
            {
                var isFirst = _currentDatabase == null;
                if (_currentDatabase == value) return;
                _currentDatabase = value;
                if (!isFirst) _client.ChangeDatabase(value.Name);
                AppConfig.SelectDatabase = value.Name;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region FilteringSelectedItem変更通知プロパティ

        private FilteringViewModel _currentFiltering;

        public FilteringViewModel CurrentFiltering
        {
            get { return _currentFiltering; }
            set
            {
                if (_currentFiltering == value) return;
                _currentFiltering = value;
                _client.ChangeFiltering(value.Model);
                RaisePropertyChanged();
            }
        }

        #endregion

        #region LibrarySelectIndex変更通知プロパティ

        private int _librarySelectIndex;

        public int LibrarySelectIndex
        {
            get { return _librarySelectIndex; }
            set
            {
                if (_librarySelectIndex == value) return;
                _librarySelectIndex = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// 処理中かどうかを取得します。
        /// </summary>
        public IProgressInfo ProgressInfo
        {
            get { return _client.ProgressInfo; }
        }

        /// <summary>
        ///     新しいインスタンスを初期化します。
        /// </summary>
        public HomeViewModel()
        {

            _client = MovselexClientFactory.Create(Assembly.GetExecutingAssembly(),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationDefinitions.DefaultAppConfigFilePath));

            CreateReadOnlyDispatcherCollection();
            CreateListener();

            //_client.Refreshed += (sender, args) =>
            //{
            //    //LibraryCount = _client.Libraries.Count();
            //    // フィルタリング選択復元
            //    //var filteringSelect = _filterings.FirstOrDefault(x => x.Model.DisplayValue == AppConfig.SelectFiltering);
            //    //if (filteringSelect != null) filteringSelect.IsSelected = true;
            //    // データベース選択復元
            //    //CurrentDatabase = Databases.SingleOrDefault(x=> x.Name == _client.AppConfig.SelectDatabase);
            //};

            // 初回データベース読み込みを行う
            _client.Initialized += (sender, args) =>
            {
                _client.UpdateLibrary();
                _log.Debug("Initialized MovselexClinet.");
            };

            _client.Initialize();
        }


        /// <summary>
        ///     VM用のコレクションを生成します。
        /// </summary>
        private void CreateReadOnlyDispatcherCollection()
        {
            _databases = ViewModelHelper.CreateReadOnlyDispatcherCollection(_client.Databases,
                s => new DatabaseViewModel(s), DispatcherHelper.UIDispatcher);

            _filterings = ViewModelHelper.CreateReadOnlyDispatcherCollection(_client.Filterings,
                m => new FilteringViewModel(m), DispatcherHelper.UIDispatcher);

            _groups = ViewModelHelper.CreateReadOnlyDispatcherCollection(_client.Groups,
                m => new GroupViewModel(m), DispatcherHelper.UIDispatcher);

            _libraries = ViewModelHelper.CreateReadOnlyDispatcherCollection(_client.Libraries,
                m => new LibraryViewModel(m), DispatcherHelper.UIDispatcher);
        }

        /// <summary>
        ///     リスナーを生成します。
        /// </summary>
        private void CreateListener()
        {
            // データベース変更イベントリスナー
            var databaseListener = new CollectionChangedEventListener(_client.Databases)
            {
                (sender, args) =>
                {
                    // データベース選択復元
                    CurrentDatabase = Databases.SingleOrDefault(x => x.Name == _client.AppConfig.SelectDatabase);
                }
            };
            CompositeDisposable.Add(databaseListener);

            // ライブラリ変更イベントリスナー
            var libraryListener = new CollectionChangedEventListener(_client.Libraries)
            {
                (sender, args) => LibraryCount = _client.Libraries.Count()
            };
            CompositeDisposable.Add(libraryListener);

            // フィルタリング変更イベントリスナー
            var filteringListener = new CollectionChangedEventListener(_client.Filterings)
            {
                (sender, args) =>
                {
                    // フィルタリング選択復元
                    FilteringViewModel filteringSelect =
                        _filterings.FirstOrDefault(x => x.Model.DisplayValue == AppConfig.SelectFiltering);
                    if (filteringSelect != null) filteringSelect.IsSelected = true;
                }
            };
            CompositeDisposable.Add(filteringListener);

            // 設定変更イベントリスナー
            var configListener = new PropertyChangedEventListener(AppConfig)
            {
                {"AccentColor", (sender, args) => AppearanceManager.Current.AccentColor = AppConfig.AccentColor},
            };
            CompositeDisposable.Add(configListener);
        }

        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

        public void Initialize()
        {
            //_filterings = _client.Filterings.FilteringItems;

            //foreach (var filteringItem in client.Filterings.FilteringItems)
            //{
            //    _filterings.Add(filteringItem);
            //}

            //var listener = new CollectionChangedEventListener(
            //    _client.Filterings.FilteringItems);
            //CompositeDisposable.Add(listener);
        }

        public void SwitchLibraryMode()
        {
            _client.SwitchLibraryMode();
        }

        public void Shuffle()
        {
            _client.ShuffleLibrary();
        }

        public void Throw()
        {
            _client.Throw(LibrarySelectIndex);
        }

        public void DoubleClickLibrary()
        {
            Libraries[LibrarySelectIndex].DebugWriteJson();
            _client.InterruptThrow(LibrarySelectIndex);
        }

        public void UpdateLibrary()
        {
            _client.UpdateLibrary();
            _client.Refresh();
        }
        

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _client.Finish();
            _client.Dispose();
        }
    }
}
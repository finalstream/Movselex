using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using FinalstreamCommons.Extentions;
using FinalstreamCommons.Utils;
using FinalstreamUIComponents.Models;
using FinalstreamUIComponents.Views;
using FirstFloor.ModernUI;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Movselex.Core;
using Movselex.Core.Models;
using NLog;
using Button = System.Windows.Controls.Button;
using Resources = Movselex.Properties.Resources;

namespace Movselex.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private readonly IMovselexClient _client;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly ISubject<string> _searchTextChangedSubject = new Subject<string>(); 

        public IMovselexClient Client { get { return _client; }}


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
        /// 再生中情報。
        /// </summary>
        private ReadOnlyDispatcherCollection<PlayingViewModel> _playings; 

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
        public ReadOnlyDispatcherCollection<PlayingViewModel> Playings
        {
            get { return _playings; }
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
                if(value != null)_client.ChangeFiltering(value.Model);
                RaisePropertyChanged();
            }
        }

        #endregion

        #region CurrentGroup変更通知プロパティ

        private GroupViewModel _currentGroup;

        public GroupViewModel CurrentGroup
        {
            get { return _currentGroup; }
            set
            {
                if (_currentGroup == value) return;
                _currentGroup = value;
                if(value != null) _client.ChangeGroup(value.Model);
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

        #region SearchText変更通知プロパティ

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value) return;
                _searchText = value;
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

        // 実装
        ListenerCommand<IEnumerable<string>> m_dropFileCommand;
        // ICommandを公開する
        public ICommand DropFileCommand
        {
            get
            {
                if (m_dropFileCommand == null)
                {
                    m_dropFileCommand = new ListenerCommand<IEnumerable<string>>(enumerable => Client.RegistFiles(enumerable));
                }
                return m_dropFileCommand;
            }
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

            _playings = ViewModelHelper.CreateReadOnlyDispatcherCollection(_client.Playings,
                m => new PlayingViewModel(m), DispatcherHelper.UIDispatcher);
        }

        /// <summary>
        ///     リスナーを生成します。
        /// </summary>
        private void CreateListener()
        {
            var h = _searchTextChangedSubject
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .DistinctUntilChanged()
                .Subscribe(x =>
            {
                // テキストフィルタリングする
                _client.FilteringLibrary(x);
            });
            var listner = new PropertyChangedEventListener(this)
            {
                (sender, args) =>
                {
                    if (args.PropertyName == "SearchText")
                    {
                        _searchTextChangedSubject.OnNext(SearchText);
                    }
                }
            };
            CompositeDisposable.Add(h);
            CompositeDisposable.Add(listner);


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
                 {"SelectedTheme", (sender, args) => AppearanceManager.Current.ThemeSource = AppConfig.SelectedTheme == "light"? AppearanceManager.LightThemeSource : AppearanceManager.DarkThemeSource},
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

#if DEBUG
            Sandbox();
#endif
        }

        private void Sandbox()
        {

            //var paramDic = new Dictionary<string, InputParam>();
            //paramDic.Add("GroupTitle", new InputParam("GroupTitle", "Group1"));
            //var inputTextContent = new InputTextContent("グループを登録します。", paramDic);
            //var dlg = new ModernDialog
            //{
            //    Title = "Regist Group",
            //    Content = inputTextContent
            //};
            //dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
            //var result = dlg.ShowDialog();
            //var input = inputTextContent.InputParamDictionary;
        }


        public void MoveGroupDirectory()
        {
            if(CurrentGroup == null) return;

            var baseDirectory = DialogUtils.ShowFolderDialog(
                "移動する場所を指定してください。指定した場所にグループ名でフォルダを作成して移動します。",
                _client.AppConfig.MoveBaseDirectory);

            if (string.IsNullOrEmpty(baseDirectory)) return;

            var group = CurrentGroup.Model;

            // 移動先フォルダ作成
            var moveDirectory = baseDirectory + "\\" + group.GroupName;

            var result = ModernDialog.ShowMessage(string.Format("{0} を {1} に移動します。よろしいですか？",
                group.GroupName, moveDirectory), "Question?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) _client.MoveGroupDirectory(CurrentGroup.Model, baseDirectory);
        }

        public void EditGroup()
        {
            var group = CurrentGroup.Model;
            var paramDic = new Dictionary<string, InputParam>();
            paramDic.Add("GroupName", new InputParam("GroupName", group.GroupName));
            paramDic.Add("GroupKeyword", new InputParam("GroupKeyword", group.Keyword));
            var inputTextContent = new InputTextContent("グループを編集します。", paramDic);
            var dlg = new ModernDialog
            {
                Title = "Edit Group",
                Content = inputTextContent
            };
            dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
            var result = dlg.ShowDialog();

            if (result == false || !inputTextContent.IsModify) return;

            var input = inputTextContent.InputParamDictionary;

            _client.ModifyGroup(group, input["GroupName"].Value.ToString(), input["GroupKeyword"].Value.ToString());
        }


        public void Grouping()
        {
            var paramDic = new Dictionary<string, InputParam>();

            var library = Libraries[LibrarySelectIndex].Model;
            var gid = library.Gid;
            if (gid != 0)
            {
                // グループ登録済み
                paramDic.Add("GroupName", new InputParam("GroupName", library.GroupName, _groups.Select(x => x.Model.GroupName)));
                paramDic.Add("GroupKeyword", new InputParam("GroupKeyword", _groups.Where(x=>x.Model.Gid== gid).Select(x=>x.Model.Keyword).FirstOrDefault()));
            }
            else
            {
                // グループ未登録
                var keywordList = new List<string>();

                foreach (var title in Libraries.Where(x => x.IsSelected).Select(x => x.Model.Title))
                {
                    var words = MovselexUtils.CreateKeywords(title.Replace("-", "").Trim());
                    keywordList.AddRange(words);
                }

                // 対象のライブラリ中で一番多く出てくるキーワードを抽出
                var keyword = MovselexUtils.GetMaxCountMaxLengthKeyword(keywordList);


                paramDic.Add("GroupName", new InputParam("GroupName", keyword, _groups.Select(x => x.Model.GroupName)));
                paramDic.Add("GroupKeyword", new InputParam("GroupKeyword", keyword));
            }

            var inputTextContent = new InputTextContent("グループを登録します。", paramDic);
            var dlg = new ModernDialog
            {
                Title = "Grouping",
                Content = inputTextContent
            };
            dlg.Buttons = new Button[] { dlg.OkButton, dlg.CancelButton };
            var result = dlg.ShowDialog();

            

            if (result == false) return;

            var input = inputTextContent.InputParamDictionary;

            _client.Grouping(
                input["GroupName"].Value.ToString(),
                input["GroupKeyword"].Value.ToString(),
                Libraries.Where(x=>x.IsSelected).Select(x=>x.Model));
        }

        public void UnGroup()
        {

            var SelectLibraries = Libraries.Where(x => x.IsSelected).Select(x=>x.Model).ToArray();
            if (!SelectLibraries.Any()) return;

            var result = ModernDialog.ShowMessage(string.Format("{0}のグループを解除します。よろしいですか？",
                SelectLibraries.Count() == 1 ? SelectLibraries.Single().Title : "選択したアイテム"), "Question?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) _client.UnGroupLibrary(SelectLibraries);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _client.Finish();
            _client.Dispose();
        }
    }
}
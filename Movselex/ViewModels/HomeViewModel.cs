using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using FinalstreamCommons.Extentions;
using FinalstreamCommons.Utils;
using FinalstreamUIComponents.Commands;
using FinalstreamUIComponents.Models;
using FinalstreamUIComponents.Views;
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Movselex.Commands;
using Movselex.Core;
using Movselex.Core.Models;
using Movselex.Models;
using Movselex.Properties;
using NLog;

namespace Movselex.ViewModels
{
    public class HomeViewModel : ViewModel
    {
        private readonly IMovselexClient _client;
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly ISubject<string> _searchTextChangedSubject = new Subject<string>();

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

        /// <summary>
        ///     再生中情報。
        /// </summary>
        private ReadOnlyDispatcherCollection<PlayingViewModel> _playings;

        // 実装
        private ListenerCommand<IEnumerable<string>> m_dropFileCommand;

        /// <summary>
        ///     新しいインスタンスを初期化します。
        /// </summary>
        public HomeViewModel()
        {
            _client = MovselexClientFactory.Create(Assembly.GetExecutingAssembly(),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationDefinitions.DefaultAppConfigFilePath));

            _client.ExceptionThrowed += ClientOnExceptionThrowed;

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

        public IMovselexClient Client
        {
            get { return _client; }
        }

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

        /// <summary>
        ///     処理中かどうかを取得します。
        /// </summary>
        public IProgressInfo ProgressInfo
        {
            get { return _client.ProgressInfo; }
        }

        // ICommandを公開する
        public ICommand DropFileCommand
        {
            get
            {
                if (m_dropFileCommand == null)
                {
                    m_dropFileCommand =
                        new ListenerCommand<IEnumerable<string>>(enumerable => Client.RegistFiles(enumerable));
                }
                return m_dropFileCommand;
            }
        }

        /// <summary>
        ///     クライアントで例外が発生したら。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="exception"></param>
        private void ClientOnExceptionThrowed(object sender, Exception exception)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (exception is MovselexException)
                {
                    ModernDialog.ShowMessage(exception.Message, "Error", MessageBoxButton.OK);
                }
                else
                {
                    ModernDialog.ShowMessage(Resources.MessageUnknownError, "Error",
                        MessageBoxButton.OK);
                }
            });
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
                //.DistinctUntilChanged()
                .Subscribe(x =>
                {
                    // テキストフィルタリングする
                    _client.FilteringLibrary(x);
                });


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
                    var filteringSelect =
                        _filterings.FirstOrDefault(x => x.Model.DisplayValue == AppConfig.SelectFiltering);
                    if (filteringSelect != null) filteringSelect.IsSelected = true;
                }
            };
            CompositeDisposable.Add(filteringListener);

            // 設定変更イベントリスナー
            var configListener = new PropertyChangedEventListener(AppConfig)
            {
                {"AccentColor", (sender, args) => AppearanceManager.Current.AccentColor = AppConfig.AccentColor},
                {
                    "SelectedTheme",
                    (sender, args) =>
                        AppearanceManager.Current.ThemeSource =
                            AppConfig.SelectedTheme == "light"
                                ? AppearanceManager.LightThemeSource
                                : AppearanceManager.DarkThemeSource
                },
                {
                    "Language", (sender, args) =>
                    {
                        MovselexResource.Current.ChangeCulture(AppConfig.Language);
                        _client.ReloadFiltering();
                    }
                },
                {
                    "MpcExePath", (sender, args) =>
                    {
                        CanThrow = !String.IsNullOrEmpty(AppConfig.MpcExePath);
                        if (_client.IsInitialized) _client.ResetBackgroundWorker();
                    }
                }
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

        public void Trimming()
        {
            _client.TrimmingLibrary(LibrarySelectIndex, IsShuffle);
        }

        public void Throw()
        {
            var view = CollectionViewSource.GetDefaultView(Libraries).OfType<LibraryViewModel>();
            var libraries = view.Select(x => x.Model).Skip(LibrarySelectIndex).Take(AppConfig.MaxGenerateNum);
            _client.Throw(libraries);
        }

        public void UpdateLibrary()
        {
            var view = CollectionViewSource.GetDefaultView(Libraries);

            _client.UpdateLibrary();

#if DEBUG
            Sandbox();
#endif
        }

        private void Sandbox()
        {
            Client.ExecEmpty();
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
            if (CurrentGroup == null) return;

            var baseDirectory = DialogUtils.ShowFolderDialog(
                Resources.MessageMoveGroupDirectory,
                _client.AppConfig.MoveBaseDirectory);

            if (string.IsNullOrEmpty(baseDirectory)) return;

            var group = CurrentGroup.Model;

            var result = ModernDialog.ShowMessage(string.Format(Resources.ConfirmMessageMove,
                group.GroupName, Path.Combine(baseDirectory, group.GroupName)), "Question?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) _client.MoveGroupDirectory(CurrentGroup.Model, baseDirectory);
        }

        public void EditGroup()
        {
            if (CurrentGroup == null) return;
            _client.GetCandidateGroupName(CurrentGroup.Model.GroupName, candidateGroupNames =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var group = CurrentGroup.Model;
                    var paramDic = new Dictionary<string, InputParam>();
                    paramDic.Add("GroupName", new InputParam("GroupName", group.GroupName, candidateGroupNames));
                    paramDic.Add("GroupKeyword", new InputParam("GroupKeyword", group.Keyword));
                    var inputTextContent = new InputTextContent(Resources.MessageEditGroup, paramDic);
                    var dlg = new ModernDialog
                    {
                        Title = Resources.EditGroup,
                        Content = inputTextContent
                    };
                    dlg.Buttons = new[] {dlg.OkButton, dlg.CancelButton};
                    var result = dlg.ShowDialog();

                    if (result == false || !inputTextContent.IsModify) return;

                    var input = inputTextContent.InputParamDictionary;

                    _client.ModifyGroup(group, input["GroupName"].Value.ToString(),
                        input["GroupKeyword"].Value.ToString());
                });
            });
        }

        public void Grouping()
        {
            if (LibrarySelectIndex == -1) return;
            var paramDic = new Dictionary<string, InputParam>();

            var library = Libraries[LibrarySelectIndex].Model;
            var gid = library.Gid;
            if (gid != 0)
            {
                // グループ登録済み
                paramDic.Add("GroupName",
                    new InputParam("GroupName", library.GroupName, _groups.Select(x => x.Model.GroupName)));
                paramDic.Add("GroupKeyword",
                    new InputParam("GroupKeyword",
                        _groups.Where(x => x.Model.Gid == gid).Select(x => x.Model.Keyword).FirstOrDefault()));
            }
            else
            {
                // グループ未登録
                var keywordList = new List<string>();

                foreach (var title in Libraries.Where(x => x.IsSelected).Select(x => x.Model.Title))
                {
                    var words = MovselexUtils.CreateKeywords(title.Trim());
                    keywordList.AddRange(words);
                }

                // 対象のライブラリ中で一番多く出てくるキーワードを抽出
                var keyword = MovselexUtils.GetMaxCountMaxLengthKeyword(keywordList);


                paramDic.Add("GroupName", new InputParam("GroupName", keyword, _groups.Select(x => x.Model.GroupName)));
                paramDic.Add("GroupKeyword", new InputParam("GroupKeyword", keyword));
            }

            var inputTextContent = new InputTextContent(Resources.MessageRegistGroup, paramDic);
            var dlg = new ModernDialog
            {
                Title = "Grouping",
                Content = inputTextContent
            };
            dlg.Buttons = new[] {dlg.OkButton, dlg.CancelButton};
            var result = dlg.ShowDialog();


            if (result == false) return;

            var input = inputTextContent.InputParamDictionary;

            _client.Grouping(
                input["GroupName"].Value.ToString(),
                input["GroupKeyword"].Value.ToString(),
                Libraries.Where(x => x.IsSelected).Select(x => x.Model));
        }

        public void UnGroup()
        {
            var selectLibraries = Libraries.Where(x => x.IsSelected).Select(x => x.Model).ToArray();
            if (!selectLibraries.Any()) return;

            var result = ModernDialog.ShowMessage(string.Format(Resources.ConfirmMessageUnGroup,
                selectLibraries.IsSingle()
                    ? selectLibraries.Single().Title
                    : string.Format(Resources.MessageSelectedItems, selectLibraries.Count())),
                DialogUtils.MessageTitleQuestion, MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) _client.UnGroupLibrary(selectLibraries);
        }

        public void OpenLibraryFolder()
        {
            if (LibrarySelectIndex == -1) return;
            _client.OpenLibraryFolder(SelectLibrary.Model);
        }

        public void MoveLibraryFile()
        {
            var selectLibraries = Libraries.Where(x => x.IsSelected).Select(x => x.Model).ToArray();
            if (!selectLibraries.Any()) return;

            var moveDestDirectory = DialogUtils.ShowFolderDialog(
                Resources.MessageMoveFile,
                _client.AppConfig.MoveBaseDirectory);

            if (string.IsNullOrEmpty(moveDestDirectory)) return;


            var result = ModernDialog.ShowMessage(string.Format(Resources.ConfirmMessageMove,
                selectLibraries.IsSingle()
                    ? selectLibraries.Single().Title
                    : string.Format(Resources.MessageSelectedItems, selectLibraries.Count()), moveDestDirectory),
                "Question?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes) _client.MoveLibraryFile(moveDestDirectory, selectLibraries);
        }

        public void DeleteLibrary()
        {
            var selectLibraries = Libraries.Where(x => x.IsSelected).Select(x => x.Model).ToArray();
            if (!selectLibraries.Any()) return;

            var dlg = new ModernDialog
            {
                Content = string.Format(Resources.ConfirmMessageDelete,
                    selectLibraries.IsSingle()
                        ? selectLibraries.Single().Title
                        : string.Format(Resources.MessageSelectedItems, selectLibraries.Count())),
                Title = DialogUtils.MessageTitleQuestion,
                MinHeight = 0
            };
            var yes = dlg.YesButton;
            yes.Content = "yes (with file)";
            var ok = dlg.OkButton;
            ok.Content = "yes (only libaray)";
            dlg.Buttons = new[] {yes, ok, dlg.NoButton};
            var result = dlg.ShowDialog();
            if (result == false) return;

            var isDeleteFile = dlg.MessageBoxResult == MessageBoxResult.Yes;
            if (isDeleteFile)
            {
                var result2 = ModernDialog.ShowMessage(Resources.ConfirmMessageFileDelete,
                    DialogUtils.MessageTitleQuestion, MessageBoxButton.YesNo);
                if (result2 == MessageBoxResult.No) return;
            }

            _client.DeleteLibrary(selectLibraries, isDeleteFile);
        }

        public void SetShuffleMode(bool isShuffle)
        {
            IsShuffle = isShuffle;
        }

        public void SetSearchKeyword(string searchKeyword)
        {
            SearchText = searchKeyword;
        }

        public void Previous(long previousId)
        {
            _client.InterruptThrow(previousId);
        }

        public void Next(long nextId)
        {
            _client.InterruptThrow(nextId);
        }

        /// <summary>
        ///     お気に入りの状態を変更します。
        /// </summary>
        /// <param name="item"></param>
        public void ModifyIsFavorite(LibraryItem item)
        {
            item.DebugWriteJson();

            App.Client.ModifyIsFavorite(item);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _client.Finish();
            _client.Dispose();
        }

        #region CanThrow変更通知プロパティ

        private bool _canThrow;

        public bool CanThrow
        {
            get { return _canThrow; }
            set
            {
                if (_canThrow == value) return;
                _canThrow = !String.IsNullOrEmpty(AppConfig.MpcExePath) && LibraryCount > 0 && value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region IsTrimmable変更通知プロパティ

        private bool _isTrimmable;

        public bool IsTrimmable
        {
            get { return _isTrimmable; }
            set
            {
                if (_isTrimmable == value) return;
                _isTrimmable = value;
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
                CanThrow = value > 0;
                IsTrimmable = value > 0;
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
                if (value != null)
                {
                    SearchText = null;
                    _client.ChangeFiltering(value.Model, SearchText);
                }
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
                if (value != null)
                {
                    if (string.IsNullOrEmpty(SearchText)) SearchText = null;
                    _client.ChangeGroup(value.Model);
                }
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

        #region LibrarySelectItem変更通知プロパティ

        private LibraryViewModel _SelectLibrary;

        public LibraryViewModel SelectLibrary
        {
            get { return _SelectLibrary; }
            set
            {
                if (_SelectLibrary == value) return;
                _SelectLibrary = value;
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
                _searchTextChangedSubject.OnNext(_searchText);
                RaisePropertyChanged();
            }
        }

        #endregion

        #region IsShuffle変更通知プロパティ

        private bool _isShuffle = true;

        public bool IsShuffle
        {
            get { return _isShuffle; }
            set
            {
                if (_isShuffle == value) return;
                _isShuffle = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        private LibraryDataGridSortingCommand _librarydataGridSortingCommand;

        public LibraryDataGridSortingCommand LibraryDataGridSortingCommand
        {
            get
            {
                return _librarydataGridSortingCommand ??
                       (_librarydataGridSortingCommand = new LibraryDataGridSortingCommand(Libraries));
            }
        }

        private CancelableDataGridSortingCommand _groupdataGridSortingCommand;

        public CancelableDataGridSortingCommand GroupDataGridSortingCommand
        {
            get
            {
                return _groupdataGridSortingCommand ??
                       (_groupdataGridSortingCommand = new CancelableDataGridSortingCommand(Groups,
                           _ => { if (CurrentGroup != null) CurrentGroup.Model.IsSelected = false; }));
            }
        }

        private DelegateCommand<MouseButtonEventArgs> _doubleClickLibraryCommand;

        public DelegateCommand<MouseButtonEventArgs> DoubleClickLibraryCommand
        {
            get
            {
                return _doubleClickLibraryCommand ??
                       (_doubleClickLibraryCommand = new DelegateCommand<MouseButtonEventArgs>(
                           args =>
                           {
                               var dep = args.OriginalSource as DependencyObject;
                               while (dep != null && !(dep is DataGridRow))
                               {
                                   dep = VisualTreeHelper.GetParent(dep);
                               }

                               if (dep is DataGridRow)
                               {
                                   // 行がクリックされたときだけ処理する
                                   SelectLibrary.Model.DebugWriteJson();
                                   _client.InterruptThrow(SelectLibrary.Model);
                               }
                           }));
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FinalstreamCommons.Collections;
using FinalstreamCommons.Frameworks;
using FinalstreamCommons.Frameworks.Actions;
using FinalstreamCommons.Utils;
using Livet;
using Movselex.Core.Models;
using Movselex.Core.Models.Actions;
using NLog;

namespace Movselex.Core
{
    internal class MovselexClient : AppClient, IMovselexClient
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly string _appConfigFilePath;
        private readonly ActionExecuter<MovselexClient> _actionExecuter;
        private readonly IMovselexDatabaseAccessor _databaseAccessor;
        private BackgroundWorker _backgroundWorker;

        #region Initializedイベント

        // Event object
        public event EventHandler Initialized;

        protected virtual void OnInitialized(EventArgs e)
        {
            var handler = this.Initialized;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Refreshedイベント

        // Event object
        public event EventHandler Refreshed;

        protected virtual void OnRefreshed(EventArgs e)
        {
            var handler = this.Refreshed;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        public SelectableObservableCollection<FilteringItem> Filterings { 
            get
            {
                return MovselexFiltering.FilteringItems;
            } 
        }
        public ObservableCollection<LibraryItem> Libraries
        {
            get
            {
                return MovselexLibrary.LibraryItems;
            }
        }
        public SelectableObservableCollection<GroupItem> Groups
        {
            get
            {
                return MovselexGroup.GroupItems;
            }
        }

        public ObservableCollection<PlayingItem> Playings
        {
            get
            {
                return MovselexPlaying.PlayingItems;
            }
        }

        public string ApplicationNameWithVersion
        {
            get
            {
                return string.Format("{0} ver.{1}", ExecutingAssemblyInfo.Product, ExecutingAssemblyInfo.FileVersion);
            }
        }

        public bool IsProgressing
        {
            set { ProgressInfo.UpdateProgress(value); }
            get { return ProgressInfo.IsProgressing; }
        }


        
        public INowPlayingInfo NowPlayingInfo { get; private set; }
        
        
        public ObservableCollection<string> Databases { get; private set; }
        public MovselexFiltering MovselexFiltering { get; private set; }
        public MovselexLibrary MovselexLibrary { get; private set; }
        public MovselexGroup MovselexGroup { get; private set; }
        public LibraryUpdater LibraryUpdater { get; private set; }
        public MovselexPlaying MovselexPlaying { get; private set; }
        
        public IProgressInfo ProgressInfo { get; private set; }

        private MovselexAppConfig _appConfig;
        public new MovselexAppConfig AppConfig
        {
            get
            {
                return _appConfig;
            }
            private set
            {
                _appConfig = value;
                base.AppConfig = value;
            }
        }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="executingAssembly"></param>
        /// <param name="appConfigFilePath"></param>
        public MovselexClient(Assembly executingAssembly, string appConfigFilePath)
            : base(executingAssembly)
        {
            _appConfigFilePath = appConfigFilePath;

            AppConfig = MovselexAppConfig.Empty;

            _actionExecuter = new ActionExecuter<MovselexClient>(this);
            _actionExecuter.ExecuteFailed += (sender, exception) => OnExceptionThrowed(exception);
            _databaseAccessor = new MovselexDatabaseAccessor(AppConfig);
            MovselexFiltering = new MovselexFiltering();
            MovselexPlaying = new MovselexPlaying(_databaseAccessor);
            MovselexGroup = new MovselexGroup(_databaseAccessor);
            MovselexLibrary = new MovselexLibrary(_databaseAccessor, MovselexGroup);
            Databases = new ObservableCollection<string>();
            NowPlayingInfo = new NowPlayingInfo();
            LibraryUpdater = new LibraryUpdater(MovselexLibrary, AppConfig.SupportExtentions);
            ProgressInfo = new ProgressInfo();
        }


        /// <summary>
        /// 初期化します。
        /// </summary>
        protected override void InitializeCore()
        {

            AppConfig.Update(LoadConfig<MovselexAppConfig>(_appConfigFilePath));
            AppConfig.AppVersion = ExecutingAssemblyInfo.FileVersion;

            UpgradeSchema();
            
            Initialize(AppConfig.SelectDatabase);
            
        }

        /// <summary>
        /// スキーマをアップグレードします。
        /// </summary>
        /// <remarks>アップグレードの必要がない場合は行いません。</remarks>
        private void UpgradeSchema()
        {
            _actionExecuter.Post(new MovselexSchemaUpgradeV1Action());
        }


        /// <summary>
        /// 終了します。
        /// </summary>
        protected override void FinalizeCore()
        {
            SaveConfig(_appConfigFilePath, AppConfig);
        }


        /// <summary>
        /// すべてのデータをリフレッシュします。
        /// </summary>
        /// <param name="filteringText"></param>
        public void Refresh()
        {
            var action = new RefreshAction(AppConfig.FilteringMode);
            action.AfterAction = () => OnRefreshed(EventArgs.Empty);
            _actionExecuter.Post(action);
        }

        public void ModifyIsPlayed(LibraryItem library)
        {
            _actionExecuter.Post(new ModifyIsPlayedAction(library));
        }

        public void ModifyIsFavorite(LibraryItem library)
        {
            _actionExecuter.Post(new ModifyIsFavoriteAction(library));
        }

        public void ModifyIsFavorite(GroupItem group)
        {
            _actionExecuter.Post(new ModifyIsFavoriteAction(group));
        }

        public void ModifyIsComplete(GroupItem group)
        {
            _actionExecuter.Post(new ModifyIsCompleteAction(group));
        }

        public void RegistFiles(IEnumerable<string> files)
        {
            var action = new RegistFileAction(files);
            _actionExecuter.Post(action);
        }

        public void MoveGroupDirectory(GroupItem group, string baseDirectory)
        {
            _actionExecuter.Post(new MoveGroupDirectoryAction(group, baseDirectory));
        }

        public void FilteringLibrary(string filteringText)
        {
            _actionExecuter.Post(new FilteringLibraryAction(filteringText));
        }

        

        public void ExecEmpty()
        {
            _actionExecuter.Post(new EmptyAction());
        }

        /// <summary>
        /// コールバック処理をPOSTします。
        /// </summary>
        /// <param name="action"></param>
        /// <remarks>Action内で非同期実行後にコールバックしたいときに使用します。</remarks>
        public void PostCallback(CallbackAction action)
        {
            _actionExecuter.Post(action);
        }

        public void Initialize(string databaseName)
        {
            _databaseAccessor.ChangeDatabase(databaseName);
            var action = new InitializeAction();
            action.AfterAction = () =>
            {
                // 初期化完了後
                var playerMediaCrawlerAction = new PlayerMediaCrawlerAction(AppConfig.MpcExePath);
                var playMonitoringAction = new PlayMonitoringAction(this.NowPlayingInfo);

                playerMediaCrawlerAction.Updated += (sender, info) => NowPlayingInfo.Update(info.Title, info.TimeString);
                playMonitoringAction.CountUpTimePlayed += (sender, l) => _actionExecuter.Post(new IncrementPlayCountAction(l));
                playMonitoringAction.SwitchTitle += (sender, s) => _actionExecuter.Post(new UpdateNowPlayInfoAction(s));

                _backgroundWorker = new BackgroundWorker(TimeSpan.FromMilliseconds(1000), new BackgroundAction[]
                {
                    playerMediaCrawlerAction, 
                    playMonitoringAction,
                    new AutoUpdateLibraryAction(this), 
                });
                _backgroundWorker.Start();
                OnInitialized(EventArgs.Empty);
            };
            _actionExecuter.Post(action);
        }

        public void ChangeDatabase(string databaseName)
        {
            if (databaseName == null) return;
            _databaseAccessor.ChangeDatabase(databaseName);
            LibraryUpdater.ClearSearchDirectoryPaths();
            //MovselexDatabaseAccessor = new MovselexDatabaseAccessor(databaseName);
            Refresh();
        }

        public void SwitchLibraryMode()
        {
            var libMode = AppConfig.LibraryMode;
            libMode++;
            if (libMode > Enum.GetValues(typeof (LibraryMode)).Cast<LibraryMode>().Max()) libMode =  LibraryMode.Normal;
            AppConfig.LibraryMode = libMode;
            Refresh();
        }

        /// <summary>
        /// フィルタを変更します。
        /// </summary>
        /// <param name="filteringItem"></param>
        public void ChangeFiltering(FilteringItem filteringItem)
        {
            if (Filterings.FirstOrDefault(x => x.IsSelected) != null) _log.Debug(Filterings.FirstOrDefault(x => x.IsSelected).DisplayValue);
            AppConfig.SelectFiltering = filteringItem.DisplayValue;
            AppConfig.FilteringMode = FilteringMode.SQL;

            // TODO: Action内で処理するようにする。
            Groups.ClearSelection();

            Refresh();
        }

        public void ChangeGroup(GroupItem groupItem)
        {
            if (Groups.FirstOrDefault(x => x.IsSelected) != null) _log.Debug(Groups.FirstOrDefault(x => x.IsSelected).GroupName);
            AppConfig.FilteringMode = FilteringMode.Group;

            // TODO: Action内で処理するようにする。
            Filterings.ClearSelection();

            Refresh();
        }

        public void TrimmingLibrary(int librarySelectIndex, bool isShuffle)
        {
            var action = new TrimmingLibraryAction(librarySelectIndex, isShuffle);
            action.AfterAction = () => OnRefreshed(EventArgs.Empty);
            _actionExecuter.Post(action);
        }

        public void Throw(int librarySelectIndex)
        {
            var libraries = MovselexLibrary.LibraryItems.Skip(librarySelectIndex).Take(AppConfig.MaxGenerateNum);
            _actionExecuter.Post(new ThrowAction(libraries));
        }

        public void InterruptThrow(int librarySelectIndex)
        {
            _actionExecuter.Post(new ThrowAction(MovselexLibrary.LibraryItems[librarySelectIndex]));
        }

        public void UpdateLibrary()
        {
            _actionExecuter.Post(new UpdateLibraryAction());
        }


        public void Grouping(string title, string keyword, IEnumerable<LibraryItem> libraries)
        {
            _actionExecuter.Post(new GroupingAction(title, keyword, libraries));
        }

        public void ModifyGroup(GroupItem group, string groupName, string keyword)
        {
            _actionExecuter.Post(new ModifyGroupAction(group, groupName, keyword));
        }

        public void UnGroupLibrary(LibraryItem[] selectLibraries)
        {
            _actionExecuter.Post(new UnGroupAction(selectLibraries));
        }

        public void SaveConfig()
        {
            SaveConfig(_appConfigFilePath, AppConfig);
        }

        public void DeleteLibrary(LibraryItem[] selectLibraries, bool isDeleteFile)
        {
            _actionExecuter.Post(new DeleteLibraryAction(selectLibraries, isDeleteFile));
        }

        public void GetCandidateGroupName(string groupName, Action<IEnumerable<string>> afterAction)
        {
            var action = new GetCandidateGroupNameAction(groupName);
            action.AfterAction = () => { afterAction.Invoke(action.CandidateGroupNames); };
            _actionExecuter.Post(action);
        }

        public void OpenLibraryFolder(LibraryItem libraryItem)
        {
            Process.Start("EXPLORER.EXE", string.Format("/n,/select,\"{0}\"", libraryItem.FilePath));
        }

        public void MoveLibraryFile(string moveDestDirectory, LibraryItem[] selectLibraries)
        {
            _actionExecuter.Post(new MoveLibraryFileAction(moveDestDirectory, selectLibraries));
        }

        public void ReloadFiltering()
        {
            _actionExecuter.Post(new ReloadFilteringAction());
        }

        #region Dispose

        // Flag: Has Dispose already been called?
        private bool disposed = false;
        

        // Public implementation of Dispose pattern callable by consumers.
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
                //_actionExecuter.Dispose();
                _actionExecuter.Dispose();
                if(_backgroundWorker != null) _backgroundWorker.Dispose();
                if (_databaseAccessor != null) _databaseAccessor.Dispose();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion

        
    }
}

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using FinalstreamCommons.Models;
using Livet;
using Movselex.Core.Models;
using Movselex.Core.Models.Actions;
using NLog;

namespace Movselex.Core
{
    internal class MovselexClient : CoreClient, IMovselexClient
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

        public ObservableCollection<FilteringItem> Filterings { 
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
        public ObservableCollection<GroupItem> Groups
        {
            get
            {
                return MovselexGroup.GroupItems;
            }
        }


        public bool IsProgressing
        {
            set { ProgressInfo.UpdateProgress(value); }
            get { return ProgressInfo.IsProgressing; }
        }

        public INowPlayingInfo NowPlayingInfo { get; private set; }
        public MovselexAppConfig AppConfig { get; private set; }
        public ObservableCollection<string> Databases { get; private set; }
        public MovselexFiltering MovselexFiltering { get; private set; }
        public MovselexLibrary MovselexLibrary { get; private set; }
        public MovselexGroup MovselexGroup { get; private set; }
        public LibraryUpdater LibraryUpdater { get; private set; }
        public LinkedListEx<string> PlayingList { get; private set; }
        public IProgressInfo ProgressInfo { get; private set; } 

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="appConfigFilePath"></param>
        public MovselexClient(Assembly executingAssembly, string appConfigFilePath)
            : base(executingAssembly)
        {
            _appConfigFilePath = appConfigFilePath;
            
            AppConfig = new MovselexAppConfig();
            _actionExecuter = new ActionExecuter<MovselexClient>(this);
            _databaseAccessor = new MovselexDatabaseAccessor(AppConfig);
            MovselexFiltering = new MovselexFiltering();
            MovselexGroup = new MovselexGroup(_databaseAccessor);
            MovselexLibrary = new MovselexLibrary(_databaseAccessor, MovselexGroup);
            Databases = new ObservableCollection<string>();
            NowPlayingInfo = new NowPlayingInfo();
            LibraryUpdater = new LibraryUpdater(MovselexLibrary, AppConfig.SupportExtentions);
            PlayingList = new LinkedListEx<string>();
            ProgressInfo = new ProgressInfo();
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        protected override void InitializeCore()
        {
            
            AppConfig.Update(LoadConfig<MovselexAppConfig>(_appConfigFilePath));

            var playerMediaCrawlerAction = new PlayerMediaCrawlerAction(AppConfig.MpcExePath);
            

            playerMediaCrawlerAction.Updated += (sender, info) => NowPlayingInfo.Update(info.Title, info.TimeString);

            _backgroundWorker = new BackgroundWorker(new []{ playerMediaCrawlerAction });
            _backgroundWorker.Start();

            
            Initialize(AppConfig.SelectDatabase);
            
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
        public void Refresh()
        {
            var action = new RefreshAction(AppConfig.FilteringMode);
            action.AfterAction = () => OnRefreshed(EventArgs.Empty);
            _actionExecuter.Post(action);
        }

        public void ExecEmpty()
        {
            _actionExecuter.Post(new EmptyAction());
        }


        public void Initialize(string databaseName)
        {
            _databaseAccessor.ChangeDatabase(databaseName);
            var action = new InitializeAction();
            action.AfterAction = () => OnInitialized(EventArgs.Empty);
            _actionExecuter.Post(action);
        }


        public void ChangeDatabase(string databaseName)
        {
            if (databaseName == null) return;
            _databaseAccessor.ChangeDatabase(databaseName);
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

        public void ChangeFiltering(FilteringItem filteringItem)
        {
            if (Filterings.FirstOrDefault(x => x.IsSelected) != null) _log.Debug(Filterings.FirstOrDefault(x => x.IsSelected).DisplayValue);
            AppConfig.SelectFiltering = filteringItem.DisplayValue;
            Refresh();
        }

        public void ShuffleLibrary()
        {
            var action = new ShuffleLibraryAction(AppConfig.LimitNum);
            action.AfterAction = () => OnRefreshed(EventArgs.Empty);
            _actionExecuter.Post(action);
        }

        public void Throw(int librarySelectIndex)
        {
            var filePaths = MovselexLibrary.LibraryItems.Skip(librarySelectIndex).Take(AppConfig.LimitNum).Select(x=>x.FilePath);
            _actionExecuter.Post(new ThrowAction(filePaths.ToArray()));
        }

        public void InterruptThrow(int librarySelectIndex)
        {
            _actionExecuter.Post(new ThrowAction(MovselexLibrary.LibraryItems[librarySelectIndex].FilePath));
        }

        public void UpdateLibrary()
        {
            _actionExecuter.Post(new UpdateLibraryAction());
        }


        #region Dispose

        // Flag: Has Dispose already been called?
        private bool disposed = false;
        

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
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

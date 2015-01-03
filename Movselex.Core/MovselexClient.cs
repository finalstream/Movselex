using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons;
using FinalstreamCommons.Models;
using Livet;
using Livet.EventListeners;
using Movselex.Core.Models;
using Movselex.Core.Models.Actions;
using NLog;

namespace Movselex.Core
{
    internal class MovselexClient : CoreClient, IMovselexClient
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public IEnumerable<FilteringItem> Filterings { 
            get
            {
                return MovselexFiltering.FilteringItems;
            } 
        }
        public IEnumerable<LibraryItem> Libraries
        {
            get
            {
                return MovselexLibrary.LibraryItems;
            }
        }
        public INowPlayingInfo NowPlayingInfo { get; private set; }
        public MovselexAppConfig AppConfig { get; private set; }
        public MovselexFiltering MovselexFiltering { get; private set; }
        public MovselexLibrary MovselexLibrary { get; private set; }
        private readonly string _appConfigFilePath;
        private readonly ActionExecuter<MovselexClient> _actionExecuter;
        private readonly IDatabaseAccessor _databaseAccessor;
        

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="appConfigFilePath"></param>
        public MovselexClient(string appConfigFilePath) : base()
        {
            _appConfigFilePath = appConfigFilePath;
            
            AppConfig = new MovselexAppConfig();
            _actionExecuter = new ActionExecuter<MovselexClient>(this);
            _databaseAccessor = new DatabaseAccessor(AppConfig.SelectDatabase);
            MovselexFiltering = new MovselexFiltering();
            MovselexLibrary = new MovselexLibrary(_databaseAccessor);
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        protected override void InitializeCore()
        {
            AppConfig.Update(LoadConfig<MovselexAppConfig>(_appConfigFilePath));

            NowPlayingInfo = new NowPlayingInfo("ここにたいとるがはいります。");

            Refresh();

            _log.Debug("Initialized MovselexClinet.");
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
            PostAction(new RefreshAction());
        }

        public void ExecEmpty()
        {
            PostAction(new EmptyAction());
        }

        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="action"></param>
        private void PostAction(IGeneralAction<MovselexClient> action)
        {
            _actionExecuter.Post(action);
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
                if (_databaseAccessor != null) _databaseAccessor.Dispose();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion
    }

}

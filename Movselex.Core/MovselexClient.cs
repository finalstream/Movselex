using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons;
using FinalstreamCommons.Models;
using Movselex.Core.Actions;
using Movselex.Core.Models;
using NLog;

namespace Movselex.Core
{
    internal class MovselexClient : CoreClient, IMovselexClient
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public IMovselexFiltering Filterings { get; private set; }
        public INowPlayingInfo NowPlayingInfo { get; private set; }
        public MovselexAppConfig AppConfig { get; private set; }

        private readonly string _appConfigFilePath;
        private readonly ActionExecuter<IMovselexClient> _actionExecuter; 

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="appConfigFilePath"></param>
        public MovselexClient(string appConfigFilePath)
        {
            _appConfigFilePath = appConfigFilePath;
            AppConfig = LoadConfig<MovselexAppConfig>(_appConfigFilePath);

            _actionExecuter = new ActionExecuter<IMovselexClient>(this);
        }

        /// <summary>
        /// 初期化します。
        /// </summary>
        protected override void InitializeCore()
        {

            var filtering = new MovselexFiltering();
            filtering.Load();
            Filterings = filtering;

            NowPlayingInfo = new NowPlayingInfo("ここにたいとるがはいります。");

            _log.Debug("Initialized MovselexClinet.");
        }

        /// <summary>
        /// 終了します。
        /// </summary>
        protected override void FinalizeCore()
        {
            SaveConfig(_appConfigFilePath, AppConfig);
        }


        public void ExecEmpty()
        {
            PostAction(new EmptyAction());
        }

        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="action"></param>
        private void PostAction(IGeneralAction<IMovselexClient> action)
        {
            _actionExecuter.Post(action);
        }
    }
}

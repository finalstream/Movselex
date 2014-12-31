using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons;
using FinalstreamCommons.Models;
using Movselex.Core.Models;
using NLog;

namespace Movselex.Core
{
    internal class MovselexClient : CoreClient, IMovselexClient
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public IMovselexFiltering Filterings { get; private set; }
        public INowPlayingInfo NowPlayingInfo { get; private set; }

        public MovselexClient()
        {
          
        }

        protected override void InitializeCore()
        {
            var filtering = new MovselexFiltering();
            filtering.Load();
            Filterings = filtering;

            NowPlayingInfo = new NowPlayingInfo("ここにたいとるがはいります。");

            _log.Debug("Initialized MovselexClinet.");
        }
    }
}

using System;
using FinalstreamCommons.Models;
using NLog;

namespace Movselex.Core.Models.Actions
{
    internal abstract class MovselexActionBase  : IGeneralAction<MovselexClient>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public Action AfterAction { get; set; }

        public void Invoke(MovselexClient client)
        {
            InvokeCore(client);
            if (AfterAction != null) AfterAction.Invoke();
            _log.Debug("Executed Action.");
        }

        public abstract void InvokeCore(MovselexClient client);
    }
}

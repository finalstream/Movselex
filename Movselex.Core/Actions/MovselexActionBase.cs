using System;
using FinalstreamCommons.Models;
using NLog;

namespace Movselex.Core.Actions
{
    internal abstract class MovselexActionBase  : IGeneralAction<IMovselexClient>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void Invoke(IMovselexClient client)
        {
            InvokeCore(client);
            _log.Debug("Executed Action.");
        }

        public abstract void InvokeCore(IMovselexClient client);
    }
}

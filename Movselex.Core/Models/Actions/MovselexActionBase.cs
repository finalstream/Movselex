using FinalstreamCommons.Models;
using NLog;

namespace Movselex.Core.Models.Actions
{
    internal abstract class MovselexActionBase  : IGeneralAction<MovselexClient>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        public void Invoke(MovselexClient client)
        {
            InvokeCore(client);
            _log.Debug("Executed Action.");
        }

        public abstract void InvokeCore(MovselexClient client);
    }
}

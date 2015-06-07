using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Frameworks.Actions;

namespace Movselex.Core.Models.Actions
{
    class AutoUpdateLibraryAction : BackgroundIntervalAction
    {

        private readonly MovselexClient _client;

        public AutoUpdateLibraryAction(MovselexClient client)
        {
            _client = client;
        }

        protected override void InvokeCore()
        {
            _client.UpdateLibrary();

        }
    }
}

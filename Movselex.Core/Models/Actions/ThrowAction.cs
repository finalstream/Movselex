using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    class ThrowAction : MovselexActionBase
    {

        public string[] FilePaths { get; private set; }

        public ThrowAction(string[] filePaths)
        {
            FilePaths = filePaths;
        }

        public override void InvokeCore(MovselexClient client)
        {
            var thrower = new MediaPlayerClassicThrower(client.AppConfig);

            thrower.Throw(FilePaths);
        }
    }
}

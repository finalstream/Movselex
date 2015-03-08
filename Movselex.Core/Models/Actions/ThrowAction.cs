using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    class ThrowAction : MovselexActionBase
    {
        enum Mode
        {
            Interrupt,
            Throw
        }

        private readonly Mode _mode;

        public string[] FilePaths { get; private set; }

        public ThrowAction(string filePath)
        {
            _mode = Mode.Interrupt;
            FilePaths = new []{filePath};
        }

        public ThrowAction(string[] filePaths)
        {
            _mode = Mode.Throw;
            FilePaths = filePaths;
        }

        public override void InvokeCore(MovselexClient client)
        {
            var thrower = new MediaPlayerClassicThrower(client.AppConfig);

            if (_mode == Mode.Interrupt) FilePaths = FilePaths.Concat(client.PlayingList).ToArray();

            thrower.Throw(FilePaths);
            client.PlayingList.Reset(FilePaths);
        }
    }
}

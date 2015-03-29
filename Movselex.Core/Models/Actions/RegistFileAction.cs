using System;
using System.Collections.Generic;
using System.Linq;

namespace Movselex.Core.Models.Actions
{
    internal class RegistFileAction : MovselexProgressAction
    {
        private readonly string[] _files;

        public RegistFileAction(IEnumerable<string> files)
        {
            _files = files.ToArray();
        }

        public override void InvokeProgress(MovselexClient client)
        {
            client.LibraryUpdater.RegistFiles(_files);
        }
    }
}
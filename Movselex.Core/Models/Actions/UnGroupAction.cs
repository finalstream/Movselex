using System;

namespace Movselex.Core.Models.Actions
{
    internal class UnGroupAction : MovselexActionBase
    {

        private readonly LibraryItem[] _libraries;

        public UnGroupAction(LibraryItem[] selectLibraries)
        {
            _libraries = selectLibraries;
        }

        public override void InvokeCore(MovselexClient client)
        {
            client.MovselexLibrary.UnGroup(_libraries);
        }
    }
}
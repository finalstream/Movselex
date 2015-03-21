using System;
using FinalstreamCommons.Models;
using Movselex.Core.Models;
using Movselex.Core.Models.Actions;

namespace Movselex.Core
{
    internal class ModifyIsPlayedAction : MovselexActionBase
    {
        private readonly LibraryItem _libraryItem;

        public ModifyIsPlayedAction(LibraryItem library)
        {
            _libraryItem = library;
        }

        public override void InvokeCore(MovselexClient client)
        {
            client.MovselexLibrary.ModifyIsPlayed(_libraryItem);
        }
    }
}
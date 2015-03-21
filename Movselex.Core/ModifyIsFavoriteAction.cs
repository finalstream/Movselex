using System;
using Movselex.Core.Models;
using Movselex.Core.Models.Actions;

namespace Movselex.Core
{
    internal class ModifyIsFavoriteAction : MovselexActionBase
    {
        private readonly LibraryItem _libraryItem;

        public ModifyIsFavoriteAction(LibraryItem library)
        {
            _libraryItem = library;
        }

        public override void InvokeCore(MovselexClient client)
        {
            client.MovselexLibrary.ModifyIsFavorite(_libraryItem);
        }
    }
}
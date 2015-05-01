using System;

namespace Movselex.Core.Models.Actions
{
    internal class DeleteLibraryAction : MovselexActionBase
    {

        private LibraryItem[] _selectLibraryItems;
        private bool _isDeleteFile;

        public DeleteLibraryAction(LibraryItem[] selectLibraries, bool isDeleteFile)
        {
            _selectLibraryItems = selectLibraries;
            _isDeleteFile = isDeleteFile;
        }

        public override void InvokeCore(MovselexClient client)
        {
            // TODO: 削除後、グループも更新する
            foreach (var libraryItem in _selectLibraryItems)
            {
                client.MovselexLibrary.Delete(libraryItem, _isDeleteFile);
            }
        }
    }
}
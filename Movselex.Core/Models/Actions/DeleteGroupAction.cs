using System;
using System.Collections.Generic;
using System.Linq;
using Firk.Core.Actions;
using Movselex.Core.Models;

namespace Movselex.Core
{
    internal class DeleteGroupAction : IGeneralAction<MovselexClient>
    {
        private readonly GroupItem _group;
        private readonly bool _isDeleteFile;

        public DeleteGroupAction(GroupItem group, bool isDeleteFile)
        {
            _group = group;
            _isDeleteFile = isDeleteFile;
        }

        public void Invoke(MovselexClient _)
        {
            var libraries = Enumerable.Empty<LibraryItem>();

            if (_isDeleteFile) libraries = _.MovselexGroup.GetLibraryIds(_group.Gid).Select(x=> _.MovselexLibrary.GetLibraryItem(x)).ToArray();

            _.MovselexGroup.DeleteGroup(_group);
            foreach (var libraryItem in _.MovselexLibrary.LibraryItems)
            {
                libraryItem.UnGroup();
            }

            // ファイルも削除する(削除する場合は事前にライブラリを取得)
            foreach (var library in libraries)
            {
                _.MovselexLibrary.Delete(library, true);
            }
        }
    }
}
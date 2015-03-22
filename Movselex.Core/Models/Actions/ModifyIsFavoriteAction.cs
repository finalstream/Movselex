using System.Linq;

namespace Movselex.Core.Models.Actions
{
    internal class ModifyIsFavoriteAction : MovselexActionBase
    {
        private readonly LibraryItem _libraryItem;
        private readonly GroupItem _groupItem;

        public ModifyIsFavoriteAction(LibraryItem library)
        {
            _libraryItem = library;
        }

        public ModifyIsFavoriteAction(GroupItem group)
        {
            _groupItem = group;
        }

        public override void InvokeCore(MovselexClient client)
        {
            // TODO: インタフェース化して共通化する。
            if (_libraryItem != null)
            {
                var newRating = _libraryItem.IsFavorite ? RatingType.Normal : RatingType.Favorite; // 逆にする
                client.MovselexLibrary.ModifyRating(_libraryItem, newRating);
            }
            if (_groupItem != null)
            {
                var newRating = _groupItem.IsFavorite ? RatingType.Normal : RatingType.Favorite; // 逆にする
                client.MovselexGroup.ModifyRating(_groupItem, newRating);
                foreach (var library in client.MovselexLibrary.LibraryItems.Where(x => x.Gid == _groupItem.Gid))
                {
                    // ライブラリも更新する
                    library.ModifyRating(newRating);
                }
            }
        }
    }
}
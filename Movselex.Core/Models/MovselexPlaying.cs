using System.Collections.Generic;
using System.Linq;
using FinalstreamCommons.Collections;

namespace Movselex.Core.Models
{
    internal class MovselexPlaying
    {

        public ObservableCollectionEx<PlayingItem> PlayingItems { get; private set; }

        private List<PlayingItem> _playingList = new List<PlayingItem>();

        public MovselexPlaying()
        {
            PlayingItems = new ObservableCollectionEx<PlayingItem>();
        }

        public void Reset(IEnumerable<LibraryItem> libraryItems)
        {
            PlayingItems.Reset(ConvertPlayingItems(libraryItems));
            _playingList = PlayingItems.ToList();
        }

        private IEnumerable<PlayingItem> ConvertPlayingItems(IEnumerable<LibraryItem> libraries)
        {
            PlayingItem beforeItem = null;
            return libraries.Select(x =>
            {
                var item = new PlayingItem(x, beforeItem);
                beforeItem = item;
                return item;
            });
        }

        public void Refresh(long id)
        {
            PlayingItems.Reset(ConvertPlayingItems(_playingList.Skip(_playingList.FindIndex(x => x.Item.Id == id)).Select(x => x.Item)));
        }
    }
}
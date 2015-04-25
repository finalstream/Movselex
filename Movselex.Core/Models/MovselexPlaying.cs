using System.Collections.Generic;
using FinalstreamCommons.Collections;

namespace Movselex.Core.Models
{
    internal class MovselexPlaying
    {

        public ObservableCollectionEx<PlayingItem> PlayingItems { get; private set; }

        public MovselexPlaying()
        {
            PlayingItems = new ObservableCollectionEx<PlayingItem>();
        }

        public void Reset(IEnumerable<PlayingItem> playingItems)
        {
            PlayingItems.Reset(playingItems);
        }
    }
}
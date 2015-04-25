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

        private PlayingItem[] _playingItems; 

        public ThrowAction(LibraryItem library)
        {
            _mode = Mode.Interrupt;
            _playingItems = new[] { new PlayingItem(library) };
        }

        public ThrowAction(IEnumerable<LibraryItem> libraries)
        {
            _mode = Mode.Throw;

            _playingItems = CreatePlayingItems(libraries);
        }

        public override void InvokeCore(MovselexClient client)
        {
            var playing = client.MovselexPlaying;
            var thrower = new MediaPlayerClassicThrower(client.AppConfig);

            if (_mode == Mode.Interrupt) _playingItems = CreatePlayingItems(_playingItems.Select(x=>x.Item).Concat(playing.PlayingItems.Select(x=>x.Item)));

            thrower.Throw(_playingItems.Select(x => x.Item.FilePath));
            playing.Reset(_playingItems);
        }

        private PlayingItem[] CreatePlayingItems(IEnumerable<LibraryItem> libraries)
        {
            PlayingItem beforeItem = null;
            return libraries.Select(x =>
            {
                var item = new PlayingItem(x, beforeItem);
                beforeItem = item;
                return item;
            }).ToArray();
        }
    }
}

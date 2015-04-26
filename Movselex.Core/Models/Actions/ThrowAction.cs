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

        private IEnumerable<LibraryItem> _libraryItems; 

        public ThrowAction(LibraryItem library)
        {
            _mode = Mode.Interrupt;
            _libraryItems = new[] { library };
        }

        public ThrowAction(IEnumerable<LibraryItem> libraries)
        {
            _mode = Mode.Throw;
            _libraryItems = libraries;
        }

        public override void InvokeCore(MovselexClient client)
        {
            var playing = client.MovselexPlaying;
            var thrower = new MediaPlayerClassicThrower(client.AppConfig);

            if (_mode == Mode.Interrupt) _libraryItems = _libraryItems.Concat(playing.PlayingItems.Select(x=>x.Item));

            var libraryItems = _libraryItems as LibraryItem[] ?? _libraryItems.ToArray();
            thrower.Throw(libraryItems.Select(x => x.FilePath));
            playing.Reset(libraryItems);

        }

        
    }
}

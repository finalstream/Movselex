﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    class ThrowAction : MovselexAction
    {
        enum Mode
        {
            Interrupt,
            Throw
        }

        private readonly Mode _mode;

        private IEnumerable<LibraryItem> _libraryItems;

        private long _id = -1;

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

        public ThrowAction(long id)
        {
            _mode = Mode.Interrupt;
            _id = id;
        }

        public override void InvokeCore(MovselexClient client)
        {
            var playing = client.MovselexPlaying;
            var thrower = new MediaPlayerClassicThrower(client.AppConfig);

            if (_id != -1)
            {
                // id指定の場合
                _libraryItems = new[] {client.MovselexLibrary.GetLibraryItem(_id)};
            }

            if (_mode == Mode.Interrupt)
            {
                var resumeItems = playing.GetResumePlayingItems();

                // 再生位置が終盤であれば現在の再生中のものは除外する。
                var nowplaying = client.NowPlayingInfo;
                if (nowplaying.TotalPlayTime.Subtract(nowplaying.NowPlayTime).Duration().TotalMinutes < 5)
                {
                    resumeItems = resumeItems.Skip(1);
                }

                _libraryItems = _libraryItems.Concat(resumeItems);
                
            }

            var libraryItems = _libraryItems as LibraryItem[] ?? _libraryItems.ToArray();
            thrower.Throw(libraryItems.Select(x => x.FilePath));
            playing.Reset(libraryItems);

        }

        
    }
}

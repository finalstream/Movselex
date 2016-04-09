using System.Collections.Generic;
using System.Linq;
using System.Text;
using FinalstreamCommons.Collections;

namespace Movselex.Core.Models
{
    internal class MovselexPlaying
    {

        public ObservableCollectionEx<PlayingItem> PlayingItems { get; private set; }

        private List<PlayingItem> _playingList = new List<PlayingItem>();

        private readonly IMovselexDatabaseAccessor _databaseAccessor;

        public MovselexPlaying(IMovselexDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
            PlayingItems = new ObservableCollectionEx<PlayingItem>();
        }

        public IEnumerable<LibraryItem> GetResumePlayingItems()
        {
            var playings = PlayingItems.Select(x => x.Item).ToArray();
            return playings.Concat(_playingList.Select(x=>x.Item).Except(playings));
        }

        public void Reset(IEnumerable<LibraryItem> libraryItems)
        {
            PlayingItems.Reset(ConvertPlayingItems(libraryItems));
            _playingList = PlayingItems.ToList();

            // 再生中リストをデータベースに登録
            using (var tran = _databaseAccessor.BeginTransaction())
            {
                _databaseAccessor.DeletePlayingList();
                var sort = 1;
                foreach (var playingItem in _playingList)
                {
                    _databaseAccessor.InsertPlayingList(playingItem.Item.Id, sort++);
                }
                tran.Commit();
            }
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

        /// <summary>
        /// ベースから再生中のものを先頭になるようにリストを更新します。
        /// </summary>
        /// <param name="library"></param>
        public void Refresh(LibraryItem library)
        {
            var playingIndex = _playingList.FindIndex(x => x.Item.Id == library.Id);
            if (playingIndex != -1)
            {
                PlayingItems.Reset(ConvertPlayingItems(_playingList.Skip(playingIndex).Select(x => x.Item)));
            }
            else
            {
                // 存在しない場合は再生中のものだけ表示
                PlayingItems.Reset(ConvertPlayingItems(new []{library}));
            }
        }

        public void Load()
        {
            if (_playingList.Count != 0) return; // ロード済みの場合は何もしない。

            var libraries = _databaseAccessor.SelectPlayingList();

            _playingList = ConvertPlayingItems(libraries).ToList();
        }
    }
}
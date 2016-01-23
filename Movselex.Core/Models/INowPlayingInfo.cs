using System;

namespace Movselex.Core.Models
{
    public interface INowPlayingInfo
    {

        long Id { get; }

        /// <summary>
        /// 再生中のタイトル。
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 再生中のタイトル(表示用)。
        /// </summary>
        string ViewTitle { get; }

        /// <summary>
        /// 再生時間(mm:ss / mm:ss)。
        /// </summary>
        string ViewPlayTime { get; }

        /// <summary>
        /// 現在再生時間
        /// </summary>
        TimeSpan NowPlayTime { get; }

        /// <summary>
        /// 合計再生時間
        /// </summary>
        TimeSpan TotalPlayTime { get; }


        string Season { get; }

        bool CanPrevious { get; }

        bool CanNext { get; }

        long PreviousId { get; }

        long NextId { get; }

        LibraryItem Library { get; }

        /// <summary>
        /// 更新します。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="playtime"></param>
        void Update(string title, string playtime);

        void SetId(long id);
        void SetLibrary(LibraryItem library);
        void SetPreviousAndNextId(Tuple<long?, long?> prevnextId);
    }
}
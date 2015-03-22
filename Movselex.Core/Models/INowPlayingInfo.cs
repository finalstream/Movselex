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
        /// 再生時間(mm:ss)。
        /// </summary>
        string PlayTime { get; }

        double PlayTimeSeconds { get; }

        /// <summary>
        /// 更新します。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="playtime"></param>
        void Update(string title, string playtime);

        void SetId(long id);
    }
}
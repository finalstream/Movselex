namespace Movselex.Core.Models
{
    public interface INowPlayingInfo
    {
        /// <summary>
        /// 再生中のタイトル。
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 再生時間。
        /// </summary>
        string PlayTime { get; }

        /// <summary>
        /// 更新します。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="playtime"></param>
        void Update(string title, string playtime);
    }
}
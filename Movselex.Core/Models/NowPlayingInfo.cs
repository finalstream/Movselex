namespace Movselex.Core.Models
{
    internal class NowPlayingInfo : INowPlayingInfo
    {

        public string Title { get; private set; }

        public NowPlayingInfo(string title)
        {
            this.Title = title;
        }
    }
}
namespace Movselex.Core.Models
{
    internal class PlayerMediaInfo
    {

        public static PlayerMediaInfo Empty = new PlayerMediaInfo(null, null);

        public string Title { get; private set; }

        public string TimeString { get; private set; }

        public PlayerMediaInfo(string title, string timeString)
        {
            Title = title;
            TimeString = timeString;
        }
    }
}
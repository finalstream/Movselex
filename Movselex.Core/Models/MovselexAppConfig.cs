using System.Windows;
using FinalstreamCommons.Models;

namespace Movselex.Core.Models
{
    public class MovselexAppConfig : IAppConfig
    {
        public string AppVersion { get; private set; }
        
        public Rect WindowBounds { get; private set; }

        public string PlayerExePath { get; private set; }

        public int ScreenNo { get; private set; }

        public bool IsFullScreen { get; private set; }

        public string[] SupportExtentions { get; private set; }

        public int LimitNum { get; private set; }

        public string MoveBaseDirectory { get; private set; }

        public string SelectDatabase { get; private set; }

        public int PlayCountUpMinutes { get; private set; }

        public int SelectFilteringIndex { get; private set; }

        public string TitleFormat { get; private set; }

        public int LibraryMode { get; private set; }


        public MovselexAppConfig()
        {
            AppVersion = "";
            WindowBounds = default(Rect);
            PlayerExePath = "";
            ScreenNo = 1;
            IsFullScreen = false;
            SupportExtentions = new[] { ".avi",".mpg",".mp4",".mkv",".flv",".wmv" };
            LimitNum = 50;
            MoveBaseDirectory = "";
            SelectDatabase = "default";
            PlayCountUpMinutes = 10;
            SelectFilteringIndex = 0;
            TitleFormat = "%TITLE% - %NO%";
            LibraryMode = 0;
        }

        public MovselexAppConfig(string appVersion, Rect windowBounds, string playerExePath, int screenNo, bool isFullScreen, string[] supportExtentions, int limitNum, string moveBaseDirectory, string selectDatabase, int playCountUpMinutes, int selectFilteringIndex, string titleFormat, int libraryMode)
        {
            AppVersion = appVersion;
            WindowBounds = windowBounds;
            PlayerExePath = playerExePath;
            ScreenNo = screenNo;
            IsFullScreen = isFullScreen;
            SupportExtentions = supportExtentions;
            LimitNum = limitNum;
            MoveBaseDirectory = moveBaseDirectory;
            SelectDatabase = selectDatabase;
            PlayCountUpMinutes = playCountUpMinutes;
            SelectFilteringIndex = selectFilteringIndex;
            TitleFormat = titleFormat;
            LibraryMode = libraryMode;
        }

    }
}
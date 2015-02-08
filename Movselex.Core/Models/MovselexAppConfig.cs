using System.Windows;
using System.Windows.Media;
using FinalstreamCommons.Models;
using Livet;

namespace Movselex.Core.Models
{
    public class MovselexAppConfig : NotificationObject, IAppConfig
    {

        public string AppVersion { get; private set; }
        
        public Rect WindowBounds { get; private set; }

        public string PlayerExePath { get; private set; }

        public int ScreenNo { get; private set; }

        public bool IsFullScreen { get; private set; }

        public string[] SupportExtentions { get; private set; }

        public int LimitNum { get; private set; }

        public string MoveBaseDirectory { get; private set; }

        public string SelectDatabase { get; set; }

        public int PlayCountUpMinutes { get; private set; }

        public string SelectFiltering { get; set; }

        public string TitleFormat { get; private set; }

        public FilteringMode FilteringMode { get; private set; }

        public string MpcExePath { get; set; }


        #region LibraryMode変更通知プロパティ

        private LibraryMode _libraryMode;

        public LibraryMode LibraryMode
        {
            get { return _libraryMode; }
            set
            {
                if (_libraryMode == value) return;
                _libraryMode = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        //public Color AccentColor { get; private set; }

        #region AccentColor変更通知プロパティ

        private Color _accentColor;

        public Color AccentColor
        {
            get { return _accentColor; }
            set
            {
                if (_accentColor == value) return;
                _accentColor = value;
                RaisePropertyChanged();
            }
        }

        #endregion


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
            SelectDatabase = "library";
            PlayCountUpMinutes = 10;
            SelectFiltering = "ALL MOVIE";
            TitleFormat = "%TITLE% - %NO%";
            LibraryMode = LibraryMode.Normal;
            AccentColor = Colors.Orange;
            FilteringMode = FilteringMode.SQL;
            MpcExePath = "";
        }

        public MovselexAppConfig(string appVersion, Rect windowBounds, string playerExePath, int screenNo, bool isFullScreen, string[] supportExtentions, int limitNum, string moveBaseDirectory, string selectDatabase, int playCountUpMinutes, string selectFiltering, string titleFormat, FilteringMode filteringMode, string mpcExePath)
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
            SelectFiltering = selectFiltering;
            TitleFormat = titleFormat;
            FilteringMode = filteringMode;
            MpcExePath = mpcExePath;
        }

        
        public void Update(MovselexAppConfig newConfig)
        {
            // MEMO: PropertyChangedEventをひろうためにめんどーだけどひとつずつ設定する。
            AppVersion = newConfig.AppVersion;
            WindowBounds = newConfig.WindowBounds;
            PlayerExePath = newConfig.PlayerExePath;
            ScreenNo = newConfig.ScreenNo;
            IsFullScreen = newConfig.IsFullScreen;
            SupportExtentions = newConfig.SupportExtentions;
            LimitNum = newConfig.LimitNum;
            MoveBaseDirectory = newConfig.MoveBaseDirectory;
            SelectDatabase = newConfig.SelectDatabase;
            PlayCountUpMinutes = newConfig.PlayCountUpMinutes;
            SelectFiltering = newConfig.SelectFiltering;
            TitleFormat = newConfig.TitleFormat;
            LibraryMode = newConfig.LibraryMode;
            AccentColor = newConfig.AccentColor;
            FilteringMode = newConfig.FilteringMode;
            MpcExePath = newConfig.MpcExePath;

        }
    }
}
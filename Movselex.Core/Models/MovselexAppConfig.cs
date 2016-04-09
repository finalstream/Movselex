using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Firk.Core;
using Livet;

namespace Movselex.Core.Models
{

    public class MovselexAppConfig : AppConfig, IMovselexAppConfig
    {
        
        public string PlayerExePath { get; private set; }

        public int ScreenNo { get; set; }

        public bool IsFullScreen { get; set; }

        public string[] SupportExtentions { get; set; }

        public int MaxGenerateNum { get; set; }

        public int MaxLimitNum { get; set; }

        public string MoveBaseDirectory { get; set; }

        public string SelectDatabase { get; set; }

        public string SelectFiltering { get; set; }

        public string TitleFormat { get; private set; }

        public FilteringMode FilteringMode { get; set; }

        #region MpcExePath変更通知プロパティ

        private string _mpcExePath;

        public string MpcExePath
        {
            get { return _mpcExePath; }
            set
            {
                if (_mpcExePath == value) return;
                _mpcExePath = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Language変更通知プロパティ

        private string _language;

        public string Language
        {
            get { return _language; }
            set
            {
                if (_language == value) return;
                _language = value;
                RaisePropertyChanged();
            }
        }

        #endregion

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

        #region SelectedTheme変更通知プロパティ

        private string _selectedTheme;

        public string SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (_selectedTheme == value) return;
                _selectedTheme = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region MonitorDirectories変更通知プロパティ

        private Collection<string> _monitorDirectories;

        public Collection<string> MonitorDirectories
        {
            get { return _monitorDirectories; }
            set
            {
                if (_monitorDirectories == value) return;
                _monitorDirectories = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /*
        public MovselexAppConfig()
        {
            AppVersion = "";
            SchemaVersion = ApplicationDefinitions.CurrentSchemaVersion;
            WindowBounds = new Rect(100d, 100d, 1100d, 700d);
            PlayerExePath = "";
            ScreenNo = 1;
            IsFullScreen = false;
            SupportExtentions = new[] { ".avi",".mpg",".mp4",".mkv",".flv",".wmv" };
            MaxGenerateNum = 30;
            MoveBaseDirectory = "";
            SelectDatabase = "library";
            SelectFiltering = "ALL MOVIE";
            TitleFormat = "%TITLE% - %NO%";
            LibraryMode = LibraryMode.Normal;
            AccentColor = Color.FromRgb(0x1b, 0xa1, 0xe2);
            SelectedTheme = "light";
            FilteringMode = FilteringMode.SQL;
            MpcExePath = "";
        }*/

        public MovselexAppConfig()
        {
            AppVersion = "";
            SchemaVersion = ApplicationDefinitions.CurrentSchemaVersion;
            WindowBounds = new Rect(100d, 100d, 1100d, 700d);
            PlayerExePath = "";
            ScreenNo = 1;
            IsFullScreen = false;
            SupportExtentions = new[] {".avi", ".mpg", ".mp4", ".mkv", ".flv", ".wmv"};
            MaxGenerateNum = 30;
            MaxLimitNum = 100;
            MoveBaseDirectory = "";
            SelectDatabase = "library";
            SelectFiltering = "ALL MOVIE";
            TitleFormat = "%TITLE% - %NO%";
            LibraryMode = LibraryMode.Normal;
            AccentColor = Color.FromRgb(0x1b, 0xa1, 0xe2);
            SelectedTheme = "light";
            FilteringMode = FilteringMode.SQL;
            MpcExePath = "";
            Language = CultureInfo.CurrentUICulture.Parent.Name;
            MonitorDirectories = new Collection<string>();
        }

        protected override void UpdateCore<T>(T config)
        {
            var newConfig = config as MovselexAppConfig;
            if (newConfig == null) return;
            // MEMO: PropertyChangedEventをひろうためにめんどーだけどひとつずつ設定する。
            PlayerExePath = newConfig.PlayerExePath;
            ScreenNo = newConfig.ScreenNo;
            IsFullScreen = newConfig.IsFullScreen;
            SupportExtentions = newConfig.SupportExtentions;
            if (newConfig.MaxGenerateNum != 0) MaxGenerateNum = newConfig.MaxGenerateNum;
            if (newConfig.MaxLimitNum != 0) MaxLimitNum = newConfig.MaxLimitNum;
            MoveBaseDirectory = newConfig.MoveBaseDirectory;
            SelectDatabase = newConfig.SelectDatabase;
            SelectFiltering = newConfig.SelectFiltering;
            TitleFormat = newConfig.TitleFormat;
            LibraryMode = newConfig.LibraryMode;
            AccentColor = newConfig.AccentColor;
            SelectedTheme = newConfig.SelectedTheme;
            FilteringMode = newConfig.FilteringMode;
            if (newConfig.MpcExePath != null) MpcExePath = newConfig.MpcExePath;
            if (newConfig.Language != null) Language = newConfig.Language;
            if (newConfig.MonitorDirectories != null) MonitorDirectories = newConfig.MonitorDirectories;

        }


    }
}
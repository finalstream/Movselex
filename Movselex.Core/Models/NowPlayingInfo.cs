using System;
using System.IO;
using Livet;

namespace Movselex.Core.Models
{
    internal class NowPlayingInfo : NotificationObject, INowPlayingInfo
    {
        public NowPlayingInfo()
        {
            _title = "";
            _totalPlayTime = TimeSpan.Zero;
        }

        public long Id { get; private set; }

        #region Library変更通知プロパティ

        private LibraryItem _Library;

        public LibraryItem Library
        {
            get { return _Library; }
            set
            {
                if (_Library == value) return;
                _Library = value;
                RaisePropertyChanged();
            }
        }

        #endregion


        #region ViewTitle変更通知プロパティ

        private string _viewTitle;

        public string ViewTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_viewTitle)) return "";
                return MovselexUtils.ReplaceTitle(_viewTitle);
            }
            set
            {
                if (_viewTitle == value) return;
                _viewTitle = value;
                RaisePropertyChanged();
            }
        }

        #endregion


        #region Title変更通知プロパティ

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                ViewTitle = value;
                RaisePropertyChanged();
            }
        }

        
        #endregion

        #region ViewPlayTime変更通知プロパティ

        private string _viewPlayTime;

        public string ViewPlayTime
        {
            get { return _viewPlayTime; }
            set
            {
                if (_viewPlayTime == value) return;
                _viewPlayTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region TotalPlayTime変更通知プロパティ

        private TimeSpan _totalPlayTime;

        public TimeSpan TotalPlayTime
        {
            get { return _totalPlayTime; }
            set
            {
                if (_totalPlayTime == value) return;
                _totalPlayTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region NowPlayTime変更通知プロパティ

        private TimeSpan _nowPlayTime;

        public TimeSpan NowPlayTime
        {
            get { return _nowPlayTime; }
            set
            {
                if (_nowPlayTime == value) return;
                _nowPlayTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        private TimeSpan ConvertTimeStringToTimeSpan(string timeString)
        {
            timeString = timeString.Replace("-", "").Trim(); // マイナスがついていたら除外する
            if (string.IsNullOrEmpty(timeString)) return TimeSpan.Zero;
            if (timeString.Length < 6)
            {
                timeString = "0:" + timeString;
            }
            return TimeSpan.Parse(timeString);
        }

        #region Season変更通知プロパティ

        private string _season;

        public string Season
        {
            get { return _season; }
            set
            {
                if (_season == value) return;
                _season = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region CanPrevious変更通知プロパティ

        private bool _CanPrevious;

        public bool CanPrevious
        {
            get { return _CanPrevious; }
            set
            {
                if (_CanPrevious == value) return;
                _CanPrevious = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region CanNext変更通知プロパティ

        private bool _CanNext;

        public bool CanNext
        {
            get { return _CanNext; }
            set
            {
                if (_CanNext == value) return;
                _CanNext = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region PreviousId変更通知プロパティ

        private long _PreviousId;

        public long PreviousId
        {
            get { return _PreviousId; }
            set
            {
                if (_PreviousId == value) return;
                _PreviousId = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region NextId変更通知プロパティ

        private long _NextId;

        public long NextId
        {
            get { return _NextId; }
            set
            {
                if (_NextId == value) return;
                _NextId = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// 更新します。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="playtime">HH:mm/HH:mm形式</param>
        public void Update(string title, string playtime)
        {
            Title = title;
            ViewPlayTime = playtime;
            if (playtime == null) return;   // プレイヤーが起動していない場合は抜ける

            var works = playtime.Split('/');
            if (works.Length == 2)
            {
                // 再生時間を設定
                NowPlayTime = ConvertTimeStringToTimeSpan(works[0].Trim());
                TotalPlayTime = ConvertTimeStringToTimeSpan(works[1].Trim());
            }
            else
            {
                NowPlayTime = TimeSpan.Zero;
                TotalPlayTime = TimeSpan.Zero;
            }
        }

        public void SetId(long id)
        {
            Id = id;
        }

        public void SetLibrary(LibraryItem library)
        {
            Library = library;
            Season = library.Season;
            if(!string.IsNullOrEmpty(library.Title)) ViewTitle = library.Title;
        }

        public void SetPreviousAndNextId(Tuple<long?, long?> prevnextId)
        {
            if (prevnextId.Item1 != null)
            {
                PreviousId = (long)prevnextId.Item1;
                CanPrevious = true;
            }
            else
            {
                CanPrevious = false;
            }

            if (prevnextId.Item2 != null)
            {
                NextId = (long)prevnextId.Item2;
                CanNext = true;
            }
            else
            {
                CanNext = false;
            }
        }
    }
}
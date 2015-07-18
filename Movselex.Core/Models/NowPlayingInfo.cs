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
            _playTime = "";
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

        #region PlayTime変更通知プロパティ

        private string _playTime = ApplicationDefinitions.TimeEmptyString;

        public string PlayTime
        {
            get { return _playTime; }
            set
            {
                if (_playTime == value) return;
                _playTime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public double PlayTimeSeconds
        {
            get
            {
                if (string.IsNullOrEmpty(PlayTime)) return 0;
                var time = PlayTime;
                if (time.Length < 6)
                {
                    time = "0:" + time;
                }
                return TimeSpan.Parse(time).TotalSeconds;
            }
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

        #region GroupName変更通知プロパティ

        private string _GroupName;

        public string GroupName
        {
            get { return _GroupName; }
            set
            {
                if (_GroupName == value) return;
                _GroupName = value;
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
        /// <param name="playtime"></param>
        public void Update(string title, string playtime)
        {
            Title = title;
            ViewPlayTime = playtime;
            if (playtime == null) return;   // プレイヤーが起動していない場合は抜ける

            var works = playtime.Split('/');
            if (works.Length == 2)
            {
                // 再生時間を設定
                PlayTime = works[1].Trim();
            }
            else
            {
                PlayTime = ApplicationDefinitions.TimeEmptyString;
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
            GroupName = !string.IsNullOrEmpty(library.GroupName) ? library.GroupName : "";
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
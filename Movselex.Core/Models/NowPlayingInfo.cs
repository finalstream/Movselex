﻿using System;
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

        public LibraryItem Library { get; private set; }

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
            if(!string.IsNullOrEmpty(library.Title)) ViewTitle = library.Title;
        }
    }
}
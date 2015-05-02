using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FinalstreamCommons.Utils;
using Livet;

namespace Movselex.Core.Models
{
    /// <summary>
    /// ライブラリ情報を表します。
    /// </summary>
    public class LibraryItem : NotificationObject
    {

        public static LibraryItem Empty = new LibraryItem(){Id = -1};

        /// <summary>
        /// Idを取得します。
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// グループIDを取得します。
        /// </summary>
        public long Gid { get; private set; }

        /// <summary>
        /// ファイルパスを取得します。
        /// </summary>
        #region FilePath変更通知プロパティ
        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath == value) return;
                _filePath = value;
                Drive = FileUtils.GetDriveLetter(value);
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// グループ名を取得します。
        /// </summary>

        #region GroupName変更通知プロパティ
        private string _groupName;

        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (_groupName == value) return;
                _groupName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// タイトルを取得します。
        /// </summary>

        #region Title変更通知プロパティ
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Noを取得します。
        /// </summary>
        public string No { get; private set; }

        /// <summary>
        /// 再生時間を取得します。
        /// </summary>

        #region Length変更通知プロパティ
        private string _length;

        public string Length
        {
            get { return _length; }
            set
            {
                if (_length == value) return;
                _length = value;
                RaisePropertyChanged();
            }
        }

        #endregion


        public TimeSpan Duration
        {
            get
            {
                var time = Length;
                if (string.IsNullOrEmpty(time)) time = ApplicationDefinitions.TimeEmptyString;
                if (time.Length < 6)
                {
                    time = "0:" + time;
                }
                return TimeSpan.Parse(time);
            }
        }


        /// <summary>
        /// コーデックを取得します。
        /// </summary>

        #region Codec変更通知プロパティ
        private string _codec;

        public string Codec
        {
            get { return _codec; }
            set
            {
                if (_codec == value) return;
                _codec = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// 再生済みかどうかを取得します。
        /// </summary>

        #region IsPlayed変更通知プロパティ
        private bool _isPlayed;

        public bool IsPlayed
        {
            get { return _isPlayed; }
            set
            {
                if (_isPlayed == value) return;
                _isPlayed = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// レーティングを取得します。
        /// </summary>

        #region Rating変更通知プロパティ
        private RatingType _rating;

        public RatingType Rating
        {
            get { return _rating; }
            set
            {
                if (_rating == value) return;
                _rating = value;
                IsFavorite = value == RatingType.Favorite;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// レーティングを取得します。
        /// </summary>
        #region IsFavorite変更通知プロパティ
        private bool _isFavorite;

        public bool IsFavorite
        {
            get { return _isFavorite; }
            set
            {
                if (_isFavorite == value) return;
                _isFavorite = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// ファイルの最終更新日時を取得します。
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// ファイルの最終更新日を取得します。
        /// </summary>
        public string FileLastUpdateDate { get { return Date.ToString("yyyy-MM-dd"); }}

        /// <summary>
        /// 動画のサイズを取得します。
        /// </summary>
        #region VideoSize変更通知プロパティ
        private string _videoSize;

        public string VideoSize
        {
            get { return _videoSize; }
            set
            {
                if (_videoSize == value) return;
                _videoSize = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// ファイルが格納されているドライブレターを取得します。
        /// </summary>
        #region Drive変更通知プロパティ
        private string _drive;

        public string Drive
        {
            get { return _drive; }
            set
            {
                if (_drive == value) return;
                _drive = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// 再生回数を取得します。
        /// </summary>
        #region PlayCount変更通知プロパティ

        private int _playCount;

        public int PlayCount
        {
            get { return _playCount; }
            set
            {
                if (_playCount == value) return;
                _playCount = value;
                RaisePropertyChanged();
            }
        }

        #endregion

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

        #region IsSelected変更通知プロパティ

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// 追加された日時を取得します。
        /// </summary>
        public DateTime AddDateTime { get; private set; }
        
        /// <summary>
        /// 最後に再生した日時を取得します。
        /// </summary>
        public DateTime LastPlayedDateTime { get; private set; }

        public void IncrementPlayCount()
        {
            PlayCount += 1;
        }

        public void UpdateMediaInfo(IMediaFile mediaFile)
        {
            Length = mediaFile.Length;
            Codec = mediaFile.Codec;
            VideoSize = mediaFile.VideoSize;
        }

        public void ModifyIsPlayed(bool isPlayed)
        {
            IsPlayed = isPlayed;
        }

        public void ModifyRating(RatingType newRating)
        {
            Rating = newRating;
        }

        public void ModifyFilePath(string filepath)
        {
            FilePath = filepath;
        }

        public void UnGroup()
        {
            Gid = 0;
            GroupName = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Livet;

namespace Movselex.Core.Models
{
    /// <summary>
    /// ライブラリ情報を表します。
    /// </summary>
    public class LibraryItem : NotificationObject
    {

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
        public string FilePath { get; private set; }

        /// <summary>
        /// グループ名を取得します。
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// タイトルを取得します。
        /// </summary>
        public string Title { get; private set; }

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
        public string Drive { get { return FilePath.Substring(0, 1).ToUpper(); } }

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
    }
}

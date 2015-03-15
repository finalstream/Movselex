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
        public long GroupId { get; private set; }

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
        /// 再生時間(sec)を取得します。
        /// </summary>
        public string Length { get; private set; }

        /// <summary>
        /// コーデックを取得します。
        /// </summary>
        public string Codec { get; private set; }

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
        public int Rating { get; private set; }

        /// <summary>
        /// レーティングを取得します。
        /// </summary>
        public bool IsFavorite { get { return Rating == (int)RatingType.Favorite; } }

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
        public string VideoSize { get; private set; }

        /// <summary>
        /// ファイルが格納されているドライブレターを取得します。
        /// </summary>
        public string Drive { get { return FilePath.Substring(0, 1).ToUpper(); } }

        /// <summary>
        /// 再生回数を取得します。
        /// </summary>
        public int PlayCount { get; private set; }

        /// <summary>
        /// 追加された日時を取得します。
        /// </summary>
        public DateTime AddDateTime { get; private set; }
        
        /// <summary>
        /// 最後に再生した日時を取得します。
        /// </summary>
        public DateTime LastPlayedDateTime { get; private set; }


        public void SwitchIsPlayed()
        {
            IsPlayed = !IsPlayed;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Collections;
using FinalstreamCommons.Utils;
using Livet;

namespace Movselex.Core.Models
{
    public class GroupItem : NotificationObject, ISelectableItem
    {
        /// <summary>
        /// グループID。
        /// </summary>
        /// <remarks>未登録は0</remarks>
        public long Gid { get; private set; }

        /// <summary>
        /// グループ名。
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
        /// ファイルが格納されているドライブ。
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
        /// ファイルサイズ(Byte)。
        /// </summary>
        public long Filesize { get; private set; }

        /// <summary>
        /// グループに格納されているファイル数。
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// グループに格納されているお気に入りのファイル数。
        /// </summary>
        public int FavoriteCount { get; private set; }

        /// <summary>
        /// グループのキーワード。
        /// </summary>

        #region Keyword変更通知プロパティ
        private string _keyword;

        public string Keyword
        {
            get { return _keyword; }
            set
            {
                if (_keyword == value) return;
                _keyword = value;
                RaisePropertyChanged();
            }
        }

        #endregion


        /// <summary>
        /// お気に入りかどうか。
        /// </summary>
        #region IsFavorite変更通知プロパティ
        private bool _isFavorite;

        public bool IsFavorite
        {
            get { return _isFavorite || (FavoriteCount > 0 && FavoriteCount == Count); }
            set
            {
                if (_isFavorite == value) return;
                _isFavorite = value;
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
        /// コンプリートしているかどうか。
        /// </summary>

        #region IsCompleted変更通知プロパティ
        private bool _isCompleted;

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (_isCompleted == value) return;
                _isCompleted = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public string FileSizeString { get { return FileUtils.ConvertFileSizeGigaByteString(Filesize); }}

        /// <summary>
        /// お気に入りを変更します。
        /// </summary>
        public void ModifyIsFavorite(bool isFavorite)
        {
            IsFavorite = isFavorite;
        }

        public void ModifyIsComplete(bool isComplete)
        {
            IsCompleted = isComplete;
        }

        public void ModifyDriveLetter(string drive)
        {
            Drive = drive;
        }

        public void ModifyNameAndKeyword(string groupName, string keyword)
        {
            GroupName = groupName;
            Keyword = keyword;
        }
    }
}

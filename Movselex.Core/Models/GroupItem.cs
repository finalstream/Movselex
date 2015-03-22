using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Utils;
using Livet;

namespace Movselex.Core.Models
{
    public class GroupItem : NotificationObject
    {
        /// <summary>
        /// グループID。
        /// </summary>
        public long Gid { get; private set; }

        /// <summary>
        /// グループ名。
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// ファイルが格納されているドライブ。
        /// </summary>
        public string Drive { get; private set; }

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
        public string Keyword { get; private set; }


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
        public bool IsCompleted { get; private set; }

        public string FileSizeString { get { return FileUtils.ConvertFileSizeGigaByteString(Filesize); }}

        /// <summary>
        /// お気に入りを変更します。
        /// </summary>
        public void ModifyIsFavorite(bool isFavorite)
        {
            IsFavorite = isFavorite;
        }
    }
}

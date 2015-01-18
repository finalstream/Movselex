using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Utils;

namespace Movselex.Core.Models
{
    public class GroupItem
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
        public bool IsFavorite { get; private set; }

        /// <summary>
        /// コンプリートしているかどうか。
        /// </summary>
        public bool IsCompleted { get; private set; }

        public string FileSizeString { get { return FileUtils.ConvertFileSizeGigaByteString(Filesize); }}

    }
}

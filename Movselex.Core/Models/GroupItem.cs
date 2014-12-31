using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    public class GroupItem
    {
        
        /// <summary>
        /// グループ名。
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// ファイルが格納されているドライブ。
        /// </summary>
        public string Drive { get; private set; }

        /// <summary>
        /// ファイルサイズ(Byte)。
        /// </summary>
        public double Filesize { get; private set; }

        /// <summary>
        /// グループに格納されているファイル数。
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// お気に入りかどうか。
        /// </summary>
        public bool IsFavorite { get; private set; }

        /// <summary>
        /// コンプリートしているかどうか。
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="drive"></param>
        /// <param name="filesize"></param>
        /// <param name="count"></param>
        /// <param name="isFavorite"></param>
        /// <param name="isCompleted"></param>
        public GroupItem(string title, string drive, double filesize, int count, bool isFavorite, bool isCompleted)
        {
            Title = title;
            Drive = drive;
            Filesize = filesize;
            Count = count;
            IsFavorite = isFavorite;
            IsCompleted = isCompleted;
        }
    }
}

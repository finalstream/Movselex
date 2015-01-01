using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Movselex.Core.Models
{
    /// <summary>
    /// ライブラリ情報を表します。
    /// </summary>
    public class LibraryItem
    {

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
        public int Length { get; private set; }

        /// <summary>
        /// お気に入りかどうかを取得します。
        /// </summary>
        public bool IsFavorite { get; private set; }

        /// <summary>
        /// 再生済みかどうかを取得します。
        /// </summary>
        public bool IsPlayed { get; private set; }

        /// <summary>
        /// ファイルの最終更新日時を取得します。
        /// </summary>
        public DateTime FileLastUpdateDateTime { get; private set; }

        /// <summary>
        /// 動画のサイズを取得します。
        /// </summary>
        public Size VideoSize { get; private set; }

        /// <summary>
        /// ファイルが格納されているドライブレターを取得します。
        /// </summary>
        public string Drive { get; private set; }

        /// <summary>
        /// 再生回数を取得します。
        /// </summary>
        public int PlayCount { get; private set; }


        /// <summary>
        /// 新しいインスタンスを取得します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="groupName"></param>
        /// <param name="title"></param>
        /// <param name="no"></param>
        /// <param name="length"></param>
        /// <param name="isFavorite"></param>
        /// <param name="isPlayed"></param>
        /// <param name="fileLastUpdateDateTime"></param>
        /// <param name="videoSize"></param>
        /// <param name="drive"></param>
        /// <param name="playCount"></param>
        public LibraryItem(string filePath, string groupName, string title, string no, int length, bool isFavorite, bool isPlayed, DateTime fileLastUpdateDateTime, Size videoSize, string drive, int playCount)
        {
            FilePath = filePath;
            GroupName = groupName;
            Title = title;
            No = no;
            Length = length;
            IsFavorite = isFavorite;
            IsPlayed = isPlayed;
            FileLastUpdateDateTime = fileLastUpdateDateTime;
            VideoSize = videoSize;
            Drive = drive;
            PlayCount = playCount;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalstreamCommons.Utils
{
    /// <summary>
    /// ファイルを扱うユーティリティです。
    /// </summary>
    public static class FileUtils
    {

        /// <summary>
        /// ファイルサイズをGBの文字列に変換します。
        /// </summary>
        /// <param name="fileSize">変換元のファイルサイズ。</param>
        /// <returns>変換後のファイルサイズ。</returns>
        public static string ConvertFileSizeGigaByteString(long fileSize)
        {
            if (fileSize == 0)
            {
                return "";
            }

            return String.Format("{0:##0.#0}", fileSize / 1073741824.0) + " GB";
        }

        /// <summary>
        /// ファイルサイズを取得します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long GetFileSize(string filePath)
        {
            return new FileInfo(filePath).Length;
        }
    }
}

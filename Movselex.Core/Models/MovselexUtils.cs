using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    static class MovselexUtils
    {
        /// <summary>
        /// タイトル用に文字列を置換します。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceTitle(string s)
        {
            var ss = Path.GetFileNameWithoutExtension(s);
            if (ss == null) return s;

            var sss = ss.Replace("　", " ").
                Replace("（", "(")
                .Replace("）", ")")
                .Replace("［", "[")
                .Replace("］", "]")
                .Replace("#", "")
                .Replace("RAW", "");

            return Regex.Replace(sss, @"[(\[].+?[)\]]", "").Trim();
        }
    }
}

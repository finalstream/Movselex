using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FinalstreamCommons.Utils;

namespace Movselex.Core.Models
{
    public static class MovselexUtils
    {
        /// <summary>
        /// タイトル用に文字列を置換します。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceTitle(string ss)
        {
            if (ss == null) return ss;

            var sss = ss.Replace("　", " ").
                Replace("（", "(")
                .Replace("）", ")")
                .Replace("［", "[")
                .Replace("］", "]")
                .Replace("#", "")
                .Replace("RAW", "");

            return Regex.Replace(sss, @"[(\[].+?[)\]]", "").Trim();
        }

        /// <summary>
        /// タイトルからキーワード候補を生成します。
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string[] CreateKeywords(string title)
        {
            string[] titlewords = title.Replace(" - ", " ").Split(' ');

            var wordList = new List<string>();

            string workword = "";
            foreach (string word in titlewords)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    workword += word + " ";
                    if (!word.Equals(workword))
                    {
                        wordList.Add(workword.TrimEnd());
                    }
                    else
                    {
                        wordList.Add(word);
                    }
                }

            }

            return wordList.ToArray();
        }


        public static string GetMaxCountMaxLengthKeyword(IEnumerable<string> keywordList)
        {
            var keywords = keywordList.GroupBy(x => x).Select(x => new {Key = x.Key, Count = x.Count()}).ToArray();
            var maxkeywords = keywords.Where(x => x.Count == keywords.Select(xx => xx.Count).Max());
            return maxkeywords.Max(x => x.Key);
        }
    }
}

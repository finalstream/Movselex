using System;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using FinalstreamCommons.Utils;

namespace Movselex.Core.Models
{
    internal class MediaFile : IMediaFile
    {
        public long Id { get; private set; }

        public string FilePath { get; private set; }

        public string MovieTitle { get; private set; }

        /// <summary>
        /// グループID
        /// </summary>
        /// <remarks>nullは未登録を表します。</remarks>
        public long? GroupId { get; private set; }

        public string GroupName { get; private set; }

        public string GroupKeyword { get; private set; }

        public RatingType Rating { get; private set; }

        public string No
        {
            get { return GetNo();}
        }

        public long FileSize { get { return FileUtils.GetFileSize(FilePath); }}

        public TimeSpan Duration { get; private set; }

        public string Length
        {
            get
            {
                if (Duration == default(TimeSpan)) return ApplicationDefinitions.TimeEmptyString;
                if (Duration.Hours > 0)
                {
                    return new DateTime(0).Add(Duration).ToString(ApplicationDefinitions.TimeFormatHourMinuteSecond);
                }
                else
                {
                    return new DateTime(0).Add(Duration).ToString(ApplicationDefinitions.TimeFormatMinuteSecond);
                }
            }
        }

        public Size Size { get; private set; }

        public string VideoSize
        {
            get
            {
                return Size != default(Size)
                    ? string.Format(ApplicationDefinitions.VideoSizeFormat, Size.Width, Size.Height)
                    : "";
            }
        }

        public string Codec { get; private set; }

        public DateTime UpdateDateTime
        {
            get { return File.Exists(FilePath)? File.GetLastWriteTime(FilePath) : DateTime.Now; }
        }

        public MediaFile(string filepath)
        {
            GroupId = null;
            FilePath = filepath;
            Rating = RatingType.Nothing;

            MovieTitle = MovselexUtils.ReplaceTitle(Path.GetFileNameWithoutExtension(FilePath));

            try
            {
                TagLib.File file = TagLib.File.Create(FilePath);

                Duration = file.Properties.Duration;
                Size = new Size(file.Properties.VideoWidth, file.Properties.VideoHeight);
                Codec = file.Properties.Description;
            }
            catch
            {
                Codec = "";
            }
        }


        private string GetNo()
        {
            // TODO: ロジックを再考する。
            var title = Path.GetFileNameWithoutExtension(FilePath);
            string work = "";
            string result = "";

            // x264はまぎらわしいのではずす
            title = title.Replace("x264", "");

            // 括弧を半角に変換
            title = title.Replace("（", "(").Replace("）", ")");
            title = title.Replace("［", "[").Replace("］", "]");

            // 正規表現で括弧内の文字列をのぞく
            title = Regex.Replace(title, @"\(.*?\)", "");
            title = Regex.Replace(title, @"\[.*?\]", "");

            work = Regex.Replace(title, @"[^\d]", ",");

            string[] ary = work.Split(',');

            Array.Reverse(ary);

            foreach (var s in ary)
            {
                if (StringUtils.IsNumeric(s))
                {
                    result = s;
                    break;
                }
            }

            if (!String.IsNullOrEmpty(result))
            {
                return int.Parse(result).ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// グループ情報を更新します。
        /// </summary>
        /// <param name="gid"></param>
        /// <param name="groupname"></param>
        /// <param name="keyword"></param>
        /// <param name="groupRating"></param>
        public void UpdateGroup(long gid, string groupname, string keyword, RatingType groupRating)
        {
            GroupId = gid;
            GroupName = groupname;
            GroupKeyword = keyword;
            Rating = groupRating;

            // グループ名を使用したタイトルに変更する。
            MovieTitle = MovieTitle.Replace(keyword, groupname);
        }


        public void UpdateId(long id)
        {
            Id = id;
        }
    }
}
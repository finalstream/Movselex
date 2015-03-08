using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using FinalstreamCommons.Extentions;
using FinalstreamCommons.Utils;
using Livet;
using NLog;

namespace Movselex.Core.Models
{
    internal class MovselexGroup : IMovselexGroup
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public ObservableCollection<GroupItem> GroupItems { get; private set; }

        private readonly IMovselexDatabaseAccessor _databaseAccessor;

        public MovselexGroup(IMovselexDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
            GroupItems = new ObservableCollection<GroupItem>();
        }


        /// <summary>
        /// ライブラリデータをロードします。
        /// </summary>
        public void Load()
        {
            var groups = _databaseAccessor.SelectGroup();

            GroupItems.Clear();
            foreach (var groupItem in groups)
            {
                GroupItems.Add(groupItem);
            }
        }

        public void SetMovGroup(MediaFile mediaFile)
        {
            var keywords = CreateKeyworkds(mediaFile.MovieTitle);
            
            _log.Debug("Title:{0} Keywords:{1}", mediaFile.MovieTitle, keywords.ToJson());

            foreach (string keyword in keywords)
            {
                // 小文字で検索
                var group = _databaseAccessor.SelectMatchGroupKeyword(keyword.ToLower());


                if (group != null)
                {
                    var gid = group.GID;

                    // グループのレーティングを取得
                    var groupRating = GetGroupRating(gid);

                    mediaFile.UpdateGroup(gid, group.GROUPNAME, keyword, groupRating);

                    // グループ最終更新日時更新
                    _databaseAccessor.UpdateGroupLastUpdateDatetime(gid);

                    break;
                }
            }

            

        }

        private RatingType GetGroupRating(long gid)
        {
            return _databaseAccessor.GetGroupRating(gid);
        }

        /// <summary>
        /// タイトルからキーワード候補を生成します。
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private string[] CreateKeyworkds(string title)
        {
            string[] titlewords = title.Split(' ');

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
    }
}
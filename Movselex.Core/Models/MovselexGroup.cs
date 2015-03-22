using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
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
        /// ���C�u�����f�[�^�����[�h���܂��B
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

        public void Load(IEnumerable<long> gids)
        {
            var groups = _databaseAccessor.SelectGroup().ToArray();

            GroupItems.Clear();
            foreach (var gid in gids)
            {
                var groupItem = groups.FirstOrDefault(x => x.Gid == gid);
                if (groupItem != null) GroupItems.Add(groupItem);
            }
        }



        /// <summary>
        /// ���f�B�A�t�@�C���ɃO���[�v��ݒ肵�܂��B
        /// </summary>
        /// <param name="mediaFile"></param>
        public void SetMovGroup(MediaFile mediaFile)
        {
            var keywords = CreateKeyworkds(mediaFile.MovieTitle);
            
            _log.Debug("Title:{0} Keywords:{1}", mediaFile.MovieTitle, keywords.ToJson());

            foreach (string keyword in keywords)
            {
                // �������Ō���
                var group = _databaseAccessor.SelectMatchGroupKeyword(keyword.ToLower());


                if (group != null)
                {
                    var gid = group.GID;

                    // �O���[�v�̃��[�e�B���O���擾
                    var groupRating = GetGroupRating(gid);

                    mediaFile.UpdateGroup(gid, group.GROUPNAME, keyword, groupRating);

                    // �O���[�v�ŏI�X�V�����X�V
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
        /// �^�C�g������L�[���[�h���𐶐����܂��B
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


        public void ModifyRating(GroupItem group, RatingType rating)
        {

            using (var tran = _databaseAccessor.BeginTransaction())
            {
                var ids = _databaseAccessor.SelectIdFromGid(group.Gid);
                foreach (var id in ids)
                {
                    _databaseAccessor.UpdateLibraryRating(id, rating);
                }
                tran.Commit();
            }

            group.ModifyIsFavorite(rating == RatingType.Favorite);
        }

        public void ModifyIsComplete(GroupItem group)
        {
            var newComplete = !group.IsCompleted; // �t�ɂ���
            _databaseAccessor.UpdateGroupIsCompleted(group.Gid, newComplete);
            group.ModifyIsComplete(newComplete);
        }
    }
}
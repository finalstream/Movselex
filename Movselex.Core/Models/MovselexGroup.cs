using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text.RegularExpressions;
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
            var keywords = MovselexUtils.CreateKeywords(mediaFile.MovieTitle).Reverse().ToArray();
            
            _log.Debug("Title:{0} Keywords:{1}", mediaFile.MovieTitle, keywords.ToJson());

            foreach (var keyword in keywords)
            {
                // �������Ō���
                var group = _databaseAccessor.SelectMatchGroupKeyword(keyword.ToLower());


                if (group != null)
                {
                    // �����L�[���[�h�̃O���[�v����������������X�V���Ĕ�����
                    var gid = group.GID;

                    // �O���[�v�̃��[�e�B���O���擾
                    var groupRating = GetGroupRating(gid);

                    mediaFile.UpdateGroup(gid, group.GROUPNAME, keyword, groupRating);

                    // �O���[�v�ŏI�X�V�����X�V
                    _databaseAccessor.UpdateGroupLastUpdateDatetime(gid);

                    return;
                }
            }

            // �O���[�v��������Ȃ������玩���O���[�v�o�^
            var ungroupLibraries = _databaseAccessor.SelectUnGroupingLibrary(keywords);

            var unGroups = ungroupLibraries
                .GroupBy(x => Regex.Replace(MovselexUtils.ReplaceTitle(x.Title).Replace(" - ", " "), @"\d+", "").Trim())
                .Select(x=> new {Key = x.Key, Count = x.Count(), Libraries= x.ToList()}).ToArray();
            if (unGroups.Any()) unGroups.DebugWriteJson("UnGroup Keywords");

            var maxUnGroup = unGroups.FirstOrDefault(x => x.Count == unGroups.Select(xx => xx.Count).Max());

            maxUnGroup.DebugWriteJson("MaxUnGroup");

            if (maxUnGroup != null)
            {
                var groupNameandKeyword = maxUnGroup.Key.TrimEnd();

                //if (groupNameandKeyword.EndsWith(" -")) groupNameandKeyword = StringUtils.ReplaceLastOnce(groupNameandKeyword, " -", "");

                // �O���[�v��o�^����
                var gid = RegistGroup(groupNameandKeyword, groupNameandKeyword);
                JoinGroup(groupNameandKeyword, groupNameandKeyword, maxUnGroup.Libraries);

                // �O���[�v�̃��[�e�B���O���擾
                var groupRating = GetGroupRating(gid);

                mediaFile.UpdateGroup(gid, groupNameandKeyword, groupNameandKeyword, groupRating);

                // �O���[�v�ŏI�X�V�����X�V
                _databaseAccessor.UpdateGroupLastUpdateDatetime(gid);
            }

        }

        private RatingType GetGroupRating(long gid)
        {
            return _databaseAccessor.GetGroupRating(gid);
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

        /// <summary>
        /// �O���[�v�Ƀ��C�u������o�^���܂��B
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="keyword"></param>
        /// <param name="libraries"></param>
        public void JoinGroup(string groupName, string keyword, IEnumerable<LibraryItem> libraries)
        {
            using (var tran = _databaseAccessor.BeginTransaction())
            {
                var gid = _databaseAccessor.SelectGIdByGroupName(groupName);

                if (gid == -1)
                {
                    // �O���[�v�����݂��Ȃ��ꍇ�o�^
                    gid = RegistGroup(groupName, keyword);
                }

                foreach (var library in libraries)
                {
                    _databaseAccessor.UpdateGidById(gid, library.Id);

                    _databaseAccessor.UpdateGroupLastUpdateDatetime(gid);

                    library.GroupName = groupName;
                }

                tran.Commit();
            }
        }

        /// <summary>
        /// �O���[�v��o�^���܂��B
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="keyword"></param>
        private long RegistGroup(string groupName, string keyword)
        {
            _databaseAccessor.InsertGroup(groupName, keyword);
            var gid = _databaseAccessor.SelectLastInsertRowId();
            _log.Info("Registed Group. Gid:{0} GroupName:{1} Keyword:{2}", gid, groupName, keyword);
            return gid;
        }

        public void ModifyGroup(GroupItem group, string groupName, string keyword)
        {
            var oldGroupName = group.GroupName;
            group.ModifyNameAndKeyword(groupName, keyword);
            _databaseAccessor.UpdateGroup(group);
            _databaseAccessor.UpdateLibraryReplaceGroupName(group.Gid, oldGroupName, group.GroupName);

        }
    }
}
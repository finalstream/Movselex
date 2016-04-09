using System.Collections.Generic;
using System.Linq;
using FinalstreamCommons.Collections;
using FinalstreamCommons.Extensions;
using NLog;

namespace Movselex.Core.Models
{
    internal class MovselexGroup : IMovselexGroup
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public SelectableObservableCollection<GroupItem> GroupItems { get; private set; }

        private readonly IMovselexDatabaseAccessor _databaseAccessor;

        public MovselexGroup(IMovselexDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
            GroupItems = new SelectableObservableCollection<GroupItem>();
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
            var groups = _databaseAccessor.SelectGroup();

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
                var group = _databaseAccessor.SelectMatchGroupKeyword(keyword).FirstOrDefault();


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
            var unGroupingItem = _databaseAccessor.SelectUnGroupingLibrary(keywords);

            if (unGroupingItem == null) return; // �L�[���[�h�Ɉ�v���郉�C�u������������Ȃ������甲����B

            unGroupingItem.DebugWriteJson("UnGroupItem");

            //var unGroups = ungroupLibraries
            //    .GroupBy(x =>
            //    {
            //        var key = MovselexUtils.ReplaceTitle(x.Title).Replace(" - ", " ").TrimEnd();
            //        var works = key.Split(' ');
            //        if (StringUtils.IsNumeric(works[works.Length - 1])) works[works.Length - 1] = "";

            //        return string.Join(" ", works).Trim();
            //    })
            //    .Select(x=> new {Key = x.Key, Count = x.Count(), Libraries= x.ToList()}).ToArray();
            //if (unGroups.Any()) unGroups.DebugWriteJson("UnGroup Keywords");

            //var maxUnGroup = unGroups.FirstOrDefault(x => x.Count == unGroups.Select(xx => xx.Count).Max());

            //maxUnGroup.DebugWriteJson("MaxUnGroup");

            if (unGroupingItem != null)
            {
                var keyword = unGroupingItem.Item1;
                var unGroupLibraries = unGroupingItem.Item2;
                var groupNameandKeyword = keyword.TrimEnd();

                //if (groupNameandKeyword.EndsWith(" -")) groupNameandKeyword = StringUtils.ReplaceLastOnce(groupNameandKeyword, " -", "");

                // �O���[�v��o�^����
                var gid = RegistGroup(groupNameandKeyword, groupNameandKeyword);
                JoinGroup(groupNameandKeyword, groupNameandKeyword, unGroupLibraries);

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
            ValidateRegistGroup(keyword);
            _databaseAccessor.InsertGroup(groupName, keyword);
            var gid = _databaseAccessor.SelectLastInsertRowId();
            _log.Info("Registed Group. Gid:{0} GroupName:{1} Keyword:{2}", gid, groupName, keyword);
            return gid;
        }

        /// <summary>
        /// �O���[�v����ύX���܂��B
        /// </summary>
        /// <param name="group"></param>
        /// <param name="newGroupName"></param>
        /// <param name="keyword"></param>
        public void ModifyGroup(GroupItem group, string newGroupName, string keyword)
        {
            ValidateRegistGroup(keyword, group);
            var oldGroupName = group.GroupName;
            group.ModifyNameAndKeyword(newGroupName, keyword);
            _databaseAccessor.UpdateGroup(group);
            _databaseAccessor.UpdateLibraryReplaceGroupName(group.Gid, oldGroupName, group.GroupName);

        }

        public IEnumerable<dynamic> GetMatchKeywordGroups(string keyword)
        {
            return _databaseAccessor.SelectMatchGroupKeyword(keyword);
        }

        public void ValidateRegistGroup(string keyword, GroupItem ownGroup = null)
        {
            var sameKeywordGroups = GetMatchKeywordGroups(keyword);
            if (ownGroup != null) sameKeywordGroups = sameKeywordGroups.Where(x => x.GID != ownGroup.Gid);  // �O���[�v�X�V�̂Ƃ��͎����������B
            if (sameKeywordGroups.Any())
            {
                // �����L�[���[�h�̃O���[�v�����łɑ��݂�����G���[
                throw new MovselexException(string.Format(Properties.Resources.ErrorMessageRegistedDuplicateKeyword, keyword.ToLower()));
            }
        }

        public void DiffLoad()
        {
            var groups = _databaseAccessor.SelectGroup().ToArray();

            GroupItems.DiffInsert(groups, new GroupItemComparer());
        }

        public IEnumerable<long> GetLibraryIds(long gid)
        {
            return _databaseAccessor.SelectIdFromGid(gid);
        }

        public void DeleteGroup(GroupItem group)
        {
            using (var tran = _databaseAccessor.BeginTransaction())
            {
                _databaseAccessor.UpdateUnGroup(group.Gid);
                _databaseAccessor.DeleteGroup(group.Gid);

                tran.Commit();
                GroupItems.Remove(group);
                _log.Info("Delete Group. Gid:{0} GroupName:{1} ", group.Gid, group.GroupName);
            }
        }
    }
}
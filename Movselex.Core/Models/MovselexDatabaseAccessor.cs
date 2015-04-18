using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
using FinalstreamCommons.Models;
using Movselex.Core.Resources;

namespace Movselex.Core.Models
{
    internal class MovselexDatabaseAccessor : DatabaseAccessor, IMovselexDatabaseAccessor
    {

        private SQLBuilder _sqlBuilder;

        private string _lastLibrarySelectSQL;

        private readonly MovselexAppConfig _appConfig;


        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="appConfig"></param>
        public MovselexDatabaseAccessor(MovselexAppConfig appConfig)
        {
            _appConfig = appConfig;
            ChangeDatabase(_appConfig.SelectDatabase);
            _sqlBuilder = new SQLBuilder();
        }

        /// <summary>
        /// ライブラリを取得します。
        /// </summary>
        /// <param name="libCondition"></param>
        /// <returns></returns>
        public IEnumerable<LibraryItem> SelectLibrary(LibraryCondition libCondition)
        {
            _lastLibrarySelectSQL = _sqlBuilder.CreateSelectLibrary(_appConfig.LibraryMode, libCondition);
            return SqlExecuter.Query<LibraryItem>(_lastLibrarySelectSQL);

        }

        /// <summary>
        /// データベースを変更します。
        /// </summary>
        /// <param name="databaseName"></param>
        /// <remarks>SQLExecuterでコネクションを管理しています。</remarks>
        public void ChangeDatabase(string databaseName)
        {
            DatabaseName = databaseName;
            SqlExecuter = MovselexSQLExecuterFactory.Create(databaseName);
            //_lastLibrarySelectSQL = SQLResource.SelectLibraryList;
        }

        /// <summary>
        /// グループを取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GroupItem> SelectGroup()
        {
            return SqlExecuter.Query<GroupItem>(
                _sqlBuilder.CreateSelectGroup(_appConfig.LibraryMode, _lastLibrarySelectSQL));
        }

        public IEnumerable<LibraryItem> ShuffleLibrary(int limitNum)
        {
            if (string.IsNullOrEmpty(_lastLibrarySelectSQL)) return Enumerable.Empty<LibraryItem>();
            var sql = SQLResource.SelectShuffleLibrary.Replace("#LastExecSql#", _lastLibrarySelectSQL);
            return SqlExecuter.Query<LibraryItem>(
                sql, 
                new { LimitNum = limitNum });
        }

        public string GetMostUseDirectoryPath()
        {
            return SqlExecuter.Query<string>(SQLResource.SelectMostUseDirectoryPath).FirstOrDefault();
        }

        public IEnumerable<string> SelectAllLibraryFilePaths()
        {
            return SqlExecuter.Query<string>(SQLResource.SelectAllFilePath);
        }

        public bool InsertMediaFile(MediaFile mediaFile)
        {

            var result = SqlExecuter.Execute(SQLResource.InsertLibrary,
                new
                {
                    Gid = mediaFile.GroupId,
                    FilePath = mediaFile.FilePath,
                    Option = (string) null,
                    Filesize = mediaFile.FileSize,
                    No = mediaFile.No,
                    Length = mediaFile.Length,
                    Title = mediaFile.MovieTitle,
                    Codec = mediaFile.Codec,
                    Rating = (int)mediaFile.Rating,
                    PlayCount = 0,
                    Date = mediaFile.UpdateDateTime.ToString(ApplicationDefinitions.SqliteDateTimeFormat),
                    NotFound = 0,
                    Tag = (string)null,
                    AddDate = DateTime.Now.ToString(ApplicationDefinitions.SqliteDateTimeFormat),
                    LastPlayDate = (string)null,
                    Played = 0,
                    VideoSize = mediaFile.VideoSize
                });

            mediaFile.UpdateId(SqlExecuter.Query<long>(SQLResource.SelectLastInsertRowid).SingleOrDefault());
                
            return result == 1;
        }

        /// <summary>
        /// キーワードにマッチしたグループ情報を取得します。
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>取得出来なかった場合はnullが返ります。</returns>
        public dynamic SelectMatchGroupKeyword(string keyword)
        {
            return SqlExecuter.Query<dynamic>(SQLResource.SelectGroupKeyword, new { Keyword = keyword}).FirstOrDefault();
        }

        public void UpdateGroupLastUpdateDatetime(long gid)
        {
            SqlExecuter.Execute(SQLResource.UpdateGroupLastUpdateDatetime,
                new {Gid = gid, LastUpdate = DateTime.Now.ToString(ApplicationDefinitions.SqliteDateTimeFormat)});
        }

        public RatingType GetGroupRating(long gid)
        {
            var gidcnt = SqlExecuter.Query<int>(SQLResource.SelectGroupIdCount, new { Gid = gid}).SingleOrDefault();

            if (gidcnt > 0)
            {
                var gidfavcnt = SqlExecuter.Query<int>(SQLResource.SelectFavGroupIdCount, new { Gid = gid }).Single();

                if (gidcnt == gidfavcnt) return RatingType.Favorite;
            }

            return RatingType.Nothing;
        }

        public long GetIdFromFileName(string filename)
        {
            var id = SqlExecuter.Query<long>(
                SQLResource.SelectIdFromFileName, 
                new {FileName = string.Format("%{0}%", filename) }).FirstOrDefault();
            return id == 0L ? -1 : id;
        }

        public void UpdatePlayCount(long id)
        {
            SqlExecuter.Execute(SQLResource.UpdatePlayCount, new { Id = id, LastPlayDate = DateTime.Now.ToString(ApplicationDefinitions.SqliteDateTimeFormat)});
        }

        public Dictionary<long, string> SelectInCompleteIds()
        {
            return SqlExecuter.Query<dynamic>(
                SQLResource.SelectInCompleteIdFilePaths, 
                new { Length = ApplicationDefinitions.TimeEmptyString}).ToDictionary(row => (long)row.ID, row=> (string)row.FILEPATH);
        }

        public bool UpdateMediaFile(MediaFile mediaFile)
        {
            var result = SqlExecuter.Execute(SQLResource.UpdateMediaInfo,
                new
                {
                    Id = mediaFile.Id,
                    Length = mediaFile.Length,
                    Codec = mediaFile.Codec,
                    VideoSize = mediaFile.VideoSize
                });

            return result == 1;
        }

        public void UpdateLibraryIsPlayed(long id, bool isPlayed)
        {
            SqlExecuter.Execute(SQLResource.UpdateLibraryIsPlayed, new {Id = id, Played = isPlayed ? 1 : 0});
        }

        public void UpdateLibraryRating(long id, RatingType rating)
        {
            SqlExecuter.Execute(SQLResource.UpdateLibraryRating, new { Id = id, Rating = (int)rating });
        }

        public IEnumerable<long> SelectIdFromGid(long gid)
        {
            return SqlExecuter.Query<long>(SQLResource.SelectIdFromGid, new {Gid = gid});
        }

        public void UpdateGroupIsCompleted(long gid, bool newComplete)
        {
            SqlExecuter.Execute(SQLResource.UpdateGroupIsCompleted, new {Gid = gid, Complete = newComplete});
        }

        public void UpdateLibraryFilePath(KeyValuePair<long, string> kv)
        {
            SqlExecuter.Execute(SQLResource.UpdateLibraryFilePath, new {Id = kv.Key, FilePath = kv.Value});
        }

        public long SelectGIdByGroupName(string groupName)
        {
            var gid = SqlExecuter.Query<long>(SQLResource.SelectGIdByGroupName, new {GroupName = groupName.ToLower()}).FirstOrDefault();
            return gid == 0L ? -1 : gid;
        }

        public void InsertGroup(string groupName, string keyword)
        {
            SqlExecuter.Execute(SQLResource.InsertGroup,
                new
                {
                    GroupName = groupName,
                    Keyword = keyword.ToLower(),
                    LastUpdate = DateTime.Now.ToString(ApplicationDefinitions.SqliteDateTimeFormat)
                });
        }

        public long SelectLastInsertRowId()
        {
            return SqlExecuter.Query<long>(SQLResource.SelectLastInsertRowid).FirstOrDefault();
        }

        public void UpdateGidById(long gid, long id)
        {
            SqlExecuter.Execute(SQLResource.UpdateGidById, new {Gid = gid, Id = id});
        }

        public void UpdateGroup(GroupItem @group)
        {
            SqlExecuter.Execute(SQLResource.UpdateGroup,
                new {Gid = group.Gid, GroupName = group.GroupName, Keyword = group.Keyword});
        }

        public void UpdateLibraryReplaceGroupName(long gid, string oldGroupName, string newGroupName)
        {
            SqlExecuter.Execute(SQLResource.UpdateLibraryReplaceGroupName,
                new
                {
                    Gid = gid,
                    OldGroupName = oldGroupName,
                    NewGroupName = newGroupName
                });
        }

        public IEnumerable<LibraryItem> SelectUnGroupingLibrary(IEnumerable<string> keywords)
        {
            // TODO: SQLBuilderを使うようにする。
            var keywordCond = new StringBuilder();
            foreach (var keyword in keywords)
            {
                if (keywordCond.Length > 0) keywordCond.Append(" OR ");
                keywordCond.Append(string.Format(" lower(TITLE) LIKE '%{0}%'", keyword.ToLower()));
            }
            var sql = string.Format("SELECT ID, TITLE  FROM MOVLIST WHERE GID IS NULL AND ({0})", keywordCond.ToString());
            return SqlExecuter.Query<LibraryItem>(sql);
        }

        public void UpdateLibraryUnGroup(long id)
        {
            SqlExecuter.Execute(SQLResource.UpdateGidById, new { Gid = (long?)null, Id = id });
        }
    }
}

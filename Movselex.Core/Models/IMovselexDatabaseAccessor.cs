using System;
using System.Collections.Generic;
using Firk.Database;

namespace Movselex.Core.Models
{
    internal interface IMovselexDatabaseAccessor : IDatabaseAccessor
    {
        IEnumerable<LibraryItem> SelectLibraryList(LibraryCondition libCondition);
        void ChangeDatabase(string databaseName);
        IEnumerable<GroupItem> SelectGroup();
        IEnumerable<LibraryItem> ShuffleLibrary(int limitNum);
        string GetMostUseDirectoryPath();
        IEnumerable<string> SelectAllLibraryFilePaths();
        bool InsertMediaFile(MediaFile mediaFile);
        IEnumerable<dynamic> SelectMatchGroupKeyword(string keyword);
        void UpdateGroupLastUpdateDatetime(long gid);
        RatingType GetGroupRating(long gid);
        long GetIdFromFileName(string filename);
        void UpdatePlayCount(long id);
        Dictionary<long, string> SelectInCompleteIds();
        bool UpdateMediaFile(MediaFile mediaFile);
        void UpdateLibraryIsPlayed(long id, bool newIsPlayed);
        void UpdateLibraryRating(long id, RatingType newRating);
        IEnumerable<long> SelectIdFromGid(long gid);
        void UpdateGroupIsCompleted(long gid, bool newComplete);
        void UpdateLibraryFilePath(KeyValuePair<long, string> kv);
        long SelectGIdByGroupName(string groupName);
        void InsertGroup(string groupName, string keyword);
        long SelectLastInsertRowId();
        void UpdateGidById(long gid, long id);
        void UpdateGroup(GroupItem group);
        void UpdateLibraryReplaceGroupName(long gid, string oldGroupName, string newGroupName);
        Tuple<string, IEnumerable<LibraryItem>> SelectUnGroupingLibrary(IEnumerable<string> keywords);
        void UpdateLibraryUnGroup(long id);
        void InsertPlayingList(long id, int sort);
        void DeletePlayingList();
        IEnumerable<LibraryItem> SelectPlayingList();
        LibraryItem SelectLibrary(long id);
        void DeleteLibrary(long id);
        Tuple<long?, long?> SelectLibraryPreviousAndNextId(long gid, string no);
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FinalstreamCommons.Extensions;
using FinalstreamCommons.Utils;
using Livet;
using NLog;

namespace Movselex.Core.Models
{
    /// <summary>
    /// Movselexで扱うライブラリを表します。
    /// </summary>
    class MovselexLibrary : NotificationObject, IMovselexLibrary
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        public ObservableCollection<LibraryItem> LibraryItems { get; private set; }

        private readonly IMovselexDatabaseAccessor _databaseAccessor;

        private readonly MovselexGroup _movselexGroup;

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="databaseAccessor"></param>
        /// <param name="movselexGroup"></param>
        public MovselexLibrary(IMovselexDatabaseAccessor databaseAccessor, MovselexGroup movselexGroup)
        {
            _databaseAccessor = databaseAccessor;
            _movselexGroup = movselexGroup;
            LibraryItems = new ObservableCollection<LibraryItem>();
        }

        public LibraryItem GetLibraryItem(long id)
        {
            var library = LibraryItems.FirstOrDefault(x => x.Id == id);
            
            if (library == null)
            {
                // メモリに見つからなかったらDBから取得
                library = _databaseAccessor.SelectLibrary(id);
            }
            return library;
        }

        /// <summary>
        /// ライブラリデータをロードします。
        /// </summary>
        /// <param name="libCondition"></param>
        /// <param name="nowPlayingInfo"></param>
        public void Load(LibraryCondition libCondition, INowPlayingInfo nowPlayingInfo)
        {
            var libraries = _databaseAccessor.SelectLibraryList(libCondition);

            LibraryItems.Clear();
            foreach (var libraryItem in libraries)
            {
                if (nowPlayingInfo.Library != null && nowPlayingInfo.Id == libraryItem.Id) libraryItem.IsPlaying = true; 
                LibraryItems.Add(libraryItem);
            }
        }

        public void Shuffle(int limitNum, LibraryMode libraryMode, bool isSelectAllMovie)
        {
            var libraries = _databaseAccessor.ShuffleLibrary(limitNum, libraryMode, isSelectAllMovie);

            LibraryItems.Clear();
            foreach (var libraryItem in libraries)
            {
                LibraryItems.Add(libraryItem);
            }
        }

        public string SelectMostUseDirectoryPath()
        {
            return _databaseAccessor.GetMostUseDirectoryPath();
        }

        /// <summary>
        /// ライブラリにファイルを登録します。
        /// </summary>
        /// <param name="registFiles"></param>
        /// <param name="progressInfo"></param>
        public void Regist(IEnumerable<string> registFiles, IProgressInfo progressInfo)
        {
            
            using (var tran = _databaseAccessor.BeginTransaction())
            {
                var existsFiles = _databaseAccessor.SelectAllLibraryFilePaths().ToList();

                var i = 1;
                var regfiles = registFiles.ToArray();
                foreach (var registFile in regfiles)
                {
                    progressInfo.UpdateProgressMessage("Regist Files", Path.GetFileName(registFile), i++, regfiles.Length);
                    if (!existsFiles.Contains(registFile))
                    {

                        // 未登録の場合のみ登録する
                        var mediaFile = new MediaFile(registFile);
                        _movselexGroup.SetMovGroup(mediaFile);
                        var isSuccess = _databaseAccessor.InsertMediaFile(mediaFile);
                        if (isSuccess)
                        {
                            _log.Info("Registed Media File. Id:{0} Title:{1} Group:{2}", mediaFile.Id, mediaFile.MovieTitle, mediaFile.GroupName);
                            mediaFile.DebugWriteJson("RegistMedia");
                            existsFiles.Add(registFile);
                        }
                        else
                        {
                            _log.Error("Fail Regist Media File. Id:{0} Title:{1} Group:{2} Detail:{3}", mediaFile.Id, mediaFile.MovieTitle, mediaFile.GroupName, mediaFile.ToJson());
                        }
                    }
                    else
                    {
                        // TODO: 同じものがあればファイルパス更新
                    }
                }
                tran.Commit();
            }


        }

        /// <summary>
        /// 再生カウントをインクリメントします。
        /// </summary>
        /// <param name="id"></param>
        public void IncrementPlayCount(long id)
        {
            _databaseAccessor.UpdatePlayCount(id);

            // ライブラリデータ更新
            var item = GetLibraryItem(id);
            if (item == null) return;

            item.IncrementPlayCount();
            _log.Info("Increment PlayCount. Id:{0} Title:{1} PlayCount:{2}",
                item.Id, item.Title, item.PlayCount);
        }

        /// <summary>
        /// ファイル名からIdを取得します。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public long FindIdByFileName(string filename)
        {
            return _databaseAccessor.GetIdFromFileName(filename);
        }

        public Dictionary<long, string> GetInCompleteIds()
        {
            return _databaseAccessor.SelectInCompleteIds();
        }

        /// <summary>
        /// 不完全なものを再スキャンします。
        /// </summary>
        /// <param name="iddic"></param>
        public void ReScan(Dictionary<long, string> iddic)
        {
            using (var tran = _databaseAccessor.BeginTransaction())
            {

                foreach (var kv in iddic)
                {
                    var mediaFile = new MediaFile(kv.Value);
                    mediaFile.UpdateId(kv.Key);
                    var isSuccess = _databaseAccessor.UpdateMediaFile(mediaFile);
                    if (isSuccess)
                    {
                        _log.Info("Updated Media File. Id:{0} Title:{1}", mediaFile.Id, mediaFile.MovieTitle);
                        mediaFile.DebugWriteJson("UpdateMedia");

                        // ライブラリ更新
                        var item = GetLibraryItem(mediaFile.Id);
                        if (item != null) item.UpdateMediaInfo(mediaFile);
                    }
                    else
                    {
                        _log.Error("Updated Regist Media File. Id:{0} Title:{1} Detail:{2}", mediaFile.Id, mediaFile.MovieTitle, mediaFile.ToJson());
                    }
                }
                tran.Commit();
            }
        }

        public void ModifyIsPlayed(LibraryItem libraryItem)
        {
            var newIsPlayed = !libraryItem.IsPlayed;
            _databaseAccessor.UpdateLibraryIsPlayed(libraryItem.Id, newIsPlayed);
            libraryItem.ModifyIsPlayed(newIsPlayed);
        }

        public void ModifyRating(LibraryItem libraryItem, RatingType rating)
        {
            _databaseAccessor.UpdateLibraryRating(libraryItem.Id, rating);
            libraryItem.ModifyRating(rating);

        }

        /// <summary>
        /// ファイルパスを更新します。
        /// </summary>
        /// <param name="movedDic">key:id value:newFilePath</param>
        public void UpdateFilePaths(Dictionary<long, string> movedDic)
        {
            using (var tran = _databaseAccessor.BeginTransaction())
            {
                foreach (var kv in movedDic)
                {
                    _databaseAccessor.UpdateLibraryFilePath(kv);
                    var library = LibraryItems.FirstOrDefault(x => x.Id == kv.Key);
                    _log.Info("Update Library FilePath. Libarary:{0} NewPath:{1}", library != null? library.Title : kv.Key.ToString(), kv.Value);
                }
                tran.Commit();
            }

            // ライブラリ内ファイルパス更新
            foreach (var kv in movedDic) {
                var library = LibraryItems.FirstOrDefault(x => x.Id == kv.Key);
                if (library == null) continue;
                library.ModifyFilePath(kv.Value);
            }
        }

        public void UnGroup(LibraryItem[] libraries)
        {
            using (var tran = _databaseAccessor.BeginTransaction())
            {
                foreach (var library in libraries)
                {
                    _databaseAccessor.UpdateLibraryUnGroup(library.Id);
                    library.UnGroup();
                }
                tran.Commit();
            }
        }

        public void Delete(LibraryItem libraryItem, bool isDeleteFile)
        {
            _databaseAccessor.DeleteLibrary(libraryItem.Id);
            LibraryItems.Remove(libraryItem);
            _log.Info("Deleted Library. Id:{0} Title:{1} FilePath:{2}", libraryItem.Id, libraryItem.Title, libraryItem.FilePath);
            if (isDeleteFile)
            {
                FileUtils.MoveRecycleBin(libraryItem.FilePath);
                _log.Info("Deleted File. FilePath:{0}", libraryItem.FilePath);
            }
        }

        public void Trimming(int librarySelectIndex, int limitNum)
        {
            var startIndex = librarySelectIndex == -1 ? 0 : librarySelectIndex;
            var libraries = LibraryItems.Skip(librarySelectIndex).Take(limitNum).ToArray();
            LibraryItems.Clear();
            foreach (var libraryItem in libraries)
            {
                LibraryItems.Add(libraryItem);
            }
        }

        public void DiffLoad(LibraryCondition libCondition)
        {
            var libraries = _databaseAccessor.SelectLibraryList(libCondition).ToArray();
            LibraryItems.DiffInsert(libraries,
                new LibraryItemComparer());
        
        }

        public Tuple<long?, long?> GetPreviousAndNextId(long gid, string no)
        {
            return _databaseAccessor.SelectLibraryPreviousAndNextId(gid, no);
        }

        public void ResetIsPlaying(LibraryItem nowLibrary)
        {
            foreach (var libraryItem in LibraryItems)
            {
                libraryItem.IsPlaying = false;
            }
            nowLibrary.IsPlaying = true;
        }

        public void ModifyLibrary(LibraryItem library, Dictionary<string, object> modDataDic)
        {
            library.Update(modDataDic);
            _databaseAccessor.UpdateLibrary(library);

        }
    }
}

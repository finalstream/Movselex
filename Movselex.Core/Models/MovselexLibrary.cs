using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Extentions;
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

        /// <summary>
        /// ライブラリデータをロードします。
        /// </summary>
        /// <param name="libCondition"></param>
        public void Load(LibraryCondition libCondition)
        {
            var libraries = _databaseAccessor.SelectLibrary(libCondition);

            LibraryItems.Clear();
            foreach (var libraryItem in libraries)
            {
                LibraryItems.Add(libraryItem);
            }
        }

        public void Shuffle(int limitNum)
        {
            var libraries = _databaseAccessor.ShuffleLibrary(limitNum);

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
        public void Regist(IEnumerable<string> registFiles)
        {
            using (var tran = _databaseAccessor.BeginTransaction())
            {
                var existsFiles = _databaseAccessor.SelectAllLibraryFilePaths().ToList();

                foreach (var registFile in registFiles)
                {
                    if (!existsFiles.Contains(registFile))
                    {
                        // 未登録の場合のみ登録する
                        var mediaFile = new MediaFile(registFile);
                        _movselexGroup.SetMovGroup(mediaFile);
                        var isSuccess = _databaseAccessor.RegistMediaFile(mediaFile);
                        if (isSuccess)
                        {
#if DEBUG
                            _log.Info("Registed Media File. Id:{0} Title:{1} Group:{2} Detail:{3}", mediaFile.Id, mediaFile.MovieTitle, mediaFile.GroupName, mediaFile.ToJson());
#else
                            _log.Info("Registed Media File. Id:{0} Title:{1} Group:{2}", mediaFile.Id, mediaFile.MovieTitle, mediaFile.GroupName);
#endif
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
    }
}

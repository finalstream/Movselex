using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FinalstreamCommons.Utils;
using NLog;

namespace Movselex.Core.Models.Actions
{
    internal class MoveLibraryFileAction : MovselexAction
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly string _moveDestDirectory;
        private readonly LibraryItem[] _selectLibraries;

        public MoveLibraryFileAction(string moveDestDirectory, LibraryItem[] selectLibraries)
        {
            _moveDestDirectory = moveDestDirectory;
            _selectLibraries = selectLibraries;
        }

        public async override void InvokeCore(MovselexClient client)
        {
            client.IsProgressing = true;

            // TODO: MoveDirectoryActionと共通化する。

            //TODO: メッセージを国際化する。
            var appConfig = client.AppConfig;

            var movedDic = new Dictionary<long, string>();


            await Task.Run(() =>
            {
                // ファイル移動は時間がかかるので別スレッドで。

                var i = 1;
                foreach (var library in _selectLibraries)
                {
                    var oldFilePath = library.FilePath;
                    var newfilepath = Path.Combine(_moveDestDirectory, Path.GetFileName(oldFilePath));

                    client.ProgressInfo.UpdateProgressMessage("Moving Library Files",oldFilePath, i++,
                        _selectLibraries.Length);

                    var isMoved = FileUtils.Move(oldFilePath, newfilepath);

                    if (isMoved)
                    {
                        _log.Info("Moved File. {0} to {1}.", library.FilePath, newfilepath);

                        movedDic.Add(library.Id, newfilepath);

                        DirecotryUtils.DeleteEmptyDirecotry(Path.GetDirectoryName(library.FilePath));
                    }
                    else
                    {
                        _log.Warn(" Fail Move File. {0} to {1}.", oldFilePath, newfilepath);
                    }
                    
                }

            });

            client.PostCallback(new CallbackAction(() =>
            {
                client.MovselexLibrary.UpdateFilePaths(movedDic);
                client.IsProgressing = false;
            }));
            
            
        }
    }
}
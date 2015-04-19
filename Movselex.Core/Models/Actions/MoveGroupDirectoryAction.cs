using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Windows.Forms;
using FinalstreamCommons.Utils;
using Microsoft.Build.Utilities;
using NLog;
using Logger = NLog.Logger;
using Task = System.Threading.Tasks.Task;

namespace Movselex.Core.Models.Actions
{
    internal class MoveGroupDirectoryAction : MovselexProgressAction
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly GroupItem _group;
        private readonly string _baseDirectory;

        public MoveGroupDirectoryAction(GroupItem group, string baseDirectory)
        {
            _group = group;
            _baseDirectory = baseDirectory;
        }

        public override void InvokeProgress(MovselexClient client)
        {
            //TODO: メッセージを国際化する。
            var appConfig = client.AppConfig;
           
            appConfig.MoveBaseDirectory = _baseDirectory;

            // 移動先フォルダ作成
            var moveDirectory = _baseDirectory + "\\" + _group.GroupName;

            Directory.CreateDirectory(moveDirectory);

            var movedDic = new Dictionary<long, string>();

            var i = 1;
            foreach (var library in client.Libraries)
            {
                var newfilepath = Path.Combine(moveDirectory, Path.GetFileName(library.FilePath));

                client.ProgressInfo.UpdateProgressMessage("Moving Group Files", _group.GroupName, i++ ,client.Libraries.Count);

                FileUtils.Move(library.FilePath, newfilepath);

                _log.Debug("Moved File. {0} to {1}.", library.FilePath, newfilepath);

                movedDic.Add(library.Id, newfilepath);

                DirecotryUtils.DeleteEmptyDirecotry(Path.GetDirectoryName(library.FilePath));
            }

            // ファイルパス更新
            client.MovselexLibrary.UpdateFilePaths(movedDic);

            _group.ModifyDriveLetter(FileUtils.GetDriveLetter(moveDirectory));

            _log.Info("Moved Group Direcotry. Group:{0} NewPath:{1}", _group.GroupName, moveDirectory);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Windows.Forms;
using FinalstreamCommons.Utils;
using NLog;

namespace Movselex.Core.Models.Actions
{
    internal class MoveGroupDirectoryAction : MovselexProgressAction
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly GroupItem _group;
        private readonly IEnumerable<LibraryItem> _libraries; 

        public MoveGroupDirectoryAction(GroupItem group, IEnumerable<LibraryItem> libraries)
        {
            _group = group;
            _libraries = libraries;
        }

        public override void InvokeProgress(MovselexClient client)
        {
            //TODO: メッセージを国際化する。
            var appConfig = client.AppConfig;
            var baseDirectory = DialogUtils.ShowFolderDialog(
                "移動する場所を指定してください。指定した場所にグループ名でフォルダを作成して移動します。",
                appConfig.MoveBaseDirectory);

            if (!string.IsNullOrEmpty(baseDirectory))
            {
                appConfig.MoveBaseDirectory = baseDirectory;

                // 移動先フォルダ作成
                var moveDirectory = baseDirectory + "\\" + _group.GroupName;

                if (MessageUtils.ShowQuestionYesNo(
                    string.Format("{0}を{1}に移動します。よろしいですか？",
                    _group.GroupName, moveDirectory)) == DialogResult.Yes)
                {
                    Directory.CreateDirectory(moveDirectory);

                    var movedDic = new Dictionary<long, string>();

                    foreach (var library in _libraries)
                    {
                        var newfilepath = Path.Combine(moveDirectory, Path.GetFileName(library.FilePath));

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
    }
}
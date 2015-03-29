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
            //TODO: ���b�Z�[�W�����ۉ�����B
            var appConfig = client.AppConfig;
            var baseDirectory = DialogUtils.ShowFolderDialog(
                "�ړ�����ꏊ���w�肵�Ă��������B�w�肵���ꏊ�ɃO���[�v���Ńt�H���_���쐬���Ĉړ����܂��B",
                appConfig.MoveBaseDirectory);

            if (!string.IsNullOrEmpty(baseDirectory))
            {
                appConfig.MoveBaseDirectory = baseDirectory;

                // �ړ���t�H���_�쐬
                var moveDirectory = baseDirectory + "\\" + _group.GroupName;

                if (MessageUtils.ShowQuestionYesNo(
                    string.Format("{0}��{1}�Ɉړ����܂��B��낵���ł����H",
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

                    // �t�@�C���p�X�X�V
                    client.MovselexLibrary.UpdateFilePaths(movedDic);

                    _group.ModifyDriveLetter(FileUtils.GetDriveLetter(moveDirectory));

                    _log.Info("Moved Group Direcotry. Group:{0} NewPath:{1}", _group.GroupName, moveDirectory);
                }

            }
        }
    }
}
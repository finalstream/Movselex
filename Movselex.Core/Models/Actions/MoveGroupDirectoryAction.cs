using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FinalstreamCommons.Utils;
using Microsoft.Build.Utilities;
using NLog;
using Logger = NLog.Logger;
using Task = System.Threading.Tasks.Task;

namespace Movselex.Core.Models.Actions
{
    internal class MoveGroupDirectoryAction : MovselexAction
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        private readonly GroupItem _group;
        private readonly string _baseDirectory;

        public MoveGroupDirectoryAction(GroupItem group, string baseDirectory)
        {
            _group = group;
            _baseDirectory = baseDirectory;
        }

        public async override void InvokeCore(MovselexClient client)
        {
            client.IsProgressing = true;

            //TODO: ���b�Z�[�W�����ۉ�����B
            var appConfig = client.AppConfig;
           
            appConfig.MoveBaseDirectory = _baseDirectory;

            // �ړ���t�H���_�쐬
            var moveDirectory = _baseDirectory + "\\" + _group.GroupName;

            Directory.CreateDirectory(moveDirectory);

            var movedDic = new Dictionary<long, string>();


            var moveLibraries = client.Libraries.ToArray();

            await Task.Run(() =>
            {
                // �t�@�C���ړ��͎��Ԃ�������̂ŕʃX���b�h�ŁB

                var i = 1;
                foreach (var library in moveLibraries)
                {
                    var newfilepath = Path.Combine(moveDirectory, Path.GetFileName(library.FilePath));

                    client.ProgressInfo.UpdateProgressMessage("Moving Group Files", _group.GroupName, i++,
                        moveLibraries.Length);

                    FileUtils.Move(library.FilePath, newfilepath);

                    _log.Info("Moved File. {0} to {1}.", library.FilePath, newfilepath);

                    movedDic.Add(library.Id, newfilepath);

                    DirecotryUtils.DeleteEmptyDirecotry(Path.GetDirectoryName(library.FilePath));
                }

            });

            client.PostCallback(new CallbackAction(() =>
            {
                client.MovselexLibrary.UpdateFilePaths(movedDic);

                if (_group != null) _group.ModifyDriveLetter(FileUtils.GetDriveLetter(moveDirectory));

                _log.Info("Moved Group Direcotry. Group:{0} NewPath:{1}", _group.GroupName, moveDirectory);

                client.IsProgressing = false;
            }));
            
            

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Windows;
using Movselex.Core.Models;

namespace Movselex.Core
{
    class MpcPlayerInfoGetter : PlayerInfoGetter
    {

        public PlayerMediaInfo Get(string exePath)
        {
            SearchProcess(exePath);

            if (HasExited()) return PlayerMediaInfo.Empty;  // プロセスが終了していたら抜ける

            var timeString = Win32Api.GetWindowCaption(Process.MainWindowHandle, "#32770", @"\d*:?\d*:\d* / \d*:?\d*:\d*");
            
            return new PlayerMediaInfo(Process.MainWindowTitle, timeString);
        }

    }
}

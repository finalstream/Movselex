using System.IO;
using System.Text.RegularExpressions;
using FinalstreamCommons.Windows;

namespace Movselex.Core.Models
{
    class MpcPlayerInfoGetter : PlayerInfoGetter
    {

        public PlayerMediaInfo Get(string exePath)
        {
            SearchProcess(exePath);

            if (HasExited()) return PlayerMediaInfo.Empty;  // プロセスが終了していたら抜ける

            var timeString = Win32Api.GetWindowCaption(Process.MainWindowHandle, "#32770", @"\d*:?\d*:\d* / \d*:?\d*:\d*");

            var title = Process.MainWindowTitle;

            // MPC-BE対応
            title = Path.GetFileNameWithoutExtension(new Regex("- MPC-BE.*").Replace(title, ""));

            return new PlayerMediaInfo(title, timeString);
        }

    }
}

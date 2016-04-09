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
            
            return new PlayerMediaInfo(Process.MainWindowTitle, timeString);
        }

    }
}

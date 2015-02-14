using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core
{
    abstract class PlayerInfoGetter
    {

        protected Process Process { get; set; }

        /// <summary>
        /// プロセスを検索します。
        /// </summary>
        /// <returns></returns>
        protected bool SearchProcess(string exePath)
        {
            Process[] processes =　Process.GetProcessesByName(Path.GetFileNameWithoutExtension(exePath));
            if (processes.Length > 0)
            {
                Process = processes[0];
                return true;
            }
            Process = null;
            return false;
        }


        /// <summary>
        /// プロセスが終了しているかどうかを取得します。
        /// </summary>
        /// <returns></returns>
        protected bool HasExited()
        {
            if (this.Process == null) return true;
            if (Process.HasExited)
            {
                // プロセス終了検知
                Process.Refresh();
                Process = null;
                return true;
            }
            return false;
        }

    }
}

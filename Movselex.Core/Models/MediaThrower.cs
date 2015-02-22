using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;

namespace Movselex.Core.Models
{
    /// <summary>
    /// メディアファイルをプレイヤーに投げるやつを表します。
    /// </summary>
    abstract class MediaThrower
    {
        private readonly string _playerExePath;

        private readonly CommandLineBuilder _commandLineBuilder;

        protected MediaThrower(string playerExePath)
        {
            _playerExePath = playerExePath;
            _commandLineBuilder = new CommandLineBuilder();
        }


    }
}

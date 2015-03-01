using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    class MediaPlayerClassicThrower : MediaThrower
    {
        public MediaPlayerClassicThrower(IMovselexAppConfig appConfig) : base(appConfig)
        {
        }

        /// <summary>
        /// コマンドラインパラメタを生成します。
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="isFirst"></param>
        /// <returns></returns>
        protected override string CreateCommandLineParameter(string filepath, bool isFirst)
        {

            // ファイルパス
            CommandLineBuilder.AppendFileNameIfNotNull(filepath);

            // 追加
            if (!isFirst) CommandLineBuilder.AppendSwitch("/add");

            // スクリーン指定
            CommandLineBuilder.AppendSwitchUnquotedIfNotNull("/monitor ", AppConfig.ScreenNo.ToString());

            if (AppConfig.IsFullScreen) CommandLineBuilder.AppendSwitch("/fullscreen");

            return CommandLineBuilder.ToString();

        }
    }
}

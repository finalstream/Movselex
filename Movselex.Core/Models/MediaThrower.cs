using System.Diagnostics;
using System.IO;
using FinalstreamCommons.Builders;
using FinalstreamCommons.Models;
using NLog;

namespace Movselex.Core.Models
{
    /// <summary>
    ///     メディアファイルをプレイヤーに投げるやつを表します。
    /// </summary>
    internal abstract class MediaThrower
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        protected readonly IMovselexAppConfig AppConfig;

        protected readonly CommandLineBuilder CommandLineBuilder;

        /// <summary>
        ///     新しいインスタンスを生成します。
        /// </summary>
        /// <param name="appConfig"></param>
        protected MediaThrower(IMovselexAppConfig appConfig)
        {
            AppConfig = appConfig;
            CommandLineBuilder = new CommandLineBuilder();
        }

        protected abstract string CreateCommandLineParameter(string filepath, bool isFirst);

        /// <summary>
        ///  メディアファイルをプレイヤーに投げます。
        /// </summary>
        /// <param name="filepaths"></param>
        public void Throw(string[] filepaths)
        {
            // TODO: exeパスが設定されていないときはボタンを押せないようにする。
            int i = 0;
            foreach (var filepath in filepaths)
            {
                CommandLineBuilder.Clear();
                
                var param = CreateCommandLineParameter(filepath, i==0);
                var exePath = "\"" + AppConfig.MpcExePath + "\"";

                _log.Debug("[Throw {0}] {1} {2}", i, exePath, param);
                var process = Process.Start(exePath, param);
                process.WaitForInputIdle();

                i++;
            }
            
        }
    }
}
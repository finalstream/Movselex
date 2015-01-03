using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FinalstreamCommons.Models;
using Livet;
using Movselex.Core.Models;

namespace Movselex.Core
{
    public interface IMovselexClient : IDisposable
    {
        
        /// <summary>
        /// フィルタリング情報。
        /// </summary>
        IEnumerable<FilteringItem> Filterings { get; }

        /// <summary>
        /// ライブラリ情報。
        /// </summary>
        IEnumerable<LibraryItem> Libraries { get; }

        /// <summary>
        /// 再生中情報。
        /// </summary>
        INowPlayingInfo NowPlayingInfo { get; }

        /// <summary>
        /// アプリ設定。
        /// </summary>
        MovselexAppConfig AppConfig { get; }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        void Initialize();

        void Finish();

        void ExecEmpty();
    }
}
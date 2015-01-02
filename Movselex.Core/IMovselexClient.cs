using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using FinalstreamCommons.Models;
using Movselex.Core.Models;

namespace Movselex.Core
{
    public interface IMovselexClient
    {
        
        /// <summary>
        /// フィルタリング情報。
        /// </summary>
        IMovselexFiltering Filterings { get; }

        INowPlayingInfo NowPlayingInfo { get;  }

        /// <summary>
        /// 初期化を行います。
        /// </summary>
        void Initialize();

        void Finish();

        void ExecEmpty();
    }
}
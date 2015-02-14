using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{


    /// <summary>
    /// 外部のプレイヤーから再生情報を収集します。
    /// </summary>
    class PlayerMediaCrawler : IDisposable
    {

        private CancellationTokenSource _cancellationTokenSource;

        private readonly MpcPlayerInfoGetter _mpcPlayerInfoGetter;

        private readonly string _exePath;

        #region Updatedイベント

        /// <summary>
        /// 再生情報が更新された時に発生するイベントです。
        /// </summary>
        public event EventHandler<PlayerMediaInfo> Updated;

        protected virtual void OnUpdated(PlayerMediaInfo arg)
        {
            var handler = this.Updated;
            if (handler != null)
            {
                handler(this, arg);
            }
        }

        #endregion


        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public PlayerMediaCrawler(string exePath)
        {
            _exePath = exePath;
            _mpcPlayerInfoGetter = new MpcPlayerInfoGetter();
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            // タスクを開始する。
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var info = _mpcPlayerInfoGetter.Get(_exePath);
                    OnUpdated(info);

                    Task.Delay(1000).Wait();
                }
            },
            _cancellationTokenSource.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        #region Dispose

        // Flag: Has Dispose already been called?
        private bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
                _cancellationTokenSource.Cancel();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion
    }
}

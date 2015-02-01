using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{


    /// <summary>
    /// 外部のプレイヤーから再生情報を収集します。
    /// </summary>
    class MediaCrawler : IDisposable
    {

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public MediaCrawler()
        {
            
            // タスクを開始する。
            Task.Factory.StartNew(() =>
            {

                Task.Delay(1000).Wait();
            }, 
            _cancellationTokenSource.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);


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

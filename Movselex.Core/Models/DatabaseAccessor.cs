using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Models;

namespace Movselex.Core.Models
{
    internal class DatabaseAccessor : IDatabaseAccessor
    {

        private SQLExecuter _sqlExecuter;

        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="databaseName"></param>
        public DatabaseAccessor(string databaseName)
        {
            _sqlExecuter = MovselexSQLExecuterFactory.Create(databaseName);
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
                if (_sqlExecuter != null) _sqlExecuter.Dispose();
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion
    }
}

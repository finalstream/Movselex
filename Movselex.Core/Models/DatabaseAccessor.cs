using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Models;
using Movselex.Core.Resources;

namespace Movselex.Core.Models
{
    internal class DatabaseAccessor : IDatabaseAccessor
    {
        public string DatabaseName { get; private set; }

        private SQLExecuter _sqlExecuter;

        private SQLBuilder _sqlBuilder;

        private string _lastLibrarySelectSQL;


        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="databaseName"></param>
        public DatabaseAccessor(string databaseName)
        {
            ChangeDatabase(databaseName);
            _sqlBuilder = new SQLBuilder();
        }

        public IEnumerable<LibraryItem> SelectLibrary()
        {
            // TODO:SQLをビルドする。
            var sql = SQLResource.SQL001;
            var aaa = _sqlExecuter.Query(sql);
            _lastLibrarySelectSQL = sql;
            return _sqlExecuter.Query<LibraryItem>(SQLResource.SQL001);

        }

        public void ChangeDatabase(string databaseName)
        {
            DatabaseName = databaseName;
            _sqlExecuter = MovselexSQLExecuterFactory.Create(databaseName);
            _lastLibrarySelectSQL = SQLResource.SQL001;
        }

        public IEnumerable<GroupItem> SelectGroup()
        {
            //throw new NotImplementedException();
            return Enumerable.Empty<GroupItem>();
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

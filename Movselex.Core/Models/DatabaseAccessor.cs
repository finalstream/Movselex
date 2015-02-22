using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;
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

        private readonly MovselexAppConfig _appConfig;


        /// <summary>
        /// 新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="databaseName"></param>
        public DatabaseAccessor(MovselexAppConfig appConfig)
        {
            _appConfig = appConfig;
            ChangeDatabase(_appConfig.SelectDatabase);
            _sqlBuilder = new SQLBuilder();
        }

        /// <summary>
        /// ライブラリを取得します。
        /// </summary>
        /// <param name="libCondition"></param>
        /// <returns></returns>
        public IEnumerable<LibraryItem> SelectLibrary(LibraryCondition libCondition)
        {
            _lastLibrarySelectSQL = _sqlBuilder.CreateSelectLibrary(_appConfig.LibraryMode, libCondition);
            return _sqlExecuter.Query<LibraryItem>(_lastLibrarySelectSQL);

        }

        /// <summary>
        /// データベースを変更します。
        /// </summary>
        /// <param name="databaseName"></param>
        /// <remarks>SQLExecuterでコネクションを管理しています。</remarks>
        public void ChangeDatabase(string databaseName)
        {
            DatabaseName = databaseName;
            _sqlExecuter = MovselexSQLExecuterFactory.Create(databaseName);
            //_lastLibrarySelectSQL = SQLResource.SelectLibraryList;
        }

        /// <summary>
        /// グループを取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GroupItem> SelectGroup()
        {
            return _sqlExecuter.Query<GroupItem>(
                _sqlBuilder.CreateSelectGroup(_appConfig.LibraryMode, _lastLibrarySelectSQL));
        }

        public IEnumerable<LibraryItem> ShuffleLibrary(int limitNum)
        {
            if (string.IsNullOrEmpty(_lastLibrarySelectSQL)) return Enumerable.Empty<LibraryItem>();
            var sql = SQLResource.SelectShuffleLibrary.Replace("#LastExecSql#", _lastLibrarySelectSQL);
            return _sqlExecuter.Query<LibraryItem>(
                sql, 
                new { LimitNum = limitNum });
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

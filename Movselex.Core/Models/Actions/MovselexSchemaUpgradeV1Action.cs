using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Database;
using NLog;

namespace Movselex.Core.Models.Actions
{
    class MovselexSchemaUpgradeV1Action : MovselexSchemaUpgradeAction
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        protected override int GetUpgradeSchemaVersion()
        {
            return 1;
        }

        protected override bool InvokeUpgradeCore(SQLExecuter sqlExecuter)
        {
            using (var tran = sqlExecuter.BeginTransaction())
            {
                try
                {
                    sqlExecuter.Execute("ALTER TABLE MOVLIST ADD COLUMN SEASON TEXT");


                }
                catch (SQLiteException ex)
                {
                    tran.Rollback();
                    _log.Error(ex);
                    return false;
                }
            }
            return true;
        }
    }
}

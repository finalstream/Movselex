using System.Data.SQLite;
using FinalstreamCommons.Utils;
using Firk.Database;
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

                    var libraries = sqlExecuter.Query<LibraryItem>("SELECT * FROM MOVLIST");

                    foreach (var library in libraries)
                    {
                        var season = string.Format(ApplicationDefinitions.SeasonFormat, library.Date.Year, DateUtils.GetSeasonString(library.Date));

                        sqlExecuter.Execute("UPDATE MOVLIST SET SEASON = @Season WHERE ID = @Id", new { Id = library.Id, Season = season });
                    }

                    tran.Commit();
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

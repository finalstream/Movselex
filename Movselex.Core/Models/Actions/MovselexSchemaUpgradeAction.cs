using System.IO;
using Firk.Core.Actions;
using Firk.Database;
using NLog;

namespace Movselex.Core.Models.Actions
{
    abstract class MovselexSchemaUpgradeAction : SchemaUpgradeAction<MovselexClient>
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();

        protected abstract bool InvokeUpgradeCore(SQLExecuter sqlExecuter);

        protected override bool InvokeUpgrade(MovselexClient client)
        {
            var dbfiles = Directory.GetFiles(ApplicationDefinitions.DatabaseDirectory, "*.db", SearchOption.TopDirectoryOnly);

            client.IsProgressing = true;
            client.ProgressInfo.SetProgressMessage("Database Upgrading...");
            foreach (var dbfile in dbfiles)
            {
                using (var sqlExecuter = SQLExecuterFactory.Create(dbfile, ApplicationDefinitions.SupportSQLiteFunctions))
                {
                    var result = InvokeUpgradeCore(sqlExecuter);
                    if (!result) return false;
                }     
            }
                     
            client.IsProgressing = false;

            return true;
        }
    }
}

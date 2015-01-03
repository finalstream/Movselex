using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Models;
using FinalstreamCommons.Models.SQLiteFunctions;
using Movselex.Core.Models.SQLiteFunctions;

namespace Movselex.Core.Models
{
    internal class MovselexSQLExecuterFactory
    {

        public static SQLExecuter Create(string databaseName)
        {
            var databaseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                ApplicationDefinitions.DatabaseDirectory);
            var databaseFilePath = Path.Combine(databaseDirectory, string.Format("{0}.db", databaseName));

            if (!File.Exists(databaseFilePath)) CreateBlankDatabase(databaseDirectory, databaseFilePath);

            return new SQLExecuter(databaseFilePath, new [] { typeof(SumStringSQLiteFunction), typeof(GetFileSizeSQLiteFunction), typeof(IsMatchMigemoSQLiteFunction) });
        }

        private static void CreateBlankDatabase(string databaseDirecotry, string databaseFilePath)
        {
            File.Copy(Path.Combine(databaseDirecotry, "blank.movselexdatabase"), databaseFilePath);
        }
    }
}

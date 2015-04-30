using System;
using System.Data.SQLite;
using System.IO;
using FinalstreamCommons.Libraries;

namespace Movselex.Core.Models.SQLiteFunctions
{
    [SQLiteFunction(Name = "ISMATCHMIGEMO", FuncType = FunctionType.Scalar, Arguments = 2)]
    public class IsMatchMigemoSQLiteFunction : SQLiteFunction
    {
        private Migemo migemo;
        public IsMatchMigemoSQLiteFunction()
        {
            migemo = new Migemo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ApplicationDefinitions.MigemoDictionaryFilePath));
        }
        public override object Invoke(object[] args)
        {
            
            return migemo.GetRegex(args[0].ToString()).IsMatch(args[1].ToString());
        }
    }
}

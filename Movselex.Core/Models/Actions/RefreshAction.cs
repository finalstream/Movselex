using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Utils;
using FinalstreamCommons.Extentions;

namespace Movselex.Core.Models.Actions
{
    internal class RefreshAction : MovselexActionBase
    {

        private string _selectedDatabase;

        public RefreshAction(string selectedDatabase)
        {
            _selectedDatabase = selectedDatabase;
        }

        public override void InvokeCore(MovselexClient client)
        {
            // データベース一覧
            var dbnames = Directory.GetFiles(
                ApplicationDefinitions.DatabaseDirectory, "*.db", SearchOption.TopDirectoryOnly).Select(Path.GetFileNameWithoutExtension);
            client.Databases.DiffUpdate(dbnames.ToArray());
            
            // フィルタリングロード
            client.MovselexFiltering.Load();
            
            client.MovselexLibrary.Load();

            client.MovselexGroup.Load();

        }
    }
}

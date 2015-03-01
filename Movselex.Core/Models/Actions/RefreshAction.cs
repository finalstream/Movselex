using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalstreamCommons.Utils;
using FinalstreamCommons.Extentions;
using NLog;

namespace Movselex.Core.Models.Actions
{
    internal class RefreshAction : MovselexActionBase
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private FilteringMode _filteringMode;

        public RefreshAction(FilteringMode filteringMode)
        {
            _filteringMode = filteringMode;
        }

        public override void InvokeCore(MovselexClient client)
        {
            // データベース一覧
            var dbnames = Directory.GetFiles(
                ApplicationDefinitions.DatabaseDirectory, "*.db", SearchOption.TopDirectoryOnly).Select(Path.GetFileNameWithoutExtension);

            client.Databases.DiffUpdate(dbnames.ToArray());

            // フィルタリングロード
            client.MovselexFiltering.Load();

            // 選択状態のフィルタのSQLを取得してロード
            var libCondition = new LibraryCondition(_filteringMode,
                client.MovselexFiltering.FilteringItems.Where(x => x.IsSelected).Select(x=>x.Value).FirstOrDefault());
            client.MovselexLibrary.Load(libCondition);

            client.MovselexGroup.Load();

        }
    }
}

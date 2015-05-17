﻿using System;
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
    internal class RefreshAction : MovselexAction
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly FilteringMode _filteringMode;

        public RefreshAction()
        {
            
        }

        public RefreshAction(FilteringMode filteringMode)
        {
            _filteringMode = filteringMode;
        }

        public override void InvokeCore(MovselexClient client)
        {
            //LoadDatabase(client);

            // フィルタリングロード
            //client.MovselexFiltering.Load(client.AppConfig.Language);

            LibraryCondition libCondition;
            switch (_filteringMode)
            {
                case FilteringMode.SQL:
                    // 選択状態のフィルタのSQLを取得してロード
                    libCondition = new LibraryCondition(_filteringMode,
                        client.MovselexFiltering.FilteringItems.Where(x => x.IsSelected).Select(x => x.Value).FirstOrDefault(),
                        client.AppConfig.MaxLimitNum);
                    break;
                case FilteringMode.Group:
                    // 選択状態のグループのSQLを取得してロード
                    libCondition = new LibraryCondition(_filteringMode,
                        client.MovselexGroup.GroupItems.Where(x => x.IsSelected).Select(x => new FilteringCondition(x.GroupName, false)).FirstOrDefault());
                    break;
                default:
                    return;
            }
            
            client.MovselexLibrary.Load(libCondition);

            if(_filteringMode == FilteringMode.SQL) client.MovselexGroup.Load();

            client.MovselexPlaying.Load();
        }

        /// <summary>
        /// データベースをロードします。
        /// </summary>
        protected void LoadDatabase(MovselexClient client)
        {
            // データベース一覧
            var dbnames = Directory.GetFiles(
                ApplicationDefinitions.DatabaseDirectory, "*.db", SearchOption.TopDirectoryOnly).Select(Path.GetFileNameWithoutExtension);

            client.Databases.DiffUpdate(dbnames.ToArray());

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    /// <summary>
    /// 差分更新するアクションを表します。
    /// </summary>
    class DiffRefreshAction : RefreshAction
    {
        public DiffRefreshAction(FilteringMode filteringMode) : base(filteringMode)
        {
            
        }

        public override void InvokeCore(MovselexClient client)
        {
            var libCondition = CreateLibraryCondition(client);

            client.MovselexLibrary.DiffLoad(libCondition);

            if (libCondition.FilteringMode != FilteringMode.Group) client.MovselexGroup.DiffLoad();

            client.MovselexPlaying.Load();
        }
    }
}

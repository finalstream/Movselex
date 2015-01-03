using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    internal class RefreshAction : MovselexActionBase
    {
        public override void InvokeCore(MovselexClient client)
        {
            // フィルタリングロード
            client.MovselexFiltering.Load();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    class ReloadFilteringAction : MovselexAction
    {
        public override void InvokeCore(MovselexClient client)
        {
            // フィルタリングロード
            client.MovselexFiltering.Load(client.AppConfig.Language);
        }
    }
}

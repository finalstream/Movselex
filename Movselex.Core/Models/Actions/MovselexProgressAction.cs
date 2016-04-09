using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    /// <summary>
    /// 進捗状況をサポートするアクションを表します。
    /// </summary>
    abstract class MovselexProgressAction : MovselexAction
    {
        public abstract void InvokeProgress(MovselexClient client);

        public override void InvokeCore(MovselexClient client)
        {
            client.IsProgressing = true;
            this.InvokeProgress(client);
            client.IsProgressing = false;
        }
    }
}

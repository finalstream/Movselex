using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    class ShuffleLibraryAction : MovselexActionBase
    {

        public int LimitNum { get; private set; }

        public ShuffleLibraryAction(int limitNum)
        {
            LimitNum = limitNum;
        }

        /// <summary>
        /// アクションを実行します。
        /// </summary>
        /// <param name="client"></param>
        public override void InvokeCore(MovselexClient client)
        {

            client.MovselexLibrary.Shuffle(LimitNum);
            client.MovselexGroup.Load(client.MovselexLibrary.LibraryItems.Where(x=>x.Gid != 0).Select(x=>x.Gid).Distinct());
        }
    }
}

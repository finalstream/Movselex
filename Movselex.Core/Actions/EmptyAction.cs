using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Actions
{
    internal class EmptyAction : MovselexActionBase
    {
        public override void InvokeCore(IMovselexClient client)
        {
            ;
        }
    }
}

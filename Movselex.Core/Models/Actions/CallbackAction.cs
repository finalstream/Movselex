using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models.Actions
{
    class CallbackAction : MovselexAction
    {
        private readonly Action _action;

        public CallbackAction(Action action)
        {
            _action = action;
        }

        public override void InvokeCore(MovselexClient client)
        {
            _action.Invoke();
        }
    }
}

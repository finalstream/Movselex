using System;
using Movselex.Core.Models;
using Movselex.Core.Models.Actions;

namespace Movselex.Core
{
    internal class ModifyIsCompleteAction : MovselexActionBase
    {
        private readonly GroupItem _group;
        public ModifyIsCompleteAction(GroupItem group)
        {
            _group = group;
        }

        public override void InvokeCore(MovselexClient client)
        {
            client.MovselexGroup.ModifyIsComplete(_group);
        }
    }
}
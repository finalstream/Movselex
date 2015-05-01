using System;

namespace Movselex.Core.Models.Actions
{
    internal class ModifyGroupAction : MovselexAction
    {
        private readonly GroupItem _group;
        private readonly string _groupName;
        private readonly string _keyword;

        public ModifyGroupAction(GroupItem group, string groupName, string keyword)
        {
            _group = group;
            _groupName = groupName;
            _keyword = keyword;
        }

        public override void InvokeCore(MovselexClient client)
        {
            client.MovselexGroup.ModifyGroup(_group, _groupName, _keyword);
        }
    }
}
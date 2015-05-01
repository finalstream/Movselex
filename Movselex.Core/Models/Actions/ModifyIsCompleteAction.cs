namespace Movselex.Core.Models.Actions
{
    internal class ModifyIsCompleteAction : MovselexAction
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
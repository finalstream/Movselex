using System.Collections.Generic;

namespace Movselex.Core.Models
{
    internal class GroupItemComparer : IEqualityComparer<GroupItem>
    {
        public bool Equals(GroupItem x, GroupItem y)
        {
            if (x == null && y == null) return true;
            return x.Gid == y.Gid;
        }

        public int GetHashCode(GroupItem obj)
        {
            return obj.Gid.GetHashCode();
        }
    }
}
using System.Collections.Generic;

namespace Movselex.Core.Models
{
    internal class LibraryItemComparer : IEqualityComparer<LibraryItem>
    {
        public bool Equals(LibraryItem x, LibraryItem y)
        {
            if (x == null && y == null) return true;
            return x.Id == y.Id;
        }

        public int GetHashCode(LibraryItem obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
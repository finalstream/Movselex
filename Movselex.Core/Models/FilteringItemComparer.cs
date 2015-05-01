using System.Collections.Generic;

namespace Movselex.Core.Models
{
    class FilteringItemComparer : IEqualityComparer<FilteringItem>
    {

        public bool Equals(FilteringItem x, FilteringItem y)
        {
            if (x == null && y == null) return true;
            return x.DisplayValue == y.DisplayValue;
        }

        public int GetHashCode(FilteringItem obj)
        {
            return obj.DisplayValue.GetHashCode();
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    /// <summary>
    /// コンディションリストの１レコードを表します。
    /// </summary>
    public class FilteringItem
    {
        public string Value { get; private set; }

        public string DisplayValue { get; private set; }

        public FilteringItem(string value, string displayValue)
        {
            Value = value;
            DisplayValue = displayValue;
        }

        public override string ToString()
        {
            return DisplayValue;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    class FilteringConfig
    {

        public string Value { get; private set; }

        public Dictionary<string, string> DisplayValue { get; private set; }

        public FilteringConfig(string value, Dictionary<string, string> displayValue)
        {
            Value = value;
            DisplayValue = displayValue;
        }
    }
}

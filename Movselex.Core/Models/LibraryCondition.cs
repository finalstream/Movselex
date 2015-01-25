using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    class LibraryCondition
    {
        public FilteringMode FilteringMode { get; private set; }

        public string ConditionSQL { get; private set; }

        public LibraryCondition(FilteringMode filteringMode, string conditionSql)
        {
            FilteringMode = filteringMode;
            ConditionSQL = conditionSql?? "";
        }
    }
}

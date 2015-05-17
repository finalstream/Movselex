using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core.Models
{
    public class FilteringCondition
    {
        public static FilteringCondition Empty = new FilteringCondition("", false);

        public string Sql { get; private set; }

        public bool IsLimited { get; private set; }

        public FilteringCondition(string sql, bool isLimited)
        {
            Sql = sql;
            IsLimited = isLimited;
        }

    }
}

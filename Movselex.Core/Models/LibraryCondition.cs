namespace Movselex.Core.Models
{
    class LibraryCondition
    {
        public FilteringMode FilteringMode { get; private set; }

        public string ConditionSQL { get; private set; }

        public string FilteringText { get; private set; }

        public LibraryCondition(FilteringMode filteringMode, string conditionSql, string filteringText = null)
        {
            FilteringMode = filteringMode;
            ConditionSQL = conditionSql?? "";
            FilteringText = filteringText;
        }
    }
}

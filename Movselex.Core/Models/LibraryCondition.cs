namespace Movselex.Core.Models
{
    class LibraryCondition
    {
        public FilteringMode FilteringMode { get; private set; }

        public FilteringCondition Condition { get; private set; }

        public string FilteringText { get; private set; }

        public int MaxLimitNum { get; private set; }

        public LibraryCondition(FilteringMode filteringMode, FilteringCondition condition, int maxLimitNum = 0, string filteringText = null)
        {
            FilteringMode = filteringMode;
            Condition = condition?? FilteringCondition.Empty;
            MaxLimitNum = maxLimitNum;
            FilteringText = filteringText;
        }
    }
}

using Livet;

namespace Movselex.Core.Models
{
    /// <summary>
    /// コンディションリストの１レコードを表します。
    /// </summary>
    public class FilteringItem : NotificationObject
    {
        public string Value { get; private set; }

        public string DisplayValue { get; private set; }

        #region IsSelected変更通知プロパティ

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public FilteringItem(string value, string displayValue)
        {
            Value = value;
            DisplayValue = displayValue;
            IsSelected = false;
        }

        public override string ToString()
        {
            return DisplayValue;
        }
    }
}
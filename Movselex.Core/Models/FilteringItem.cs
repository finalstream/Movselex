using System;
using System.Collections.Generic;
using Livet;

namespace Movselex.Core.Models
{
    /// <summary>
    /// コンディションリストの１レコードを表します。
    /// </summary>
    public class FilteringItem : NotificationObject, IEquatable<FilteringItem>
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

        public bool Equals(FilteringItem other)
        {
            if (other == null) return false;
            return DisplayValue == other.DisplayValue;
        }

        public override string ToString()
        {
            return DisplayValue;
        }
    }

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
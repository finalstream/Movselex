using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace FinalstreamUIComponents.Models
{
    public class InputParam : NotificationObject
    {
        private bool _isModify;

        public bool IsModify { get { return _isModify; }}

        #region Key変更通知プロパティ

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Value変更通知プロパティ

        private object _value;

        public object Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = value;
                _isModify = true;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Values変更通知プロパティ

        private IEnumerable<string> _values;

        public IEnumerable<string> Values
        {
            get { return _values; }
            set
            {
                if (_values == value) return;
                _values = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region IsVisibleCombobox変更通知プロパティ

        private bool _isVisibleCombobox;

        public bool IsVisibleCombobox
        {
            get { return _isVisibleCombobox; }
            set
            {
                if (_isVisibleCombobox == value) return;
                _isVisibleCombobox = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public InputParam(string title, object value)
        {
            _title = title;
            _value = value;
            IsVisibleCombobox = false;
        }

        public InputParam(string title, object value, IEnumerable<string> values)
        {
            _title = title;
            _value = value;
            _values = values;
            IsVisibleCombobox = true;
        }
    }
}

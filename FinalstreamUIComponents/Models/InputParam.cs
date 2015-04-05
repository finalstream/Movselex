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

        #region Key変更通知プロパティ

        private string _key;

        public string Key
        {
            get { return _key; }
            set
            {
                if (_key == value) return;
                _key = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Value変更通知プロパティ

        private string _value;

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public InputParam(string key, string value)
        {
            _key = key;
            _value = value;
        }
    }
}

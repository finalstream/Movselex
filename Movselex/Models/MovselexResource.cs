using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FinalstreamUIComponents.Models;
using Movselex.Properties;

namespace Movselex.Models
{
    public class MovselexResource : INotifyPropertyChanged
    {
        #region singleton members

        private static readonly MovselexResource _current = new MovselexResource();
        public static MovselexResource Current
        {
            get { return _current; }
        }

        #endregion


        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// 指定されたカルチャ名を使用して、リソースのカルチャを変更します。
        /// </summary>
        /// <param name="name">カルチャの名前。</param>
        public void ChangeCulture(string name)
        {
            Resources.Culture = System.Globalization.CultureInfo.GetCultureInfo(name);
            this.RaisePropertyChanged("Resources");

        }

        private readonly Resources _resources = new Resources();


        public Resources Resources
        {
            get { return _resources; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace Movselex.Core.Models
{
    /// <summary>
    /// Movselexで扱うライブラリを表します。
    /// </summary>
    class MovselexLibrary : NotificationObject
    {
        public readonly ObservableSynchronizedCollection<FilteringItem> ConditionItems; 

        public MovselexLibrary()
        {
            ConditionItems = new ObservableSynchronizedCollection<FilteringItem>();
        }
        
    }
}

using System.Collections.ObjectModel;
using System.ComponentModel;
using FinalstreamCommons.Collections;
using Livet;

namespace Movselex.Core.Models
{
    public interface IMovselexFiltering
    {
        SelectableObservableCollection<FilteringItem> FilteringItems { get; }

        /// <summary>
        /// フィルタ条件をロードします。
        /// </summary>
        void Load(string language);
    }
}
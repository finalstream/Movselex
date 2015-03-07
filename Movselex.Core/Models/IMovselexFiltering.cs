using System.Collections.ObjectModel;
using System.ComponentModel;
using Livet;

namespace Movselex.Core.Models
{
    public interface IMovselexFiltering
    {
        ObservableCollection<FilteringItem> FilteringItems { get; }

        /// <summary>
        /// フィルタ条件をロードします。
        /// </summary>
        void Load();
    }
}
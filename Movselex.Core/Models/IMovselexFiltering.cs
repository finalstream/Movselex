using System.ComponentModel;
using Livet;

namespace Movselex.Core.Models
{
    public interface IMovselexFiltering
    {
        ObservableSynchronizedCollection<FilteringItem> FilteringItems { get; }
    }
}
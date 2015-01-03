using System.ComponentModel;
using Livet;

namespace Movselex.Core.Models
{
    public interface IMovselexFiltering
    {
        DispatcherCollection<FilteringItem> FilteringItems { get; }
    }
}
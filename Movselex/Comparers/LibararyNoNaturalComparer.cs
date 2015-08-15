using System.ComponentModel;
using FinalstreamCommons.Comparers;
using Movselex.ViewModels;

namespace Movselex.Comparers
{
    public class LibararyNoNaturalComparer : NaturalComparer
    {
        private ListSortDirection _direction;

        public LibararyNoNaturalComparer(ListSortDirection direction)
        {
            _direction = direction;
        }

        public override int Compare(object x, object y)
        {
            var xn = x as LibraryViewModel;
            var yn = y as LibraryViewModel;
            return _direction == ListSortDirection.Ascending ? 
                base.Compare(xn.Model.No ?? "", yn.Model.No ?? "")
                : base.Compare(yn.Model.No ?? "", xn.Model.No ?? "");
        }
    }
}
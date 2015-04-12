using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinalstreamUIComponents.Converters
{
    public sealed class BoolToVisibilityConverter : BoolConverter<Visibility>
    {
        public BoolToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed) { }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Movselex.Views.Converters
{
    class IsCompleteConverter : IValueConverter
    {
        private readonly BitmapImage _complete;
        private readonly BitmapImage _normal;

        public IsCompleteConverter()
        {
            _complete = new BitmapImage(new Uri("..\\..\\images\\complete-on.png", UriKind.Relative));
            _normal = new BitmapImage(new Uri("..\\..\\images\\complete-off.png", UriKind.Relative));
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            return val ? _complete : _normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

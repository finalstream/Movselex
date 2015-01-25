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
    class IsPlayedConverter : IValueConverter
    {
        private readonly BitmapImage _played;
        private readonly BitmapImage _normal;

        public IsPlayedConverter()
        {
            _played = new BitmapImage(new Uri("..\\..\\images\\played-on.png", UriKind.Relative));
            _normal = new BitmapImage(new Uri("..\\..\\images\\played-off.png", UriKind.Relative));
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            return val ? _played : _normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

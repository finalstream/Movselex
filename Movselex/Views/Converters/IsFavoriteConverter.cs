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
    class IsFavoriteConverter : IValueConverter
    {
        private readonly BitmapImage _favorite;
        private readonly BitmapImage _normal;

        public IsFavoriteConverter()
        {
            _favorite = new BitmapImage(new Uri("..\\..\\images\\star-favorite.png", UriKind.Relative));
            _normal = new BitmapImage(new Uri("..\\..\\images\\star-normal.png", UriKind.Relative));
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;
            return val ? _favorite : _normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

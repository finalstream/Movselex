using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FinalstreamUIComponents.Converters
{
    public class NullVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value == null ? Visibility.Collapsed : Visibility.Visible;
    }
 
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        Visibility? v = value as Visibility?;
        return ((v.HasValue) || (v.Value == Visibility.Collapsed)) ? null : "";
    }
}
}

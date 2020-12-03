using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Ks.Avalonia.Converters
{
    public class ModuloConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = System.Convert.ToInt32(value);
            var b = System.Convert.ToInt32(parameter);

            return a % b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

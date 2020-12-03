using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Ks.Avalonia.Converters
{
    public class ScaleInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(parameter) / System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToDouble(parameter) / System.Convert.ToDouble(value);
        }
    }
}

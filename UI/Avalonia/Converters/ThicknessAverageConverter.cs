using System;
using System.Globalization;

using Avalonia;
using Avalonia.Data.Converters;

namespace Ks.Avalonia.Converters
{
    public class ThicknessAverageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var th = (Thickness) value;
            return (th.Left + th.Top + th.Right + th.Bottom) / 4;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = System.Convert.ToDouble(value);
            return new Thickness(v);
        }
    }
}

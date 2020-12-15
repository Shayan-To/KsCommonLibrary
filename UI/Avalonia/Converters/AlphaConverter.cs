using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Ks.Avalonia.Converters
{
    public class AlphaConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = BrushConverter.ToColor(value);
            if (!color.HasValue)
            {
                return null;
            }
            var c = color.Value;

            var p = (byte) 255;
            if (parameter != null)
            {
                p = System.Convert.ToByte(parameter);
            }

            c = Color.FromArgb(p, c.R, c.G, c.B);
            return BrushConverter.ToColorBrush(c, targetType);
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Convert(value, targetType, parameter, culture);
        }
    }
}

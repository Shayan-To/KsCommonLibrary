using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

using Ks.Common;

namespace Ks.Avalonia.Converters
{
    public class ContrastingColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = BrushConverter.ToColor(value) ?? new Color();
            var rgb = (color.R / 255.0, color.G / 255.0, color.B / 255.0);
            var res = color.A / 255.0 * (1 - Utilities.Color.GetLuminance(rgb)) > 0.5 ? Colors.White : Colors.Black;
            return BrushConverter.ToColorBrush(res, targetType);
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw Verify.Fail();
        }
    }
}

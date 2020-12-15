using System;
using System.Globalization;

using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

using Ks.Common;

namespace Ks.Avalonia.Converters
{
    public class ColorHexConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            Verify.True(targetType == typeof(string));
            var ncolor = BrushConverter.ToColor(value);
            if (!ncolor.HasValue)
            {
                return null;
            }
            var color = ncolor.Value;
            var argb = (color.A, color.R, color.G, color.B);
            return Utilities.Serialization.ColorToHex(argb);
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            var hex = (string?) value;
            if (string.IsNullOrWhiteSpace(hex))
            {
                return null;
            }
            try
            {
                var argb = Utilities.Serialization.HexToColor(hex);
                return BrushConverter.ToColorBrush(new Color(argb.A, argb.R, argb.G, argb.B), targetType);
            }
            catch (Exception ex)
            {
                return new BindingNotification(ex, BindingErrorType.DataValidationError);
            }
        }
    }
}

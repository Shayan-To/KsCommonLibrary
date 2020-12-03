using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

using Ks.Avalonia.Utils;
using Ks.Common;

namespace Ks.Avalonia.Converters
{
    public class BrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = ToColor(value);
            return ToColorBrush(color, targetType);
        }

        public static object? ToColorBrush(Color? color, Type targetType)
        {
            if (!color.HasValue)
            {
                return null;
            }
            return targetType switch
            {
                var t when t == typeof(Color) || t == typeof(Color?) => color,
                var t when t == typeof(Brush) || t == typeof(IBrush) || t == typeof(SolidColorBrush) => BrushCache.GetBrush(color.Value),
                _ => throw Verify.FailArg(nameof(targetType), "Target type not supported."),
            };
        }

        public static Color? ToColor(object? value)
        {
            return value switch
            {
                null => null,
                Color c => c,
                SolidColorBrush b => b.Color,
                _ => throw Verify.FailArg(nameof(value), "Only Color and SolidColorBrush are supported"),
            };
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Convert(value, targetType, parameter, culture);
        }
    }
}

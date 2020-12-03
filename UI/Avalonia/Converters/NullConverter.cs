using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Ks.Avalonia.Converters
{
    public class NullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null ? NullObject.Value : value;
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is NullObject ? null : value;
        }
    }

    public class NullObject
    {
        public static NullObject Value { get; } = new NullObject();
    }
}

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;

using Avalonia.Data.Converters;

namespace Ks.Avalonia.Converters
{
    public class EmptyToNullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IEnumerable e)
            {
                return value;
            }
            foreach (var _ in e)
            {
                return value;
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

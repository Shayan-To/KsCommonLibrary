using System;
using System.Globalization;

using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;

using Ks.Common;

namespace Ks.Avalonia.Converters
{
    public class BooleanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            var res = value switch
            {
                null => false,
                bool b => b,
                int i => i != 0,
                string s => s.Length != 0,
                var o => !(o.Equals(0) || o.Equals(false) || o.Equals("") || o.Equals(null))
            };
            if (parameter is string p && p == "!")
            {
                res = !res;
            }
            return res;
        }

        public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            throw Assert.Fail();
        }
    }
}

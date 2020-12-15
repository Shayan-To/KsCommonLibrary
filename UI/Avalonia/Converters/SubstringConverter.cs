using System;
using System.Data;
using System.Globalization;
using System.Linq;

using Avalonia.Data.Converters;

using Ks.Common;

namespace Ks.Avalonia.Converters
{
    public class SubstringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = ((string) parameter).Split(',').Select(s => int.Parse(s.Trim())).ToArray();
            Verify.True(p.Length == 2);
            return System.Convert.ToString(value)!.Substring(p[0], p[1]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

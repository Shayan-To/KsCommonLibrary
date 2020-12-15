using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Ks.Avalonia.Converters
{
    public class NumberPaddingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(culture, "{0}", value).PadLeft(System.Convert.ToInt32(parameter), '0');
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = string.Format(culture, "{0}", value);
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] != '0')
                {
                    return s.Substring(i);
                }
            }
            return "0";
        }
    }
}

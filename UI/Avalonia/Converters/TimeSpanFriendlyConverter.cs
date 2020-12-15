using System;
using System.Globalization;

using Avalonia.Data.Converters;

using Ks.Common;

namespace Ks.Avalonia.Converters
{
    public class TimeSpanFriendlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Utilities.Representation.GetFriendlyTimeSpan((TimeSpan) value, TimeSpan.FromMinutes(System.Convert.ToDouble(parameter)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ToDo Implement the reverse conversion.
            throw new NotSupportedException();
        }
    }
}

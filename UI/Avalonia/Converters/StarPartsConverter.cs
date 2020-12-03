using System;
using System.Globalization;

using Avalonia.Controls;
using Avalonia.Data.Converters;

using Ks.Avalonia.Controls;
using Ks.Common;

namespace Ks.Avalonia.Converters
{
    public class StarPartsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Verify.TrueArg(targetType == typeof(GridLengths), nameof(targetType));
            var count = System.Convert.ToInt32(value);
            var res = new GridLengths();
            for (var i = 0; i < count; i++)
            {
                res.Add(new GridLength(1, GridUnitType.Star));
            }
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((GridLengths) value).Count;
        }
    }
}

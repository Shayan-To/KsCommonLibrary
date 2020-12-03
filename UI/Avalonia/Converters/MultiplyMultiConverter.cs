using System;
using System.Collections.Generic;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Ks.Avalonia.Converters
{
    public class MultiplyMultiConverter : IMultiValueConverter
    {
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            var r = parameter == null ? 1 : System.Convert.ToDouble(parameter);
            for (var i = 0; i < values.Count; i++)
            {
                r *= System.Convert.ToDouble(values[i]);
            }

            return r;
        }
    }
}

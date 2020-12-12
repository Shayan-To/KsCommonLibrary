using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks.Common.MVVM.Converters
{
    public class MultiplyMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var R = parameter == null ? 1 : System.Convert.ToDouble(parameter);
            for (var I = 0; I < values.Length; I++)
                R *= System.Convert.ToDouble(values[I]);
            return R;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks
{
    namespace Common.MVVM.Converters
    {
        public class MultiplyMultiConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                var R = parameter == null ? 1 : System.Convert.ToDouble(parameter);
                var loopTo = values.Length - 1;
                for (var I = 0; I <= loopTo; I++)
                    R *= System.Convert.ToDouble(values[I]);
                return R;
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException();
            }
        }
    }
}

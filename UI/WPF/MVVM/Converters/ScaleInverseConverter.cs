using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks.Common.MVVM.Converters
{
    public class ScaleInverseConverter : IValueConverter
    {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return System.Convert.ToDouble(Parameter) / System.Convert.ToDouble(Value);
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return System.Convert.ToDouble(Parameter) / System.Convert.ToDouble(Value);
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks.Common.MVVM.Converters
{
        public class ScaleConverter : IValueConverter
        {
            public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                return System.Convert.ToDouble(Value) * System.Convert.ToDouble(Parameter);
            }

            public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                return System.Convert.ToDouble(Value) / System.Convert.ToDouble(Parameter);
            }
        }
    }

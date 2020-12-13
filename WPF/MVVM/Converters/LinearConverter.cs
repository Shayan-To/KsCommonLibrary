using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks
{
    namespace Common.MVVM.Converters
    {
        public class LinearConverter : IValueConverter
        {
            public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                return (this.A * System.Convert.ToDouble(Value)) + this.B;
            }

            public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                return (System.Convert.ToDouble(Value) - this.B) / this.A;
            }

            public double A { get; set; } = 1;
            public double B { get; set; } = 0;
        }
    }
}

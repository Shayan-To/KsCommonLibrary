using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks
{
    namespace Common.MVVM.Converters
    {
        public class ModuloConverter : IValueConverter
        {
            public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                var A = System.Convert.ToInt32(Value);
                var B = System.Convert.ToInt32(Parameter);

                return A % B;
            }

            public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                return Value;
            }
        }
    }
}

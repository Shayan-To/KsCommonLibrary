using System.Windows;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks
{
    namespace Ks.Common.MVVM.Converters
    {
        public class BooleanToVisibilityConverter : IValueConverter
        {
            public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                if ((System.Convert.ToString(Parameter) == "~") ^ System.Convert.ToBoolean(Value))
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

            public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                return (System.Convert.ToString(Parameter) == "~") ^ ((byte)(Visibility)Value == (byte)Visibility.Visible);
            }
        }
    }
}

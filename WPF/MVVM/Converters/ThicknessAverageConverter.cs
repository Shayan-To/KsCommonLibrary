using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ks.Common.MVVM.Converters
{
    public class ThicknessAverageConverter : IValueConverter
    {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var Th = (Thickness) Value;
            return (Th.Left + Th.Top + Th.Right + Th.Bottom) / 4;
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var V = System.Convert.ToDouble(Value);
            return new Thickness(V);
        }
    }
}

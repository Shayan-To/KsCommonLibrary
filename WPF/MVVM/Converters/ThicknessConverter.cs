using System.Windows;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks
{
    namespace Ks.Common.MVVM.Converters
    {
        public class ThicknessConverter : IValueConverter
        {
            public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                var V = System.Convert.ToDouble(Value);
                var P = 1.0;
                if (Parameter != null)
                    P = System.Convert.ToDouble(Parameter);
                return new Thickness(P * V * this.Coefficients.Left, P * V * this.Coefficients.Top, P * V * this.Coefficients.Right, P * V * this.Coefficients.Bottom);
            }

            public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                var Th = (Thickness)Value;
                double? V = null;

                // ToDo Correct the double equality checks. (It should somehow just check the significant digits, and not the order of magnitude...)

                if (this.Coefficients.Left != (double)0)
                    V = Th.Left / this.Coefficients.Left;
                if (this.Coefficients.Top != (double)0)
                {
                    if (V.HasValue & (V.Value != (Th.Top / this.Coefficients.Top)))
                        return 0;
                    V = Th.Top / this.Coefficients.Top;
                }
                if (this.Coefficients.Right != (double)0)
                {
                    if (V.HasValue & (V.Value != (Th.Right / this.Coefficients.Right)))
                        return 0;
                    V = Th.Right / this.Coefficients.Right;
                }
                if (this.Coefficients.Bottom != (double)0)
                {
                    if (V.HasValue & (V.Value != (Th.Bottom / this.Coefficients.Bottom)))
                        return 0;
                    V = Th.Bottom / this.Coefficients.Bottom;
                }
                if (!V.HasValue)
                    return 0;
                var P = 1.0;
                if (Parameter != null)
                    P = System.Convert.ToDouble(Parameter);
                return V.Value * P;
            }

            public Thickness Coefficients { get; set; }
        }
    }
}

using System;
using System.Globalization;

using Avalonia;
using Avalonia.Data.Converters;

namespace Ks.Avalonia.Converters
{
    public class ThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = System.Convert.ToDouble(value);
            var p = 1.0;
            if (parameter != null)
            {
                p = System.Convert.ToDouble(parameter);
            }

            return new Thickness(p * v * this.Coefficients.Left, p * v * this.Coefficients.Top, p * v * this.Coefficients.Right, p * v * this.Coefficients.Bottom);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var th = (Thickness) value;
            double? v = null;

            // ToDo Correct the double equality checks. (It should somehow just check the significant digits, and not the order of magnitude...)

            if (this.Coefficients.Left != 0)
            {
                v = th.Left / this.Coefficients.Left;
            }

            if (this.Coefficients.Top != 0)
            {
                if (v.HasValue && (v.Value != (th.Top / this.Coefficients.Top)))
                {
                    return 0;
                }

                v = th.Top / this.Coefficients.Top;
            }
            if (this.Coefficients.Right != 0)
            {
                if (v.HasValue && (v.Value != (th.Right / this.Coefficients.Right)))
                {
                    return 0;
                }

                v = th.Right / this.Coefficients.Right;
            }
            if (this.Coefficients.Bottom != 0)
            {
                if (v.HasValue && (v.Value != (th.Bottom / this.Coefficients.Bottom)))
                {
                    return 0;
                }

                v = th.Bottom / this.Coefficients.Bottom;
            }
            if (!v.HasValue)
            {
                return 0;
            }

            var p = 1.0;
            if (parameter != null)
            {
                p = System.Convert.ToDouble(parameter);
            }

            return v.Value / p;
        }

        public Thickness Coefficients { get; set; }
    }
}

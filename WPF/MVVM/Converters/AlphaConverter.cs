using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Ks.Common.MVVM.Converters
{
    public class AlphaConverter : IValueConverter
    {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var B = Value as SolidColorBrush;
            Color C;
            if (B != null)
            {
                C = B.Color;
            }
            else
            {
                C = (Color) Value;
            }

            var P = (byte) 255;
            if (Parameter != null)
            {
                P = System.Convert.ToByte(Parameter);
            }

            C = Color.FromArgb(P, C.R, C.G, C.B);
            if (TargetType == typeof(Color))
            {
                return C;
            }

            return new SolidColorBrush(C);
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var B = Value as SolidColorBrush;
            Color C;
            if (B != null)
            {
                C = B.Color;
            }
            else
            {
                C = (Color) Value;
            }

            var P = (byte) 255;
            if (Parameter != null)
            {
                P = System.Convert.ToByte(Parameter);
            }

            C = Color.FromArgb(P, C.R, C.G, C.B);
            if (TargetType == typeof(Color))
            {
                return C;
            }

            return new SolidColorBrush(C);
        }
    }
}

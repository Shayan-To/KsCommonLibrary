using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Ks.Common.MVVM.Converters
{
    public class BrightnessConverter : DependencyObject, IValueConverter
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

            var P = 0.0;
            if (Parameter != null)
            {
                P = System.Convert.ToDouble(Parameter);
            }

            C = this.Convert(C, P);

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

            var P = 0.0;
            if (Parameter != null)
            {
                P = System.Convert.ToDouble(Parameter);
            }

            C = this.Convert(C, -P);

            if (TargetType == typeof(Color))
            {
                return C;
            }

            return new SolidColorBrush(C);
        }

        private static byte GetByte(int N)
        {
            if (N < 0)
            {
                N = 0;
            }

            if (N > 255)
            {
                N = 255;
            }

            return (byte) N;
        }

        public Color Convert(Color C, double Parameter = 0.0)
        {
            var P = System.Convert.ToInt32(Parameter * 255 * this.Coeff);
            return Color.FromArgb(C.A, GetByte(C.R + P), GetByte(C.G + P), GetByte(C.B + P));
        }

        private void CalculateCoeff()
        {
            var Background = this.Background;
            var Foreground = this.Foreground;
            var BG = (Background.ScR + Background.ScG + Background.ScB) / 3;
            var FG = (Foreground.ScR + Foreground.ScG + Foreground.ScB) / 3;

            this.Coeff = BG - FG;
        }

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Color), typeof(BrightnessConverter), new PropertyMetadata(Colors.White, Background_Changed));

        private static void Background_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (BrightnessConverter) D;
            Self.CalculateCoeff();
        }

        public Color Background
        {
            get
            {
                return (Color) this.GetValue(BackgroundProperty);
            }
            set
            {
                this.SetValue(BackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Color), typeof(BrightnessConverter), new PropertyMetadata(Colors.Black, Foreground_Changed));

        private static void Foreground_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (BrightnessConverter) D;
            Self.CalculateCoeff();
        }

        public Color Foreground
        {
            get
            {
                return (Color) this.GetValue(ForegroundProperty);
            }
            set
            {
                this.SetValue(ForegroundProperty, value);
            }
        }

        private double Coeff = 1;
    }
}

using System;
using System.Globalization;

using Avalonia.Data.Converters;

namespace Ks.Avalonia.Converters
{
    public class LinearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (this.A * System.Convert.ToDouble(value)) + this.B;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (System.Convert.ToDouble(value) - this.B) / this.A;
        }

        public double A { get; set; } = 1;
        public double B { get; set; } = 0;
    }
}

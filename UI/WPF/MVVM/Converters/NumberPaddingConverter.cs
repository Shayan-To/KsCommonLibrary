using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks.Common.MVVM.Converters
{
    public class NumberPaddingConverter : IValueConverter
    {
        public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            return string.Format(Culture, "{0}", Value).PadLeft(System.Convert.ToInt32(Parameter), '0');
        }

        public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
        {
            var S = string.Format(Culture, "{0}", Value);
            for (var I = 0; I < S.Length; I++)
            {
                if (S[I] != '0')
                {
                    return S.Substring(I);
                }
            }
            return "0";
        }
    }
}

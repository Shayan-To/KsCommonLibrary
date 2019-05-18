using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks
{
    namespace Common.MVVM.Converters
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
                var loopTo = S.Length - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    if (S[I] != '0')
                        return S.Substring(I);
                }
                return "0";
            }
        }
    }
}

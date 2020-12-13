using System.Data;
using System.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks
{
    namespace Common.MVVM.Converters
    {
        public class SubstringConverter : IValueConverter
        {
            public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                var P = ((string)Parameter).Split(',').Select(S => int.Parse(S.Trim())).ToArray();
                Verify.True(P.Length == 2);
                return System.Convert.ToString(Value).Substring(P[0], P[1]);
            }

            public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                throw new NotSupportedException();
            }
        }
    }
}

using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks.Common.MVVM.Converters
{
        public class TimeSpanFriendlyConverter : IValueConverter
        {
            public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                return Utilities.Representation.GetFriendlyTimeSpan((TimeSpan)Value, TimeSpan.FromMinutes(System.Convert.ToDouble(Parameter)));
            }

            public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                // ToDo Implement the reverse conversion.
                throw new NotSupportedException();
            }
        }
    }

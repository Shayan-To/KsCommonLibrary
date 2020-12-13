using System;
using System.Globalization;
using System.Windows.Data;

namespace Ks
{
    namespace Common.MVVM
    {
        public class TranslationConverter : IValueConverter
        {
            private KsLanguage GetLanguage()
            {
                return KsApplication.Current?.Language;
            }

            public object Convert(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                var Lang = this.GetLanguage();
                var Text = System.Convert.ToString(Value);
                return Lang?.GetTranslation(Text) ?? Text;
            }

            public object ConvertBack(object Value, Type TargetType, object Parameter, CultureInfo Culture)
            {
                throw new NotSupportedException();
            }
        }
    }
}

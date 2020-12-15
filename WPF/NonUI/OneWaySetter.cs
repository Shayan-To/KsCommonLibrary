using System.Threading.Tasks;
using System.Windows;

namespace Ks.Common.NonUI
{
    public class OneWaySetter : NonUIElement
    {
        public static readonly DependencyProperty InputProperty = DependencyProperty.Register("Input", typeof(object), typeof(OneWaySetter), new PropertyMetadata(null, Input_Changed, Input_Coerce));

        private static object Input_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, OneWaySetter)

            // Dim Value = DirectCast(BaseValue, Object)

            return BaseValue;
        }

        private static void Input_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (OneWaySetter) D;

            // Dim OldValue = DirectCast(E.OldValue, Object)
            var NewValue = (object) E.NewValue;

            Self.Output = NewValue;
        }

        public object Input
        {
            get => (object) this.GetValue(InputProperty);
            set => this.SetValue(InputProperty, value);
        }

        public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(object), typeof(OneWaySetter), new PropertyMetadata(null, Output_Changed, Output_Coerce));

        private static object Output_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, OneWaySetter)

            // Dim Value = DirectCast(BaseValue, Object)

            return BaseValue;
        }

        private static async void Output_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (OneWaySetter) D;

            // Dim OldValue = DirectCast(E.OldValue, Object)
            // Dim NewValue = DirectCast(E.NewValue, Object)

            await Task.Delay(1);

            Self.Output = Self.Input;
        }

        public object Output
        {
            get => (object) this.GetValue(OutputProperty);
            set => this.SetValue(OutputProperty, value);
        }
    }
}

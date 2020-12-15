// Imports System.Collections.Specialized

using System.Windows;
using System.Windows.Controls;

using Ks.Common.MVVM;

namespace Ks.Common.Controls
{
    public class NumericUpDown : Control
    {
        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        public NumericUpDown()
        {
            this.IncrementCommand = new DelegateCommand(this.OnIncrementCommand);
            this.DecrementCommand = new DelegateCommand(this.OnDecrementCommand);
        }

        private void OnIncrementCommand()
        {
            this.Value += this.Step;
        }

        public DelegateCommand IncrementCommand { get; }

        private void OnDecrementCommand()
        {
            this.Value -= this.Step;
        }

        public DelegateCommand DecrementCommand { get; }

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(1.0, Step_Changed, Step_Coerce));

        private static object Step_Coerce(DependencyObject D, object BaseValue)
        {
            // var Self = (NumericUpDown) D;

            // var Value = (decimal) BaseValue;

            return BaseValue;
        }

        private static void Step_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            // var Self = (NumericUpDown) D;

            // var OldValue = (decimal) E.OldValue;
            // var NewValue = (decimal) E.NewValue;
        }

        public decimal Step
        {
            get => (decimal) this.GetValue(StepProperty);
            set => this.SetValue(StepProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(0.0, Value_Changed, Value_Coerce));

        private static object Value_Coerce(DependencyObject D, object BaseValue)
        {
            // var Self = (NumericUpDown) D;

            // var Value = (decimal) BaseValue;

            return BaseValue;
        }

        private static void Value_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            // var Self = (NumericUpDown) D;

            // var OldValue = (decimal) E.OldValue;
            // var NewValue = (decimal) E.NewValue;
        }

        public decimal Value
        {
            get => (decimal) this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }
    }
}

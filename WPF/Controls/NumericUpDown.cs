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

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(NumericUpDown), new PropertyMetadata(1.0, Step_Changed, Step_Coerce));

        private static object Step_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = (NumericUpDown) D;

            var Value = (double) BaseValue;

            return BaseValue;
        }

        private static void Step_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (NumericUpDown) D;

            var OldValue = (double) E.OldValue;
            var NewValue = (double) E.NewValue;
        }

        public double Step
        {
            get => (double) this.GetValue(StepProperty);
            set => this.SetValue(StepProperty, value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new PropertyMetadata(0.0, Value_Changed, Value_Coerce));

        private static object Value_Coerce(DependencyObject D, object BaseValue)
        {
            var Self = (NumericUpDown) D;

            var Value = (double) BaseValue;

            return BaseValue;
        }

        private static void Value_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (NumericUpDown) D;

            var OldValue = (double) E.OldValue;
            var NewValue = (double) E.NewValue;
        }

        public double Value
        {
            get => (double) this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

#pragma warning disable CS0169 // The field is never used
        private bool IsDirty; // ToDo
#pragma warning restore CS0169 // The field is never used
    }
}

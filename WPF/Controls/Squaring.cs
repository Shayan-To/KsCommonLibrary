using System.Windows;
using System;

namespace Ks
{
    namespace Common.Controls
    {
        public class Squaring : ContentElement
        {
            static Squaring()
            {
                DefaultStyleKeyProperty.OverrideMetadata(typeof(Squaring), new FrameworkPropertyMetadata(typeof(Squaring)));
            }

            protected override Size MeasureOverride(Size Constraint)
            {
                var Dimension = Math.Min(Constraint.Width, Constraint.Height);
                var Size = new Size(Dimension, Dimension);

                var Content = this.Content;
                Content?.Measure(Size);

                if (this.FillAvailableSpace)
                {
                    if (double.IsPositiveInfinity(Dimension))
                        return new Size();
                    return Size;
                }

                Dimension = 0.0;
                if (Content != null)
                {
                    Size = Content.DesiredSize;
                    Dimension = Math.Max(Size.Width, Size.Height);
                }

                return new Size(Dimension, Dimension);
            }

            protected override Size ArrangeOverride(Size arrangeBounds)
            {
                var Content = this.Content;
                if (Content == null)
                    return arrangeBounds;

                Rect Rect;
                if (arrangeBounds.Height < arrangeBounds.Width)
                    Rect = new Rect((arrangeBounds.Width - arrangeBounds.Height) / 2, 0, arrangeBounds.Height, arrangeBounds.Height);
                else
                    Rect = new Rect(0, (arrangeBounds.Height - arrangeBounds.Width) / 2, arrangeBounds.Width, arrangeBounds.Width);

                Content.Arrange(Rect);

                return arrangeBounds;
            }

            public static readonly DependencyProperty FillAvailableSpaceProperty = DependencyProperty.Register("FillAvailableSpace", typeof(bool), typeof(Squaring), new PropertyMetadata(true, FillAvailableSpace_Changed, FillAvailableSpace_Coerce));

            private static object FillAvailableSpace_Coerce(DependencyObject D, object BaseValue)
            {
                // Dim Self = DirectCast(D, Squaring)

                // Dim Value = DirectCast(BaseValue, Boolean)

                return BaseValue;
            }

            private static void FillAvailableSpace_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
            {
                var Self = (Squaring)D;

                // Dim OldValue = DirectCast(E.OldValue, Boolean)
                // Dim NewValue = DirectCast(E.NewValue, Boolean)

                Self.InvalidateMeasure();
            }

            public bool FillAvailableSpace
            {
                get
                {
                    return (bool)this.GetValue(FillAvailableSpaceProperty);
                }
                set
                {
                    this.SetValue(FillAvailableSpaceProperty, value);
                }
            }
        }
    }
}

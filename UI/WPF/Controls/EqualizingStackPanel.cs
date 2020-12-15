using System;
using System.Windows;
using System.Windows.Controls;

namespace Ks.Common.Controls
{
    public class EqualizingStackPanel : Panel
    {

        // As Orientation.Horizontal is 0 (which makes no changes to the Orientation when got XOred with it), then the default for writing the Measure and Arrange methods will be the horizontal orientation.

        protected override Size MeasureOverride(Size AvailableSize)
        {
            var ChildCount = this.Children.Count;

            if (ChildCount == 0)
            {
                return Size.Empty;
            }

            var MaxHeight = 0.0;
            var MaxWidth = 0.0;
            AvailableSize = this.CreateSize(this.Dimension(AvailableSize, Orientation.Horizontal) / ChildCount,
                                            this.Dimension(AvailableSize, Orientation.Vertical));

            foreach (UIElement C in this.Children)
            {
                C.Measure(AvailableSize);
                var Sz = C.DesiredSize;
                MaxWidth = Math.Max(MaxWidth, this.Dimension(Sz, Orientation.Horizontal));
                MaxHeight = Math.Max(MaxHeight, this.Dimension(Sz, Orientation.Vertical));
            }

            return this.CreateSize(MaxWidth * ChildCount, MaxHeight);
        }

        protected override Size ArrangeOverride(Size FinalSize)
        {
            var ChildCount = this.Children.Count;
            var OrigFinalSize = FinalSize;

            var Width = this.Dimension(FinalSize, Orientation.Horizontal) / ChildCount;
            FinalSize = this.CreateSize(Width, this.Dimension(FinalSize, Orientation.Vertical));
            var X = 0.0;

            foreach (UIElement C in this.Children)
            {
                C.Arrange(new Rect(this.CreatePoint(X, 0), FinalSize));
                X += Width;
            }

            return OrigFinalSize;
        }

        private Size CreateSize(double Width, double Height)
        {
            if (this.OrientationCache == Orientation.Horizontal)
            {
                return new Size(Width, Height);
            }
            else
            {
                return new Size(Height, Width);
            }
        }

        private Point CreatePoint(double X, double Y)
        {
            if (this.OrientationCache == Orientation.Horizontal)
            {
                return new Point(X, Y);
            }
            else
            {
                return new Point(Y, X);
            }
        }

        private double Dimension(Size Size, Orientation Orientation)
        {
            Orientation ^= this.OrientationCache;
            if (Orientation == Orientation.Horizontal)
            {
                return Size.Width;
            }
            else
            {
                return Size.Height;
            }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(EqualizingStackPanel), new PropertyMetadata(Orientation.Vertical, Orientation_Changed, Orientation_Coerce));

        private static object Orientation_Coerce(DependencyObject D, object BaseValue)
        {
            // Dim Self = DirectCast(D, EqualizingStackPanel)

            // Dim Value = DirectCast(BaseValue, Orientation)

            return BaseValue;
        }

        private static void Orientation_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (EqualizingStackPanel) D;

            // Dim OldValue = DirectCast(E.OldValue, Orientation)
            var NewValue = (Orientation) E.NewValue;

            Self.OrientationCache = NewValue;

            Self.InvalidateMeasure();
        }

        public Orientation Orientation
        {
            get => (Orientation) this.GetValue(OrientationProperty);
            set => this.SetValue(OrientationProperty, value);
        }

        private Orientation OrientationCache = Orientation.Vertical;
    }
}

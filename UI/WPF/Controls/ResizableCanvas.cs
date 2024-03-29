using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Ks.Common;

using KsG = Ks.Common.Geometry;

namespace Ks.Common.Controls
{
    public class ResizableCanvas : Panel
    {
        private KsG.Rect? GetViewingArea(KsG.Size Size)
        {
            var ViewingRectangle = this.ViewingRectangle;
            // ViewingRectangle.Intersect(ShapeRectangle)

            if (this.KeepAspectRatio)
            {
                var RB = ViewingRectangle.Ks().GetSmallestBoundOf(this.GetClientArea(Size).Size);
                if (!RB.Item2.HasValue)
                {
                    Console.WriteLine("Error. " + Utilities.Debug.CompactStackTrace(5));
                    return default;
                }
                ViewingRectangle = RB.Item1.WPF();
            }

            return ViewingRectangle.Ks();
        }

        private KsG.Rect GetClientArea(KsG.Size Size)
        {
            var Padding = this.Padding;
            var Padding1 = new KsG.Point(Padding.Left, Padding.Top);
            var Padding2 = new KsG.Point(Padding.Right, Padding.Bottom);

            return new KsG.Rect(Padding1, (-Padding2.ToVector() + Size.ToVector()).ToPoint());
        }

        protected override Size MeasureOverride(Size AvailableSize)
        {
            var InfSize = new KsG.Size(double.PositiveInfinity, double.PositiveInfinity);

            var Client = this.GetClientArea(AvailableSize.Ks());

            var ViewingRectangleQ = this.GetViewingArea(AvailableSize.Ks());
            if (!ViewingRectangleQ.HasValue)
            {
                return AvailableSize;
            }

            var ViewingRectangle = ViewingRectangleQ.Value;

            var Sz0 = new KsG.Point();

            Sz0 = ViewingRectangle.ToLocal01(Sz0);
            Sz0 = Client.FromLocal01(Sz0);

            foreach (UIElement C in this.Children)
            {
                var Sz = new KsG.Vector(GetW(C), GetH(C));

                if (Sz == new KsG.Vector())
                {
                    C.Measure(InfSize.WPF());
                }
                else
                {
                    var Szz = Sz.ToPoint();

                    Szz = ViewingRectangle.ToLocal01(Szz);
                    Szz = Client.FromLocal01(Szz);

                    var Size = (Szz - Sz0).ToSize();
                    if (double.IsNaN(Size.Width) | double.IsNaN(Size.Height))
                    {
                        Size = new KsG.Size();
                    }

                    C.Measure(Size.WPF());
                }
            }

            return new Size();
        }

        protected override Size ArrangeOverride(Size FinalSize)
        {
            var Client = this.GetClientArea(FinalSize.Ks());

            var ViewingRectangleQ = this.GetViewingArea(FinalSize.Ks());
            if (!ViewingRectangleQ.HasValue)
            {
                return FinalSize;
            }

            var ViewingRectangle = ViewingRectangleQ.Value;

            foreach (UIElement C in this.Children)
            {
                var Pt = new KsG.Point(GetX(C), GetY(C));
                var Sz = new KsG.Vector(GetW(C), GetH(C));

                var Pt1 = Pt;
                Pt1 = ViewingRectangle.ToLocal01(Pt1);
                Pt1 = Client.FromLocal01(Pt1);

                if (Sz == new KsG.Vector())
                {
                    C.Arrange(new Rect(Pt1.WPF(), C.DesiredSize));
                }
                else
                {
                    var Pt2 = Pt + Sz;
                    Pt2 = ViewingRectangle.ToLocal01(Pt2);
                    Pt2 = Client.FromLocal01(Pt2);
                    C.Arrange(new KsG.Rect(Pt1, Pt2).WPF());
                }
            }

            return FinalSize;
        }

        // ToDo Support Padding and MaxViewingRectangle.

        public static readonly DependencyProperty ViewingRectangleProperty = DependencyProperty.Register("ViewingRectangle", typeof(Rect), typeof(ResizableCanvas), new PropertyMetadata(new Rect(0, 0, 1, 1), ViewingRectangle_Changed));

        private static void ViewingRectangle_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (ResizableCanvas) D;
            Self.InvalidateMeasure();
        }

        public Rect ViewingRectangle
        {
            get => (Rect) this.GetValue(ViewingRectangleProperty);
            set => this.SetValue(ViewingRectangleProperty, value);
        }

        // #Region "MaxViewingRectangle Property"
        // Public Shared ReadOnly MaxViewingRectangleProperty As DependencyProperty = DependencyProperty.Register("MaxViewingRectangle", GetType(Rect?), GetType(ResizableCanvas), New PropertyMetadata(New Rect?(), AddressOf MaxViewingRectangle_Changed))

        // Private Shared Sub MaxViewingRectangle_Changed(ByVal D As DependencyObject, ByVal E As DependencyPropertyChangedEventArgs)
        // Dim Self = DirectCast(D, ResizableCanvas)

        // Dim OldValue = DirectCast(E.OldValue, Rect?)
        // Dim NewValue = DirectCast(E.NewValue, Rect?)
        // End Sub

        // Public Property MaxViewingRectangle As Rect?
        // Get
        // Return DirectCast(Me.GetValue(MaxViewingRectangleProperty), Rect?)
        // End Get
        // Set(ByVal value As Rect?)
        // Me.SetValue(MaxViewingRectangleProperty, value)
        // End Set
        // End Property
        // #End Region

        public static readonly DependencyProperty KeepAspectRatioProperty = DependencyProperty.Register("KeepAspectRatio", typeof(bool), typeof(ResizableCanvas), new PropertyMetadata(true, KeepAspectRatio_Changed));

        private static void KeepAspectRatio_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (ResizableCanvas) D;
            Self.InvalidateMeasure();
        }

        public bool KeepAspectRatio
        {
            get => (bool) this.GetValue(KeepAspectRatioProperty);
            set => this.SetValue(KeepAspectRatioProperty, value);
        }

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(ResizableCanvas), new PropertyMetadata(new Thickness(), Padding_Changed));

        private static void Padding_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (ResizableCanvas) D;
            Self.InvalidateMeasure();
        }

        public Thickness Padding
        {
            get => (Thickness) this.GetValue(PaddingProperty);
            set => this.SetValue(PaddingProperty, value);
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.RegisterAttached("Y", typeof(double), typeof(ResizableCanvas), new PropertyMetadata(0.0, Y_Changed));

        private static void Y_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            if (VisualTreeHelper.GetParent(D) is ResizableCanvas C)
            {
                C.InvalidateMeasure();
            }
        }

        public static double GetY(UIElement O)
        {
            return (double) O.GetValue(YProperty);
        }

        public static void SetY(UIElement O, double Value)
        {
            O.SetValue(YProperty, Value);
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.RegisterAttached("X", typeof(double), typeof(ResizableCanvas), new PropertyMetadata(0.0, X_Changed));

        private static void X_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            if (VisualTreeHelper.GetParent(D) is ResizableCanvas C)
            {
                C.InvalidateMeasure();
            }
        }

        public static double GetX(UIElement O)
        {
            return (double) O.GetValue(XProperty);
        }

        public static void SetX(UIElement O, double Value)
        {
            O.SetValue(XProperty, Value);
        }

        public static readonly DependencyProperty HProperty = DependencyProperty.RegisterAttached("H", typeof(double), typeof(ResizableCanvas), new PropertyMetadata(0.0, H_Changed));

        private static void H_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            if (VisualTreeHelper.GetParent(D) is ResizableCanvas C)
            {
                C.InvalidateMeasure();
            }
        }

        public static double GetH(UIElement O)
        {
            return (double) O.GetValue(HProperty);
        }

        public static void SetH(UIElement O, double Value)
        {
            O.SetValue(HProperty, Value);
        }

        public static readonly DependencyProperty WProperty = DependencyProperty.RegisterAttached("W", typeof(double), typeof(ResizableCanvas), new PropertyMetadata(0.0, W_Changed));

        private static void W_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            if (VisualTreeHelper.GetParent(D) is ResizableCanvas C)
            {
                C.InvalidateMeasure();
            }
        }

        public static double GetW(UIElement O)
        {
            return (double) O.GetValue(WProperty);
        }

        public static void SetW(UIElement O, double Value)
        {
            O.SetValue(WProperty, Value);
        }
    }
}

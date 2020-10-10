using System.Windows;
using System.Windows.Markup;

using Ks.Common.MVVM;

namespace Ks.Common.Controls
{
    [ContentProperty("Content")]
    public class PopupHolder : FrameworkElement // UIElement is not used as base type throughout the whole libraries! Using FrameworkElement instead.
    {

        public PopupHolder()
        {
            this.ShowPopupCommand = new DelegateCommand(this.ShowPopup);
            this.HidePopupCommand = new DelegateCommand(this.HidePopup);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return default;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return default;
        }

        protected virtual Rect? ArrangePopup(PopupPanel Panel, Size PanelSize, Size PopupSize)
        {
            var Target = this.GetTarget();

            if (Target == null)
            {
                return default;
            }

            var Transform = Target.TransformToVisual(Panel);
            var TargetSize = new Size(Target.ActualWidth, Target.ActualHeight);
            var BasePoint = Transform.Transform(new Point(TargetSize.Width * this.TargetHorizontalAnchor, TargetSize.Height * this.TargetVerticalAnchor));

            BasePoint -= new Vector(PopupSize.Width * this.PopupHorizontalAnchor, PopupSize.Height * this.PopupVerticalAnchor);

            return new Rect(BasePoint, PopupSize);
        }

        public void ShowPopup()
        {
            this.IsPopupShown = true;
        }

        public DelegateCommand ShowPopupCommand { get; }

        public void HidePopup()
        {
            this.IsPopupShown = false;
        }

        public DelegateCommand HidePopupCommand { get; }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(PopupHolder), new PropertyMetadata(null, Content_Changed));

        private static void Content_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PopupHolder) D;

            // Dim OldValue = DirectCast(E.OldValue, UIElement)
            var NewValue = (UIElement) E.NewValue;
            var Popup = NewValue as Popup;

            if (Popup != null)
            {
                Self.Popup = Popup;
            }
            else if (Self.Popup != null)
            {
                Self.Popup.Content = NewValue;
            }
        }

        public UIElement Content
        {
            get => (UIElement) this.GetValue(ContentProperty);
            set => this.SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(FrameworkElement), typeof(PopupHolder), new PropertyMetadata(null, Target_Changed));

        private static void Target_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PopupHolder) D;
        }

        public FrameworkElement Target
        {
            get => (FrameworkElement) this.GetValue(TargetProperty);
            set => this.SetValue(TargetProperty, value);
        }

        private FrameworkElement GetTarget()
        {
            return this.Target ?? (System.Windows.Media.VisualTreeHelper.GetParent(this) as FrameworkElement);
        }

        private static readonly DependencyPropertyKey PopupPropertyKey = DependencyProperty.RegisterReadOnly("Popup", typeof(Popup), typeof(PopupHolder), new PropertyMetadata(null, Popup_Changed));

        private static void Popup_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PopupHolder) D;

            var OldValue = (Popup) E.OldValue;
            var NewValue = (Popup) E.NewValue;

            if (OldValue != null)
            {
                OldValue.ArrangeCallBack = null;
            }

            if (NewValue != null)
            {
                NewValue.ArrangeCallBack = Self.ArrangePopup;
            }
        }

        public static readonly DependencyProperty PopupProperty = PopupPropertyKey.DependencyProperty;

        public Popup Popup
        {
            get => (Popup) this.GetValue(PopupProperty);
            private set => this.SetValue(PopupPropertyKey, value);
        }

        public static readonly DependencyProperty IsPopupShownProperty = DependencyProperty.Register("IsPopupShown", typeof(bool), typeof(PopupHolder), new PropertyMetadata(false, IsPopupShown_Changed));

        private static void IsPopupShown_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PopupHolder) D;

            // Dim OldValue = DirectCast(E.OldValue, Boolean)
            var NewValue = (bool) E.NewValue;

            var Popup = Self.Popup;
            if (Popup == null & NewValue)
            {
                Popup = new Popup() { Content = Self.Content };
                Self.Popup = Popup;
            }

            if (Popup != null)
            {
                // If NewValue Then
                // Self.OnBeforeShowPopup()
                // End If
                Popup.IsShown = NewValue;
            }
        }

        public bool IsPopupShown
        {
            get => (bool) this.GetValue(IsPopupShownProperty);
            set => this.SetValue(IsPopupShownProperty, value);
        }

        public static readonly DependencyProperty TargetHorizontalAnchorProperty = DependencyProperty.Register("TargetHorizontalAnchor", typeof(double), typeof(PopupHolder), new PropertyMetadata(0.5, TargetHorizontalAnchor_Changed));

        private static void TargetHorizontalAnchor_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PopupHolder) D;

            var OldValue = (double) E.OldValue;
            var NewValue = (double) E.NewValue;
        }

        public double TargetHorizontalAnchor
        {
            get => (double) this.GetValue(TargetHorizontalAnchorProperty);
            set => this.SetValue(TargetHorizontalAnchorProperty, value);
        }

        public static readonly DependencyProperty TargetVerticalAnchorProperty = DependencyProperty.Register("TargetVerticalAnchor", typeof(double), typeof(PopupHolder), new PropertyMetadata(0.5, TargetVerticalAnchor_Changed));

        private static void TargetVerticalAnchor_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PopupHolder) D;

            var OldValue = (double) E.OldValue;
            var NewValue = (double) E.NewValue;
        }

        public double TargetVerticalAnchor
        {
            get => (double) this.GetValue(TargetVerticalAnchorProperty);
            set => this.SetValue(TargetVerticalAnchorProperty, value);
        }

        public static readonly DependencyProperty PopupHorizontalAnchorProperty = DependencyProperty.Register("PopupHorizontalAnchor", typeof(double), typeof(PopupHolder), new PropertyMetadata(0.5, PopupHorizontalAnchor_Changed));

        private static void PopupHorizontalAnchor_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PopupHolder) D;

            var OldValue = (double) E.OldValue;
            var NewValue = (double) E.NewValue;
        }

        public double PopupHorizontalAnchor
        {
            get => (double) this.GetValue(PopupHorizontalAnchorProperty);
            set => this.SetValue(PopupHorizontalAnchorProperty, value);
        }

        public static readonly DependencyProperty PopupVerticalAnchorProperty = DependencyProperty.Register("PopupVerticalAnchor", typeof(double), typeof(PopupHolder), new PropertyMetadata(0.5, PopupVerticalAnchor_Changed));

        private static void PopupVerticalAnchor_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (PopupHolder) D;

            var OldValue = (double) E.OldValue;
            var NewValue = (double) E.NewValue;
        }

        public double PopupVerticalAnchor
        {
            get => (double) this.GetValue(PopupVerticalAnchorProperty);
            set => this.SetValue(PopupVerticalAnchorProperty, value);
        }
    }
}

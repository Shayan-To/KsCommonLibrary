using System;
using System.Windows;

using Ks.Common.MVVM;

namespace Ks.Common.Controls
{
    public class Popup : ContentElement
    {
        static Popup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Popup), new FrameworkPropertyMetadata(typeof(Popup)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(Popup), new FrameworkPropertyMetadata(HorizontalAlignment.Center));
            VerticalAlignmentProperty.OverrideMetadata(typeof(Popup), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        public Popup()
        {
            this.ShowCommand = new DelegateCommand(this.Show);
            this.HideCommand = new DelegateCommand(this.Hide);
        }

        public static readonly RoutedEvent BeforeShowEvent = EventManager.RegisterRoutedEvent("BeforeShow", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Popup));

        protected virtual void OnBeforeShow()
        {
            var E = new RoutedEventArgs(BeforeShowEvent);
            this.RaiseEvent(E);
        }

        public event RoutedEventHandler BeforeShow
        {
            add
            {
                this.AddHandler(BeforeShowEvent, value);
            }
            remove
            {
                this.RemoveHandler(BeforeShowEvent, value);
            }
        }

        public static readonly RoutedEvent ShownEvent = EventManager.RegisterRoutedEvent("Shown", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Popup));

        protected virtual void OnShown()
        {
            var E = new RoutedEventArgs(ShownEvent);
            this.RaiseEvent(E);
        }

        public event RoutedEventHandler Shown
        {
            add
            {
                this.AddHandler(ShownEvent, value);
            }
            remove
            {
                this.RemoveHandler(ShownEvent, value);
            }
        }

        public static readonly RoutedEvent HidEvent = EventManager.RegisterRoutedEvent("Hid", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Popup));

        protected virtual void OnHid()
        {
            var E = new RoutedEventArgs(HidEvent);
            this.RaiseEvent(E);
        }

        public event RoutedEventHandler Hid
        {
            add
            {
                this.AddHandler(HidEvent, value);
            }
            remove
            {
                this.RemoveHandler(HidEvent, value);
            }
        }

        public void Show()
        {
            this.IsShown = true;
        }

        public DelegateCommand ShowCommand { get; }

        public void Hide()
        {
            this.IsShown = false;
        }

        public DelegateCommand HideCommand { get; }

        public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register("IsShown", typeof(bool), typeof(Popup), new PropertyMetadata(false, IsShown_Changed));

        private static void IsShown_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (Popup) D;

            if ((bool) E.NewValue)
            {
                Self.OnBeforeShow();
                Self.GetWindow()?.AddPopup(Self);
                Self.OnShown();
            }
            else
            {
                Self.GetWindow()?.RemovePopup(Self);
                Self.OnHid();
            }
        }

        public bool IsShown
        {
            get => (bool) this.GetValue(IsShownProperty);
            set => this.SetValue(IsShownProperty, value);
        }

        public static readonly DependencyProperty HasShelterProperty = DependencyProperty.Register("HasShelter", typeof(bool), typeof(Popup), new PropertyMetadata(true, HasShelter_Changed));

        private static void HasShelter_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (Popup) D;

            // Dim OldValue = DirectCast(E.OldValue, Boolean)
            // Dim NewValue = DirectCast(E.NewValue, Boolean)

            if (Self.IsShown)
            {
                Self.GetWindow()?.RefreshPopup(Self);
            }
        }

        public bool HasShelter
        {
            get => (bool) this.GetValue(HasShelterProperty);
            set => this.SetValue(HasShelterProperty, value);
        }

        public static readonly DependencyProperty DimShelterProperty = DependencyProperty.Register("DimShelter", typeof(bool), typeof(Popup), new PropertyMetadata(true, DimShelter_Changed));

        private static void DimShelter_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (Popup) D;

            // Dim OldValue = DirectCast(E.OldValue, Boolean)
            // Dim NewValue = DirectCast(E.NewValue, Boolean)

            if (Self.IsShown)
            {
                Self.GetWindow()?.UpdateDims();
            }
        }

        public bool DimShelter
        {
            get => (bool) this.GetValue(DimShelterProperty);
            set => this.SetValue(DimShelterProperty, value);
        }

        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register("Layer", typeof(int), typeof(Popup), new PropertyMetadata(0, Layer_Changed));

        private static void Layer_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (Popup) D;

            if (Self.IsShown)
            {
                var OldValue = (int) E.OldValue;
                // Dim NewValue = DirectCast(E.NewValue, Integer)

                var Window = Self.GetWindow();

                Window?.RemovePopup(Self, OldValue);
                Window?.AddPopup(Self);
            }
        }

        public int Layer
        {
            get => (int) this.GetValue(LayerProperty);
            set => this.SetValue(LayerProperty, value);
        }

        public static readonly DependencyProperty IsShelterSensitiveProperty = DependencyProperty.Register("IsShelterSensitive", typeof(bool), typeof(PopupShelter), new PropertyMetadata(true, IsShelterSensitive_Changed));

        private static void IsShelterSensitive_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            // var Self = (PopupShelter) D;

            // var OldValue = (bool) E.OldValue;
            // var NewValue = (bool) E.NewValue;
        }

        public bool IsShelterSensitive
        {
            get => (bool) this.GetValue(IsShelterSensitiveProperty);
            set => this.SetValue(IsShelterSensitiveProperty, value);
        }

        public static readonly DependencyProperty WindowProperty = DependencyProperty.Register("Window", typeof(Window), typeof(Popup), new PropertyMetadata(null, Window_Changed));

        private static void Window_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (Popup) D;

            var OldValue = GetWindow((Window) E.OldValue);
            var NewValue = GetWindow((Window) E.NewValue);

            if (Self.IsShown)
            {
                OldValue?.RemovePopup(Self);
                NewValue?.AddPopup(Self);
            }
        }

        public Window Window
        {
            get => (Window) this.GetValue(WindowProperty);
            set => this.SetValue(WindowProperty, value);
        }

        private static Window GetWindow(Window Window)
        {
            if (Window != null)
            {
                return Window;
            }

            return KsApplication.Current.Window.View as Window;
        }

        private Window GetWindow()
        {
            return GetWindow(this.Window);
        }

        internal Func<PopupPanel, Size, Size, Rect?> ArrangeCallBack;
    }
}

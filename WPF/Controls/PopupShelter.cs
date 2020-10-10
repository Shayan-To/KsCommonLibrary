using System.Windows;
using System.Windows.Controls;

namespace Ks.Common.Controls
{
    public class PopupShelter : Control
    {
        static PopupShelter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupShelter), new FrameworkPropertyMetadata(typeof(PopupShelter)));
        }

        private static readonly DependencyPropertyKey IsShelterShownPropertyKey = DependencyProperty.RegisterReadOnly("IsShelterShown", typeof(bool), typeof(PopupShelter), new PropertyMetadata(true));
        public static readonly DependencyProperty IsShelterShownProperty = IsShelterShownPropertyKey.DependencyProperty;

        public bool IsShelterShown
        {
            get => (bool) this.GetValue(IsShelterShownProperty);
            internal set => this.SetValue(IsShelterShownPropertyKey, value);
        }
    }
}

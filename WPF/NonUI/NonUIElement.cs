using System.Windows;

namespace Ks.Common.NonUI
{
    public abstract class NonUIElement : FrameworkElement
    {
        private static readonly object Collapsed = Visibility.Collapsed;

        static NonUIElement()
        {
            VisibilityProperty.OverrideMetadata(typeof(NonUIElement), new FrameworkPropertyMetadata(Collapsed, null, (D, B) => Collapsed));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size();
        }
    }
}

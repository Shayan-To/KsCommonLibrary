using System.Collections;
using System.Windows;
using System.Windows.Markup;

namespace Ks.Common.Controls
{
    [ContentProperty("Content")]
    public class ContentElement : FrameworkElement
    {
        static ContentElement()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentElement), new FrameworkPropertyMetadata(typeof(ContentElement)));
        }

        protected override Size MeasureOverride(Size AvailableSize)
        {
            var Content = this.Content;
            if (Content != null)
            {
                Content.Measure(AvailableSize);
                return Content.DesiredSize;
            }
            return new Size();
        }

        protected override Size ArrangeOverride(Size FinalSize)
        {
            this.Content?.Arrange(new Rect(FinalSize));
            return FinalSize;
        }

        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            Verify.TrueArg((index == 0) & this.Content != null, nameof(index));
            return this.Content;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return this.Content == null ? 0 : 1;
            }
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (this.Content != null)
                {
                    yield return this.Content;
                }
            }
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(Squaring), new PropertyMetadata(null, Content_Changed));

        public static void Content_Changed(DependencyObject D, DependencyPropertyChangedEventArgs E)
        {
            var Self = (ContentElement) D;

            var OldValue = (UIElement) E.OldValue;
            var NewValue = (UIElement) E.NewValue;

            if (OldValue != null)
            {
                Self.RemoveVisualChild(OldValue);
                Self.RemoveLogicalChild(OldValue);
            }
            if (NewValue != null)
            {
                Self.AddVisualChild(NewValue);
                Self.AddLogicalChild(NewValue);
            }

            Self.InvalidateArrange();
        }

        public UIElement Content
        {
            get
            {
                return (UIElement) this.GetValue(ContentProperty);
            }
            set
            {
                this.SetValue(ContentProperty, value);
            }
        }
    }
}

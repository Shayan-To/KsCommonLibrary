using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Metadata;

namespace Ks.Avalonia.Controls
{
    public class VerticalContainer : Control
    {

        public VerticalContainer()
        {
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.Content is null)
            {
                return new();
            }
            this.Content.Measure(Transpose(availableSize));
            return Transpose(this.Content.DesiredSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.Content is null)
            {
                return new();
            }
            this.Content.Arrange(new Rect(new(), Transpose(finalSize)));
            return Transpose(this.Content.Bounds.Size);
        }

        private static Size Transpose(Size s)
        {
            return new(s.Height, s.Width);
        }

        public static readonly DirectProperty<VerticalContainer, Control?> OrientationProperty =
            AvaloniaProperty.RegisterDirect<VerticalContainer, Control?>(nameof(Content), t => t.Content, (t, v) => t.Content = v);

        private Control? _Content;

        [Content]
        public Control? Content
        {
            get => this._Content;
            set
            {
                if (this._Content != value)
                {
                    this.VisualChildren.Clear();
                    this.LogicalChildren.Clear();
                    if (value is not null)
                    {
                        this.LogicalChildren.Add(value);
                        this.VisualChildren.Add(value);
                    }
                    this._Content = value;
                }
            }
        }
    }
}

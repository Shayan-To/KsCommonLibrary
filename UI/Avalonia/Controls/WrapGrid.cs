using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Metadata;

using Ks.Common;

namespace Ks.Avalonia.Controls
{
    public class WrapGrid : Control, IPanel
    {
        public WrapGrid()
        {
            this.VisualChildren.Add(this.Grid);
            this.Children.CollectionChanged += this.Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Taken from Panel.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.LogicalChildren.InsertRange(e.NewStartingIndex, e.NewItems.CastAsList<IControl>());
                    this.Grid.Children.InsertRange(e.NewStartingIndex, e.NewItems.CastAsList<IControl>());
                    //this.VisualChildren.InsertRange(e.NewStartingIndex, e.NewItems.CastAsList<IControl>());
                    break;

                case NotifyCollectionChangedAction.Move:
                    this.LogicalChildren.MoveRange(e.OldStartingIndex, e.OldItems.Count, e.NewStartingIndex);
                    this.Grid.Children.MoveRange(e.OldStartingIndex, e.OldItems.Count, e.NewStartingIndex);
                    //this.VisualChildren.MoveRange(e.OldStartingIndex, e.OldItems.Count, e.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    this.LogicalChildren.RemoveAll(e.OldItems.CastAsList<IControl>());
                    this.Grid.Children.RemoveAll(e.OldItems.CastAsList<IControl>());
                    //this.VisualChildren.RemoveAll(e.OldItems.CastAsList<IControl>());
                    break;

                case NotifyCollectionChangedAction.Replace:
                    for (var i = 0; i < e.OldItems.Count; ++i)
                    {
                        var index = i + e.OldStartingIndex;
                        var child = (IControl) e.NewItems[i]!;
                        this.LogicalChildren[index] = child;
                        this.Grid.Children[index] = child;
                        //this.VisualChildren[index] = child;
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    throw new NotSupportedException();
            }
            this.SetGridProps();
        }

        private void SetGridProps()
        {
            var dimension = this.Orientation switch
            {
                Orientation.Horizontal => this.ColumnDefinitions.Count,
                _ => this.RowDefinitions.Count
            };

            var x = 0;
            var y = 0;
            for (var i = 0; i < this.Children.Count; i+= 1)
            {
                var ch = (Control)this.Children[i];
                this.SetGridProp(ch, x, true);
                this.SetGridProp(ch, y, false);
                x += 1;
                if (x == dimension)
                {
                    x = 0;
                    y += 1;
                }
            }
        }

        private void SetGridProp(Control control, int value, bool x)
        {
            if (this.Orientation == Orientation.Vertical)
            {
                x = !x;
            }
            if (x)
            {
                Grid.SetColumn(control, value);
                Grid.SetColumnSpan(control, 1);
            }
            else
            {
                Grid.SetRow(control, value);
                Grid.SetRowSpan(control, 1);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.Grid.Measure(availableSize);
            return this.Grid.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Grid.Arrange(new(new(), finalSize));
            return finalSize;
        }

        public RowDefinitions RowDefinitions
        {
            get => this.Grid.RowDefinitions;
            set => this.Grid.RowDefinitions = value;
        }

        public ColumnDefinitions ColumnDefinitions
        {
            get => this.Grid.ColumnDefinitions;
            set => this.Grid.ColumnDefinitions = value;
        }

        public static readonly DirectProperty<WrapGridPanel, Orientation> OrientationProperty =
            AvaloniaProperty.RegisterDirect<WrapGridPanel, Orientation>(nameof(Orientation), t => t.Orientation, (t, v) => t.Orientation = v);

        private Orientation _Orientation = Orientation.Horizontal;

        public Orientation Orientation
        {
            get => this._Orientation;
            set
            {
                Verify.TrueArg(value == Orientation.Horizontal || value == Orientation.Vertical);
                this.SetAndRaise(OrientationProperty, ref this._Orientation, value);
                this.SetGridProps();
            }
        }

        [Content]
        public global::Avalonia.Controls.Controls Children { get; } = new();
        private Grid Grid { get; } = new();
    }
}

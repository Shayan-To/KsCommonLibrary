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
    public class WrapGridPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var d = new MeasureData(this, availableSize);
            this._AutoWidths = d.Defs.Select(d => 0.0).ToArray();
            d.Init();

            var maxWidthPerValue = 0.0;
            var maxHeight = 0.0;

            var x = 0;
            var y = 0;
            foreach (var ch in this.Children)
            {
                var def = d.Defs[x];
                ch.Measure(d.Orient(new Size(d.GetCellWidth(x, true), d.CellHeight)));
                var sz = d.Orient(ch.DesiredSize);

                switch (def.GridUnitType)
                {
                    case GridUnitType.Auto:
                        this._AutoWidths[x] = Math.Max(this._AutoWidths[x], sz.Width);
                        break;
                    case GridUnitType.Star:
                        maxWidthPerValue = Math.Max(maxWidthPerValue, def.Value == 0 ? 0 : sz.Width / def.Value);
                        break;
                }
                maxHeight = Math.Max(maxHeight, sz.Height);

                x += 1;
                if (x == d.XCount)
                {
                    x = 0;
                    y += 1;
                }
            }

            var infWidth = double.IsPositiveInfinity(d.Size.Width);
            var infHeight = double.IsPositiveInfinity(d.Size.Height);

            //return d.Orient(new Size(
            //    infWidth ? (maxWidthPerValue * d.StarValueSum) + d.AbsoluteValueSum + this._AutoWidths.Sum() + d.TotalSpacing.Width : d.Size.Width,
            //    infHeight ? (maxHeight * d.YCount) + d.TotalSpacing.Height : d.Size.Height));
            return d.Orient(new Size((maxWidthPerValue * d.StarValueSum) + d.AbsoluteValueSum + this._AutoWidths.Sum() + d.TotalSpacing.Width,
                                     (maxHeight * d.YCount) + d.TotalSpacing.Height));
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var d = new MeasureData(this, finalSize);
            d.Init();

            var px = 0.0;
            var py = 0.0;

            var x = 0;
            var y = 0;
            foreach (var ch in this.Children)
            {
                var def = d.Defs[x];
                var cellWidth = d.GetCellWidth(x, false);
                ch.Arrange(d.Orient(new Rect(px, py, cellWidth, d.CellHeight)));

                x += 1;
                px += cellWidth + d.Spacing.Width;
                if (x == d.XCount)
                {
                    x = 0;
                    y += 1;
                    px = 0;
                    py += d.CellHeight + d.Spacing.Height;
                }
            }

            return finalSize;
        }

        public static readonly DirectProperty<WrapGridPanel, GridLengths?> PartDefinitionsProperty =
            AvaloniaProperty.RegisterDirect<WrapGridPanel, GridLengths?>(nameof(PartDefinitions), t => t.PartDefinitions, (t, v) => t.PartDefinitions = v);

        private GridLengths? _PartDefinitions;
        public GridLengths? PartDefinitions
        {
            get => this._PartDefinitions;
            set
            {
                if (value?.Count == 0)
                {
                    value = null;
                }
                this.SetAndRaise(PartDefinitionsProperty, ref this._PartDefinitions, value);
                this.InvalidateMeasure();
            }
        }

        public static readonly DirectProperty<WrapGridPanel, Size> SpacingProperty =
            AvaloniaProperty.RegisterDirect<WrapGridPanel, Size>(nameof(Spacing), t => t.Spacing, (t, v) => t.Spacing = v);

        private Size _Spacing;
        public Size Spacing
        {
            get => this._Spacing;
            set
            {
                this.SetAndRaise(SpacingProperty, ref this._Spacing, value);
                this.InvalidateMeasure();
            }
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
                this.InvalidateMeasure();
            }
        }

        private double[] _AutoWidths = null!;

        private static readonly GridLengths _DefaultPartDefinitions = new() { new(1, GridUnitType.Star) };

        private class MeasureData
        {
            public MeasureData(WrapGridPanel panel, Size size)
            {
                this.Panel = panel;
                this.Orientation = panel.Orientation;
                this.Size = this.Orient(size);
                this.Spacing = this.Orient(panel.Spacing);
                this.Defs = this.Panel.PartDefinitions ?? _DefaultPartDefinitions;
            }

            public double GetCellWidth(int x, bool measure)
            {
                var def = this.Defs[x];
                return def.GridUnitType switch
                {
                    GridUnitType.Auto => measure ? this.RemainingWidth : this.Panel._AutoWidths[x],
                    GridUnitType.Pixel => def.Value,
                    GridUnitType.Star => def.Value == 0 ? 0 : this.CellWidthPerStar * def.Value,
                    _ => throw Verify.Fail("Invalid grid length unit type."),
                };
            }

            public void Init()
            {
                this.AutoWidthsSum = this.Panel._AutoWidths.Sum();
                this.AbsoluteValueSum = this.Defs.Where(d => d.IsAbsolute).Sum(d => d.Value);
                this.StarValueSum = this.Defs.Where(d => d.IsStar).Sum(d => d.Value);
                this.StarValueSum = this.StarValueSum == 0 ? 0 : this.StarValueSum;

                this.XCount = this.Defs.Count;
                this.YCount = Utilities.Math.CeilDiv(this.Panel.Children.Count, this.XCount);

                this.TotalSpacing = new((this.XCount - 1) * this.Spacing.Width, (this.YCount - 1) * this.Spacing.Height);

                this.RemainingWidth = this.Size.Width - this.TotalSpacing.Width - this.AbsoluteValueSum - this.AutoWidthsSum;

                this.CellWidthPerStar = this.RemainingWidth / this.StarValueSum;
                this.CellHeight = (this.Size.Height - this.TotalSpacing.Height) / this.YCount;
            }

            public Size Orient(Size size)
            {
                if (this.Orientation == Orientation.Horizontal)
                {
                    return size;
                }
                return new(size.Height, size.Width);
            }

            public Point Orient(Point point)
            {
                if (this.Orientation == Orientation.Horizontal)
                {
                    return point;
                }
                return new(point.Y, point.X);
            }

            public Rect Orient(Rect rect)
            {
                if (this.Orientation == Orientation.Horizontal)
                {
                    return rect;
                }
                return new(this.Orient(rect.Position), this.Orient(rect.Size));
            }

            public double RemainingWidth { get; private set; }
            public double CellWidthPerStar { get; private set; }
            public double CellHeight { get; private set; }
            public double StarValueSum { get; private set; }
            public double AbsoluteValueSum { get; private set; }
            public double AutoWidthsSum { get; private set; }
            public int XCount { get; private set; }
            public int YCount { get; private set; }
            public Size TotalSpacing { get; private set; }

            public WrapGridPanel Panel { get; }
            public Orientation Orientation { get; }
            public GridLengths Defs { get; }
            public Size Spacing { get; }
            public Size Size { get; }

        }
    }

    public class GridLengths : List<GridLength>
    {
        public GridLengths() : base() { }

        public GridLengths(IEnumerable<GridLength> collection) : base(collection) { }

        public GridLengths(int capacity) : base(capacity) { }

        public static GridLengths Parse(string s)
        {
            return new(GridLength.ParseLengths(s));
        }
    }
}

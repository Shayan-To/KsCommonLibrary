﻿using System.Windows;
using System.Windows.Controls;

namespace Ks
{
    namespace Common.Controls
    {
        public class SimplePanel : Panel
        {
            protected override Size MeasureOverride(Size AvailableSize)
            {
                var MaxWidth = 0.0;
                var MaxHeight = 0.0;

                foreach (UIElement C in this.Children)
                {
                    C.Measure(AvailableSize);
                    Size Sz = C.DesiredSize;

                    if (MaxHeight < Sz.Height)
                        MaxHeight = Sz.Height;
                    if (MaxWidth < Sz.Width)
                        MaxWidth = Sz.Width;
                }

                return new Size(MaxWidth, MaxHeight);
            }

            protected override Size ArrangeOverride(Size FinalSize)
            {
                foreach (UIElement C in this.Children)
                    C.Arrange(new Rect(FinalSize));

                return FinalSize;
            }
        }
    }
}

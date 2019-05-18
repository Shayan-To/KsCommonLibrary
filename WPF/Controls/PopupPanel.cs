﻿using System.Windows;
using System.Windows.Controls;

namespace Ks
{
    namespace Common.Controls
    {
        public class PopupPanel : Panel
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
                {
                    var Popup = C as Popup;
                    Rect? Rect = default(Rect?);

                    if (Popup != null)
                        Rect = Popup.ArrangeCallBack?.Invoke(this, FinalSize, Popup.DesiredSize);

                    if (!Rect.HasValue)
                        Rect = new Rect(FinalSize);

                    C.Arrange(Rect.Value);
                }

                return FinalSize;
            }
        }
    }
}

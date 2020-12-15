using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Metadata;

namespace Ks.Avalonia.Controls
{
    public class HorizontalScrollContentPresenter : ScrollContentPresenter
    {
        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            var origDelta = e.Delta;
            e.Delta = new(e.Delta.Y, e.Delta.X);
            base.OnPointerWheelChanged(e);
            e.Delta = origDelta;
        }
    }
}

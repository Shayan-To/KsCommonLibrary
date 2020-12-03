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
    public class ToggleButton : global::Avalonia.Controls.Primitives.ToggleButton
    {

        public static readonly DirectProperty<ToggleButton, bool> ToggleOnClickProperty =
            AvaloniaProperty.RegisterDirect<ToggleButton, bool>(nameof(ToggleOnClick), t => t.ToggleOnClick, (t, v) => t.ToggleOnClick = v, true);

        private bool _ToggleOnClick;

        public bool ToggleOnClick
        {
            get => this._ToggleOnClick;
            set => this.SetAndRaise(ToggleOnClickProperty, ref this._ToggleOnClick, value);
        }

        protected override void OnClick()
        {
            this._IsClicking = true;
            base.OnClick();
        }

        protected override void Toggle()
        {
            if (!this._IsClicking || this.ToggleOnClick)
            {
                base.Toggle();
            }
            this._IsClicking = false;
        }

        private bool _IsClicking;

    }
}

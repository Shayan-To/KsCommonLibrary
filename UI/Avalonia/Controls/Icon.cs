using System;
using System.Collections.Generic;
using System.Text;

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Metadata;

namespace Ks.Avalonia.Controls
{
    public class Icon : TemplatedControl
    {
        public static readonly DirectProperty<Icon, Geometry?> DataProperty =
            AvaloniaProperty.RegisterDirect<Icon, Geometry?>(nameof(Data), t => t.Data, (t, v) => t.Data = v);

        private Geometry? _Data;

        [Content]
        public Geometry? Data
        {
            get => this._Data;
            set => this.SetAndRaise(DataProperty, ref this._Data, value);
        }
    }
}

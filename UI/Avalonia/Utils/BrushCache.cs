using System;
using System.Collections.Generic;
using System.Text;

using Avalonia.Media;

namespace Ks.Avalonia.Utils
{
    public static class BrushCache
    {
        public static SolidColorBrush GetBrush(Color color)
        {
            if (!_BrushCache.TryGetValue(color, out var brush))
            {
                brush = new SolidColorBrush(color);
                _BrushCache[color] = brush;
            }

            return brush;
        }

        private static readonly Dictionary<Color, SolidColorBrush> _BrushCache = new();
    }
}

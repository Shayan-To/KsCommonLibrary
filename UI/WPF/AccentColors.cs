using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace Ks.Common
{
    public static class AccentColorsWpf
    {
        public static readonly Color Lime = AccentColors.Lime.WPF();
        public static readonly Color Green = AccentColors.Green.WPF();
        public static readonly Color Emerald = AccentColors.Emerald.WPF();
        public static readonly Color Teal = AccentColors.Teal.WPF();
        public static readonly Color Cyan = AccentColors.Cyan.WPF();
        public static readonly Color Cobalt = AccentColors.Cobalt.WPF();
        public static readonly Color Indigo = AccentColors.Indigo.WPF();
        public static readonly Color Violet = AccentColors.Violet.WPF();
        public static readonly Color Pink = AccentColors.Pink.WPF();
        public static readonly Color Magenta = AccentColors.Magenta.WPF();
        public static readonly Color Crimson = AccentColors.Crimson.WPF();
        public static readonly Color Red = AccentColors.Red.WPF();
        public static readonly Color Orange = AccentColors.Orange.WPF();
        public static readonly Color Amber = AccentColors.Amber.WPF();
        public static readonly Color Yellow = AccentColors.Yellow.WPF();
        public static readonly Color Brown = AccentColors.Brown.WPF();
        public static readonly Color Olive = AccentColors.Olive.WPF();
        public static readonly Color Steel = AccentColors.Steel.WPF();
        public static readonly Color Mauve = AccentColors.Mauve.WPF();
        public static readonly Color Taupe = AccentColors.Taupe.WPF();

        public static IEnumerable<Color> AllColors
        {
            get
            {
                return AccentColors.AllColors.Select(c => c.WPF());
            }
        }
    }
}

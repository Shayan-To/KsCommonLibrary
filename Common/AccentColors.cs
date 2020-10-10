using System.Collections.Generic;

namespace Ks.Common
{
        public static class AccentColors
        {

            public static readonly System.Windows.Media.Color Lime = Utilities.Serialization.HexToColor("#A4C400");
            public static readonly System.Windows.Media.Color Green = Utilities.Serialization.HexToColor("#60A917");
            public static readonly System.Windows.Media.Color Emerald = Utilities.Serialization.HexToColor("#008A00");
            public static readonly System.Windows.Media.Color Teal = Utilities.Serialization.HexToColor("#00ABA9");
            public static readonly System.Windows.Media.Color Cyan = Utilities.Serialization.HexToColor("#1BA1E2");
            public static readonly System.Windows.Media.Color Cobalt = Utilities.Serialization.HexToColor("#0050EF");
            public static readonly System.Windows.Media.Color Indigo = Utilities.Serialization.HexToColor("#6A00FF");
            public static readonly System.Windows.Media.Color Violet = Utilities.Serialization.HexToColor("#AA00FF");
            public static readonly System.Windows.Media.Color Pink = Utilities.Serialization.HexToColor("#F472D0");
            public static readonly System.Windows.Media.Color Magenta = Utilities.Serialization.HexToColor("#D80073");
            public static readonly System.Windows.Media.Color Crimson = Utilities.Serialization.HexToColor("#A20025");
            public static readonly System.Windows.Media.Color Red = Utilities.Serialization.HexToColor("#E51400");
            public static readonly System.Windows.Media.Color Orange = Utilities.Serialization.HexToColor("#FA6800");
            public static readonly System.Windows.Media.Color Amber = Utilities.Serialization.HexToColor("#F0A30A");
            public static readonly System.Windows.Media.Color Yellow = Utilities.Serialization.HexToColor("#E3C800");
            public static readonly System.Windows.Media.Color Brown = Utilities.Serialization.HexToColor("#825A2C");
            public static readonly System.Windows.Media.Color Olive = Utilities.Serialization.HexToColor("#6D8764");
            public static readonly System.Windows.Media.Color Steel = Utilities.Serialization.HexToColor("#647687");
            public static readonly System.Windows.Media.Color Mauve = Utilities.Serialization.HexToColor("#76608A");
            public static readonly System.Windows.Media.Color Taupe = Utilities.Serialization.HexToColor("#87794E");

            public static IEnumerable<System.Windows.Media.Color> Colors
            {
                get
                {
                    yield return Lime;
                    yield return Green;
                    yield return Emerald;
                    yield return Teal;
                    yield return Cyan;
                    yield return Cobalt;
                    yield return Indigo;
                    yield return Violet;
                    yield return Pink;
                    yield return Magenta;
                    yield return Crimson;
                    yield return Red;
                    yield return Orange;
                    yield return Amber;
                    yield return Yellow;
                    yield return Brown;
                    yield return Olive;
                    yield return Steel;
                    yield return Mauve;
                    yield return Taupe;
                }
            }
        }
    }

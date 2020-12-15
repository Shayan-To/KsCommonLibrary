using System.Collections.Generic;

namespace Ks.Common
{
    public static class AccentColors
    {
        public static readonly (byte A, byte R, byte G, byte B) Lime = Utilities.Serialization.HexToColor("#A4C400");
        public static readonly (byte A, byte R, byte G, byte B) Green = Utilities.Serialization.HexToColor("#60A917");
        public static readonly (byte A, byte R, byte G, byte B) Emerald = Utilities.Serialization.HexToColor("#008A00");
        public static readonly (byte A, byte R, byte G, byte B) Teal = Utilities.Serialization.HexToColor("#00ABA9");
        public static readonly (byte A, byte R, byte G, byte B) Cyan = Utilities.Serialization.HexToColor("#1BA1E2");
        public static readonly (byte A, byte R, byte G, byte B) Cobalt = Utilities.Serialization.HexToColor("#0050EF");
        public static readonly (byte A, byte R, byte G, byte B) Indigo = Utilities.Serialization.HexToColor("#6A00FF");
        public static readonly (byte A, byte R, byte G, byte B) Violet = Utilities.Serialization.HexToColor("#AA00FF");
        public static readonly (byte A, byte R, byte G, byte B) Pink = Utilities.Serialization.HexToColor("#F472D0");
        public static readonly (byte A, byte R, byte G, byte B) Magenta = Utilities.Serialization.HexToColor("#D80073");
        public static readonly (byte A, byte R, byte G, byte B) Crimson = Utilities.Serialization.HexToColor("#A20025");
        public static readonly (byte A, byte R, byte G, byte B) Red = Utilities.Serialization.HexToColor("#E51400");
        public static readonly (byte A, byte R, byte G, byte B) Orange = Utilities.Serialization.HexToColor("#FA6800");
        public static readonly (byte A, byte R, byte G, byte B) Amber = Utilities.Serialization.HexToColor("#F0A30A");
        public static readonly (byte A, byte R, byte G, byte B) Yellow = Utilities.Serialization.HexToColor("#E3C800");
        public static readonly (byte A, byte R, byte G, byte B) Brown = Utilities.Serialization.HexToColor("#825A2C");
        public static readonly (byte A, byte R, byte G, byte B) Olive = Utilities.Serialization.HexToColor("#6D8764");
        public static readonly (byte A, byte R, byte G, byte B) Steel = Utilities.Serialization.HexToColor("#647687");
        public static readonly (byte A, byte R, byte G, byte B) Mauve = Utilities.Serialization.HexToColor("#76608A");
        public static readonly (byte A, byte R, byte G, byte B) Taupe = Utilities.Serialization.HexToColor("#87794E");

        public static IEnumerable<(byte A, byte R, byte G, byte B)> AllColors
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

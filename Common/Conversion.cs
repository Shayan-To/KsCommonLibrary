using System;

namespace Ks.Common
{
        public static class ParseInv
        {

            public static int Integer(string Str)
            {
                return int.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static long Long(string Str)
            {
                return long.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static short Short(string Str)
            {
                return short.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static byte Byte(string Str)
            {
                return byte.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static uint UInteger(string Str)
            {
                return uint.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static ulong ULong(string Str)
            {
                return ulong.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static ushort UShort(string Str)
            {
                return ushort.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static sbyte SByte(string Str)
            {
                return sbyte.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static double Double(string Str)
            {
                return double.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static float Single(string Str)
            {
                return float.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static decimal Decimal(string Str)
            {
                return decimal.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }

            public static bool Boolean(string Str)
            {
                return bool.Parse(Str);
            }

            public static bool TryInteger(string Str, out int Value)
            {
                return int.TryParse(Str, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryLong(string Str, out long Value)
            {
                return long.TryParse(Str, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryShort(string Str, out short Value)
            {
                return short.TryParse(Str, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryByte(string Str, out byte Value)
            {
                return byte.TryParse(Str, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryUInteger(string Str, out uint Value)
            {
                return uint.TryParse(Str, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryULong(string Str, out ulong Value)
            {
                return ulong.TryParse(Str, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryUShort(string Str, out ushort Value)
            {
                return ushort.TryParse(Str, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TrySByte(string Str, out sbyte Value)
            {
                return sbyte.TryParse(Str, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryDouble(string Str, out double Value)
            {
                return double.TryParse(Str, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TrySingle(string Str, out float Value)
            {
                return float.TryParse(Str, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryDecimal(string Str, out decimal Value)
            {
                return decimal.TryParse(Str, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out Value);
            }

            public static bool TryBoolean(string Str, out bool Value)
            {
                return bool.TryParse(Str, out Value);
            }
        }

        public static class ConversionExtensions
        {
            public static string ToStringInv(this int Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this long Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this short Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this byte Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this uint Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this ulong Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this ushort Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this sbyte Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this double Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this float Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this decimal Self)
            {
                return Self.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            public static string ToStringInv(this bool Self)
            {
                return Self.ToString();
            }
        }
    }

using System.Collections.Generic;
using System;
using System.Text;
using Media = System.Windows.Media;
using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class Serialization
        {

            public static Media.Color HexToColor(string Hex)
            {
                if (Hex.StartsWith("#"))
                    Hex = Hex.Substring(1);
                if (Hex.Length == 8)
                    return System.Windows.Media.Color.FromArgb(byte.Parse(Hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(Hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(Hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(Hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber));
                if (Hex.Length == 6)
                    return System.Windows.Media.Color.FromRgb(byte.Parse(Hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(Hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(Hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));

                throw new ArgumentException("Invalid hex color.");
            }

            public static string ColorToHex(Media.Color Color)
            {
                return string.Concat("#", Color.A.ToString("X2"), Color.R.ToString("X2"), Color.G.ToString("X2"), Color.B.ToString("X2"));
            }

            private static string EscapeChar(char Ch, string EscapeChars)
            {
                switch (Ch)
                {
                    case '\r':
                        return @"\r";
                    case '\n':
                        return @"\n";
                    default:
                        var s = Ch.ToString();
                        if (EscapeChars.Contains(s))
                            return "\\" + s;
                        return s;
                }
            }

            private static char UnescapeChar(char Ch, string EscapeChars)
            {
                switch (Ch)
                {
                    case 'r':
                        return '\r';
                    case 'n':
                        return '\n';
                    default:
                        if (EscapeChars.Contains(Ch.ToString()))
                            return Ch;
                        throw new Exception("Invalid escape character.");
                }
            }

            public static string ListToString(IEnumerable<string> List)
            {
                var Res = new StringBuilder("{");

                foreach (var Str in List)
                {
                    foreach (var Ch in Str)
                        Res.Append(EscapeChar(Ch, @",\{}"));

                    // We always append a comma to the end of a list.
                    // This Is to distinguish {} from {""}.

                    Res.Append(",");
                }

                return Res.Append("}").ToString();
            }

            public static List<string> ListFromString(string Str)
            {
                var Res = new List<string>();
                var R = new StringBuilder();
                for (var I = 0; I < Str.Length; I++)
                {
                    var Ch = Str[I];

                    if (I == 0)
                    {
                        if (Ch != '{')
                            throw new Exception("Invalid list string.");
                        continue;
                    }

                    if (Ch == '\\')
                    {
                        I += 1;
                        if (I == Str.Length)
                            throw new Exception("Invalid list string.");

                        R.Append(UnescapeChar(Str[I], @",\{}"));
                        continue;
                    }

                    if ((Ch == ',') | (Ch == '}'))
                    {
                        // We have to ignore the last comma. See ListToString.
                        // But for backward compatibility, we add the non-empty strings to the list.
                        if (!((Ch == '}') & (R.Length == 0)))
                        {
                            Res.Add(R.ToString());
                            R.Clear();
                        }

                        if ((Ch == '}') & (I != (Str.Length - 1)))
                            throw new Exception("Invalid list string.");

                        continue;
                    }

                    R.Append(Ch);
                }

                return Res;
            }

            public static string ListToStringMultiline(IEnumerable<string> List)
            {
                var Res = new StringBuilder();

                foreach (var Str in List)
                {
                    foreach (var Ch in Str)
                        Res.Append(EscapeChar(Ch, @"\"));

                    Res.AppendLine();
                }

                return Res.ToString();
            }

            public static List<string> ListFromStringMultiline(string Str)
            {
                var Res = new List<string>();
                var R = new StringBuilder();
                for (var I = 0; I < Str.Length; I++)
                {
                    var Ch = Str[I];

                    if (Ch == '\\')
                    {
                        I += 1;
                        if (I == Str.Length)
                            Verify.Fail("Invalid list string.");

                        R.Append(UnescapeChar(Str[I], @"\"));
                        continue;
                    }

                    if ((Ch == '\r') | (Ch == '\n'))
                    {
                        if (((Ch == '\r') & ((I + 1) < Str.Length)) && Str[I + 1] == '\n')
                            I += 1;

                        Res.Add(R.ToString());
                        R.Clear();

                        if ((I + 1) == Str.Length)
                            return Res;

                        continue;
                    }

                    R.Append(Ch);
                }

                if (Str.Length == 0)
                    return Res;

                Verify.Fail("Invalid list string.");
                return null;
            }

            public static string DicToString(IDictionary<string, string> Dic)
            {
                var Res = new StringBuilder("{");
                var Bl = true;

                foreach (var KV in Dic)
                {
                    if (Bl)
                        Bl = false;
                    else
                        Res.Append(",");

                    foreach (var Ch in KV.Key)
                        Res.Append(EscapeChar(Ch, @",\{}:"));
                    Res.Append(':');
                    foreach (var Ch in KV.Value)
                        Res.Append(EscapeChar(Ch, @",\{}"));
                }

                return Res.Append("}").ToString();
            }

            public static OrderedDictionary<string, string> DicFromString(string Str)
            {
                var Res = new OrderedDictionary<string, string>();
                var R = new StringBuilder();
                string Key = null;
                for (var I = 0; I < Str.Length; I++)
                {
                    var Ch = Str[I];

                    if (I == 0)
                    {
                        if (Ch != '{')
                            throw new Exception("Invalid dictionary string.");
                        continue;
                    }

                    if (Ch == '\\')
                    {
                        I += 1;
                        if (I == Str.Length)
                            throw new Exception("Invalid dictionary string.");

                        R.Append(UnescapeChar(Str[I], @",\{}:"));
                        continue;
                    }

                    if (Ch == ':')
                    {
                        Key = R.ToString();
                        R.Clear();

                        continue;
                    }

                    if ((Ch == ',') | (Ch == '}'))
                    {
                        // If Key is nothing, then the collection must be empty.
                        Verify.True((Key == null).Implies((Res.Count == 0) & (R.Length == 0)), "Invalid dictionary string.");

                        if (Key != null)
                        {
                            Res.Add(Key, R.ToString());
                            R.Clear();
                            Key = null;
                        }

                        if ((Ch == '}') & (I != (Str.Length - 1)))
                            throw new Exception("Invalid dictionary string.");

                        continue;
                    }

                    R.Append(Ch);
                }

                return Res;
            }

            public static string DicToStringMultiline(IDictionary<string, string> Dic)
            {
                var Res = new StringBuilder();

                foreach (var KV in Dic)
                {
                    foreach (var Ch in KV.Key)
                        Res.Append(EscapeChar(Ch, @"\:"));
                    Res.Append(':');
                    foreach (var Ch in KV.Value)
                        Res.Append(EscapeChar(Ch, @"\"));

                    Res.AppendLine();
                }

                return Res.ToString();
            }

            public static OrderedDictionary<string, string> DicFromStringMultiline(string Str)
            {
                var Res = new OrderedDictionary<string, string>();
                var R = new StringBuilder();
                string Key = null;
                for (var I = 0; I < Str.Length; I++)
                {
                    var Ch = Str[I];

                    if (Ch == '\\')
                    {
                        I += 1;
                        Verify.False(I == Str.Length, "Invalid dictionary string.");

                        R.Append(UnescapeChar(Str[I], @"\:"));
                        continue;
                    }

                    if ((Ch == ':') & Key == null)
                    {
                        Key = R.ToString();
                        R.Clear();

                        continue;
                    }

                    if ((Ch == '\r') | (Ch == '\n'))
                    {
                        if (((Ch == '\r') & ((I + 1) < Str.Length)) && Str[I + 1] == '\n')
                            I += 1;

                        Verify.False(Key == null, "Invalid dictionary string.");
                        Res.Add(Key, R.ToString());
                        R.Clear();
                        Key = null;

                        if ((I + 1) == Str.Length)
                            return Res;

                        continue;
                    }

                    R.Append(Ch);
                }

                if (Str.Length == 0)
                    return Res;

                Verify.Fail("Invalid dictionary string.");
                return null;
            }
        }
    }
}

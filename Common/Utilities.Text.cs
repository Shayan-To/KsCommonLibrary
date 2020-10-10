using System;
using System.Collections.Generic;
using System.Text;

using Media = System.Windows.Media;
using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class Text
        {

            private static readonly Dictionary<char, char> EscapeDic = new Dictionary<char, char>()
                {
                    {'0', (char)0x00},
                    {'"', (char)0x27},
                    {'\'', (char)0x22},
                    {'?', (char)0x3F},
                    {'\\', (char)0x5C},
                    {'a', (char)0x07},
                    {'b', (char)0x08},
                    {'f', (char)0x0C},
                    {'n', (char)0x0A},
                    {'r', (char)0x0D},
                    {'t', (char)0x09},
                    {'v', (char)0x0B}
                };

            public static bool IsBinaryDigit(char C)
            {
                return C == '0' || C == '1';
            }

            public static bool IsDecimalDigit(char C)
            {
                return '0' <= C && C <= '9';
            }

            public static bool IsOctalDigit(char C)
            {
                return '0' <= C && C <= '7';
            }

            public static bool IsHexadecimalDigit(char C)
            {
                return ('0' <= C && C <= '9') || ('A' <= C && C <= 'F') || ('a' <= C && C <= 'f');
            }

            public static char CEscapeC(string Input, bool DoesThrow = true)
            {
                char T1, T2;

                if (Input.Length == 0)
                {
                    throw new ArgumentException();
                }

                T2 = Input[0];

                if (T2 != '\\')
                {
                    if (DoesThrow && Input.Length != 1)
                    {
                        throw new ArgumentException("Invalid escaped character.");
                    }

                    return T2;
                }

                T1 = Input[1];

                if (EscapeDic.TryGetValue(T1, out T2))
                {
                    if (DoesThrow && Input.Length != 2)
                    {
                    }
                    return T2;
                }

                if (T1 == 'x')
                {
                    if (Input.Length != 4)
                    {
                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            return T1;
                        }
                    }
                    if (!(IsHexadecimalDigit(Input[2]) && IsHexadecimalDigit(Input[3])))
                    {
                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            return T1;
                        }
                    }

                    return (char) Math.ConvertFromBase(Input.Substring(2, 2), 16);
                }

                if (T1 == 'u')
                {
                    if (Input.Length != 6)
                    {
                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            return T1;
                        }
                    }
                    if (!(IsHexadecimalDigit(Input[2]) && IsHexadecimalDigit(Input[3]) && IsHexadecimalDigit(Input[4]) && IsHexadecimalDigit(Input[5])))
                    {
                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            return T1;
                        }
                    }

                    return (char) Math.ConvertFromBase(Input.Substring(2, 4), 16);
                }

                if (T1 == 'U')
                {
                    if (Input.Length != 10)
                    {
                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            return T1;
                        }
                    }
                    if (!(IsHexadecimalDigit(Input[2]) && IsHexadecimalDigit(Input[3]) && IsHexadecimalDigit(Input[4]) && IsHexadecimalDigit(Input[5]) && IsHexadecimalDigit(Input[6]) && IsHexadecimalDigit(Input[7]) && IsHexadecimalDigit(Input[8]) && IsHexadecimalDigit(Input[9])))
                    {
                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            return T1;
                        }
                    }

                    return (char) Math.ConvertFromBase(Input.Substring(2, 8), 16);
                }

                if (IsOctalDigit(T1))
                {
                    if (Input.Length != 4)
                    {
                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            return T1;
                        }
                    }
                    if (!(IsOctalDigit(Input[2]) && IsOctalDigit(Input[3])))
                    {
                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            return T1;
                        }
                    }

                    return (char) Math.ConvertFromBase(Input.Substring(1, 3), 8);
                }

                if (DoesThrow)
                {
                    throw new ArgumentException("Invalid escaped string.");
                }
                else
                {
                    return T1;
                }
            }

            public static string CEscape(string Input, bool DoesThrow = true)
            {
                char T1 = default, T2 = default;

                var Res = new StringBuilder();
                for (var I = 0; I < Input.Length; I++)
                {
                    T2 = Input[I];

                    if (T2 == '\\')
                    {
                        I += 1;

                        if (I == Input.Length)
                        {
                            if (DoesThrow)
                            {
                                throw new ArgumentException("Invalid escaped string.");
                            }
                            else
                            {
                                break;
                            }
                        }

                        T1 = Input[I];

                        if (EscapeDic.TryGetValue(T1, out T2))
                        {
                            Res.Append(T2);
                            continue;
                        }

                        if (T1 == 'x')
                        {
                            if ((I + 2) >= Input.Length)
                            {
                                if (DoesThrow)
                                {
                                    throw new ArgumentException("Invalid escaped string.");
                                }
                                else
                                {
                                    Res.Append(T1);
                                    continue;
                                }
                            }
                            if (!(IsHexadecimalDigit(Input[I + 1]) && IsHexadecimalDigit(Input[I + 2])))
                            {
                                if (DoesThrow)
                                {
                                    throw new ArgumentException("Invalid escaped string.");
                                }
                                else
                                {
                                    Res.Append(T1);
                                    continue;
                                }
                            }

                            Res.Append((char) Math.ConvertFromBase(Input.Substring(I + 1, 2), 16));
                            I += 2;
                            continue;
                        }

                        if (T1 == 'u')
                        {
                            if ((I + 4) >= Input.Length)
                            {
                                if (DoesThrow)
                                {
                                    throw new ArgumentException("Invalid escaped string.");
                                }
                                else
                                {
                                    Res.Append(T1);
                                    continue;
                                }
                            }
                            if (!(IsHexadecimalDigit(Input[I + 1]) && IsHexadecimalDigit(Input[I + 2]) && IsHexadecimalDigit(Input[I + 3]) && IsHexadecimalDigit(Input[I + 4])))
                            {
                                if (DoesThrow)
                                {
                                    throw new ArgumentException("Invalid escaped string.");
                                }
                                else
                                {
                                    Res.Append(T1);
                                    continue;
                                }
                            }

                            Res.Append((char) Math.ConvertFromBase(Input.Substring(I + 1, 4), 16));
                            I += 4;
                            continue;
                        }

                        if (T1 == 'U')
                        {
                            if ((I + 8) >= Input.Length)
                            {
                                if (DoesThrow)
                                {
                                    throw new ArgumentException("Invalid escaped string.");
                                }
                                else
                                {
                                    Res.Append(T1);
                                    continue;
                                }
                            }
                            if (!(IsHexadecimalDigit(Input[I + 1]) && IsHexadecimalDigit(Input[I + 2]) && IsHexadecimalDigit(Input[I + 3]) && IsHexadecimalDigit(Input[I + 4]) && IsHexadecimalDigit(Input[I + 5]) && IsHexadecimalDigit(Input[I + 6]) && IsHexadecimalDigit(Input[I + 7]) && IsHexadecimalDigit(Input[I + 8])))
                            {
                                if (DoesThrow)
                                {
                                    throw new ArgumentException("Invalid escaped string.");
                                }
                                else
                                {
                                    Res.Append(T1);
                                    continue;
                                }
                            }

                            Res.Append((char) Math.ConvertFromBase(Input.Substring(I + 1, 8), 16));
                            I += 8;
                            continue;
                        }

                        if (IsOctalDigit(T1))
                        {
                            if ((I + 2) >= Input.Length)
                            {
                                if (DoesThrow)
                                {
                                    throw new ArgumentException("Invalid escaped string.");
                                }
                                else
                                {
                                    Res.Append(T1);
                                    continue;
                                }
                            }
                            if (!(IsOctalDigit(Input[I + 1]) && IsOctalDigit(Input[I + 2])))
                            {
                                if (DoesThrow)
                                {
                                    throw new ArgumentException("Invalid escaped string.");
                                }
                                else
                                {
                                    Res.Append(T1);
                                    continue;
                                }
                            }

                            Res.Append((char) Math.ConvertFromBase(Input.Substring(I, 3), 8));
                            I += 2;
                            continue;
                        }

                        if (DoesThrow)
                        {
                            throw new ArgumentException("Invalid escaped string.");
                        }
                        else
                        {
                            Res.Append(T1);
                            continue;
                        }
                    }
                    Res.Append(T2);
                }

                return Res.ToString();
            }

            public static string EscapeNewLine(string Str)
            {
                var Res = new StringBuilder();

                foreach (var Ch in Str)
                {
                    switch (Ch)
                    {
                        case '\r':
                            Res.Append(@"\r");
                            break;
                        case '\n':
                            Res.Append(@"\n");
                            break;
                        case '\\':
                            Res.Append(@"\\");
                            break;
                        default:
                            Res.Append(Ch);
                            break;
                    }
                }

                return Res.ToString();
            }

            public static string UnescapeNewLine(string Str)
            {
                var Res = new StringBuilder();
                for (var I = 0; I < Str.Length; I++)
                {
                    var Ch = Str[I];

                    if (Ch == '\\')
                    {
                        I += 1;
                        if (I == Str.Length)
                        {
                            throw new Exception("Invalid list string.");
                        }

                        Ch = Str[I];

                        switch (Ch)
                        {
                            case 'r':
                                Res.Append('\r');
                                break;
                            case 'n':
                                Res.Append('\n');
                                break;
                            case '\\':
                                Res.Append(@"\");
                                break;
                            default:
                                throw new Exception("Invalid escape character.");
                        }
                    }

                    Res.Append(Ch);
                }

                return Res.ToString();
            }

            public static IFormatProvider CurruntFormatProvider => null;

            public static string FirstCapitalized(string Str)
            {
                return char.ToUpper(Str[0]).ToString() + Str.Substring(1).ToLower();
            }

            public static string EnumerableToString<T>(IEnumerable<T> Enumerable)
            {
                var Res = new StringBuilder("{");
                var Bl = true;

                foreach (var I in Enumerable)
                {
                    if (Bl)
                    {
                        Bl = false;
                    }
                    else
                    {
                        Res.Append(", ");
                    }

                    Res.Append(I);
                }

                return Res.Append('}').ToString();
            }

            public static string PadAlignString(string Str, char Ch, int Length, TextAlignment Alignment)
            {
                Verify.False(Length < Str.Length, "Length of Str is less than Length.");

                var N = Str.Length;

                var Res = "";
                switch (Alignment)
                {
                    case TextAlignment.Left:
                        Res = Str.PadRight(Length, Ch);
                        break;
                    case TextAlignment.Center:
                        Res = Str.PadRight(Length - (N / 2), Ch).PadLeft(Length, Ch);
                        break;
                    case TextAlignment.Right:
                        Res = Str.PadLeft(Length, Ch);
                        break;
                    default:
                        Verify.FailArg(nameof(Alignment), "Invalid Alignment.");
                        break;
                }

                return Res;
            }

            public enum TextAlignment
            {
                Right,
                Center,
                Left
            }
        }
    }
}

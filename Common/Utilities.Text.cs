using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Ks
{
    namespace Common
    {
        partial class Utilities
        {
            public class Text
            {
                private Text()
                {
                    throw new NotSupportedException();
                }

                private static readonly Dictionary<char, char> EscapeDic = (() => new Dictionary<char, char>()
                {
                    {
                        '0',
                        (char)0x0
                    },
                    {
                        '"',
                        (char)0x27
                    },
                    {
                        '\'',
                        (char)0x22
                    },
                    {
                        '?',
                        (char)0x3F
                    },
                    {
                        '\\',
                        (char)0x5C
                    },
                    {
                        'a',
                        (char)0x7
                    },
                    {
                        'b',
                        (char)0x8
                    },
                    {
                        'f',
                        (char)0xC
                    },
                    {
                        'n',
                        (char)0xA
                    },
                    {
                        'r',
                        (char)0xD
                    },
                    {
                        't',
                        (char)0x9
                    },
                    {
                        'v',
                        (char)0xB
                    }
                }).Invoke();

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
                        throw new ArgumentException();

                    T2 = Input.Chars[0];

                    if (Conversions.ToString(T2) != @"\")
                    {
                        if (DoesThrow && Input.Length != 1)
                            throw new ArgumentException("Invalid escaped character.");
                        return T2;
                    }

                    T1 = Input.Chars[1];

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
                                throw new ArgumentException("Invalid escaped string.");
                            else
                                return T1;
                        }
                        if (!(IsHexadecimalDigit(Input.Chars[2]) && IsHexadecimalDigit(Input.Chars[3])))
                        {
                            if (DoesThrow)
                                throw new ArgumentException("Invalid escaped string.");
                            else
                                return T1;
                        }

                        return (char)Convert.ToInt32(Input.Substring(2, 2), 16);
                    }

                    if (T1 == 'u')
                    {
                        if (Input.Length != 6)
                        {
                            if (DoesThrow)
                                throw new ArgumentException("Invalid escaped string.");
                            else
                                return T1;
                        }
                        if (!(IsHexadecimalDigit(Input.Chars[2]) && IsHexadecimalDigit(Input.Chars[3]) && IsHexadecimalDigit(Input.Chars[4]) && IsHexadecimalDigit(Input.Chars[5])))
                        {
                            if (DoesThrow)
                                throw new ArgumentException("Invalid escaped string.");
                            else
                                return T1;
                        }

                        return (char)Convert.ToInt32(Input.Substring(2, 4), 16);
                    }

                    if (T1 == 'U')
                    {
                        if (Input.Length != 10)
                        {
                            if (DoesThrow)
                                throw new ArgumentException("Invalid escaped string.");
                            else
                                return T1;
                        }
                        if (!(IsHexadecimalDigit(Input.Chars[2]) && IsHexadecimalDigit(Input.Chars[3]) && IsHexadecimalDigit(Input.Chars[4]) && IsHexadecimalDigit(Input.Chars[5]) && IsHexadecimalDigit(Input.Chars[6]) && IsHexadecimalDigit(Input.Chars[7]) && IsHexadecimalDigit(Input.Chars[8]) && IsHexadecimalDigit(Input.Chars[9])))
                        {
                            if (DoesThrow)
                                throw new ArgumentException("Invalid escaped string.");
                            else
                                return T1;
                        }

                        return (char)Convert.ToInt32(Input.Substring(2, 8), 16);
                    }

                    if (IsOctalDigit(T1))
                    {
                        if (Input.Length != 4)
                        {
                            if (DoesThrow)
                                throw new ArgumentException("Invalid escaped string.");
                            else
                                return T1;
                        }
                        if (!(IsOctalDigit(Input.Chars[2]) && IsOctalDigit(Input.Chars[3])))
                        {
                            if (DoesThrow)
                                throw new ArgumentException("Invalid escaped string.");
                            else
                                return T1;
                        }

                        return (char)Convert.ToInt32(Input.Substring(1, 3), 8);
                    }

                    if (DoesThrow)
                        throw new ArgumentException("Invalid escaped string.");
                    else
                        return T1;
                }

                public static string CEscape(string Input, bool DoesThrow = true)
                {
                    char T1 = default(char), T2 = default(char);

                    var Res = new StringBuilder();
                    var loopTo = Input.Length - 1;
                    for (int I = 0; I <= loopTo; I++)
                    {
                        T2 = Input.Chars[I];

                        if (T2 == '\\')
                        {
                            I += 1;

                            if (I == Input.Length)
                            {
                                if (DoesThrow)
                                    throw new ArgumentException("Invalid escaped string.");
                                else
                                    break;
                            }

                            T1 = Input.Chars[I];

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
                                        throw new ArgumentException("Invalid escaped string.");
                                    else
                                    {
                                        Res.Append(T1);
                                        continue;
                                    }
                                }
                                if (!(IsHexadecimalDigit(Input.Chars[I + 1]) && IsHexadecimalDigit(Input.Chars[I + 2])))
                                {
                                    if (DoesThrow)
                                        throw new ArgumentException("Invalid escaped string.");
                                    else
                                    {
                                        Res.Append(T1);
                                        continue;
                                    }
                                }

                                Res.Append((char)Convert.ToInt32(Input.Substring(I + 1, 2), 16));
                                I += 2;
                                continue;
                            }

                            if (T1 == 'u')
                            {
                                if ((I + 4) >= Input.Length)
                                {
                                    if (DoesThrow)
                                        throw new ArgumentException("Invalid escaped string.");
                                    else
                                    {
                                        Res.Append(T1);
                                        continue;
                                    }
                                }
                                if (!(IsHexadecimalDigit(Input.Chars[I + 1]) && IsHexadecimalDigit(Input.Chars[I + 2]) && IsHexadecimalDigit(Input.Chars[I + 3]) && IsHexadecimalDigit(Input.Chars[I + 4])))
                                {
                                    if (DoesThrow)
                                        throw new ArgumentException("Invalid escaped string.");
                                    else
                                    {
                                        Res.Append(T1);
                                        continue;
                                    }
                                }

                                Res.Append((char)Convert.ToInt32(Input.Substring(I + 1, 4), 16));
                                I += 4;
                                continue;
                            }

                            if (T1 == 'U')
                            {
                                if ((I + 8) >= Input.Length)
                                {
                                    if (DoesThrow)
                                        throw new ArgumentException("Invalid escaped string.");
                                    else
                                    {
                                        Res.Append(T1);
                                        continue;
                                    }
                                }
                                if (!(IsHexadecimalDigit(Input.Chars[I + 1]) && IsHexadecimalDigit(Input.Chars[I + 2]) && IsHexadecimalDigit(Input.Chars[I + 3]) && IsHexadecimalDigit(Input.Chars[I + 4]) && IsHexadecimalDigit(Input.Chars[I + 5]) && IsHexadecimalDigit(Input.Chars[I + 6]) && IsHexadecimalDigit(Input.Chars[I + 7]) && IsHexadecimalDigit(Input.Chars[I + 8])))
                                {
                                    if (DoesThrow)
                                        throw new ArgumentException("Invalid escaped string.");
                                    else
                                    {
                                        Res.Append(T1);
                                        continue;
                                    }
                                }

                                Res.Append((char)Convert.ToInt32(Input.Substring(I + 1, 8), 16));
                                I += 8;
                                continue;
                            }

                            if (IsOctalDigit(T1))
                            {
                                if ((I + 2) >= Input.Length)
                                {
                                    if (DoesThrow)
                                        throw new ArgumentException("Invalid escaped string.");
                                    else
                                    {
                                        Res.Append(T1);
                                        continue;
                                    }
                                }
                                if (!(IsOctalDigit(Input.Chars[I + 1]) && IsOctalDigit(Input.Chars[I + 2])))
                                {
                                    if (DoesThrow)
                                        throw new ArgumentException("Invalid escaped string.");
                                    else
                                    {
                                        Res.Append(T1);
                                        continue;
                                    }
                                }

                                Res.Append((char)Convert.ToInt32(Input.Substring(I, 3), 8));
                                I += 2;
                                continue;
                            }

                            if (DoesThrow)
                                throw new ArgumentException("Invalid escaped string.");
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
                            case ControlChars.Cr:
                                {
                                    Res.Append(@"\r");
                                    break;
                                }

                            case ControlChars.Lf:
                                {
                                    Res.Append(@"\n");
                                    break;
                                }

                            case '\\':
                                {
                                    Res.Append(@"\\");
                                    break;
                                }

                            default:
                                {
                                    Res.Append(Ch);
                                    break;
                                }
                        }
                    }

                    return Res.ToString();
                }

                public static string UnescapeNewLine(string Str)
                {
                    var Res = new StringBuilder();
                    var loopTo = Str.Length - 1;
                    for (int I = 0; I <= loopTo; I++)
                    {
                        var Ch = Str.Chars[I];

                        if (Ch == '\\')
                        {
                            I += 1;
                            if (I == Str.Length)
                                throw new Exception("Invalid list string.");
                            Ch = Str.Chars[I];

                            switch (Ch)
                            {
                                case 'r':
                                    {
                                        Res.Append(ControlChars.Cr);
                                        break;
                                    }

                                case 'n':
                                    {
                                        Res.Append(ControlChars.Lf);
                                        break;
                                    }

                                case '\\':
                                    {
                                        Res.Append(@"\");
                                        break;
                                    }

                                default:
                                    {
                                        throw new Exception("Invalid escape character.");
                                        break;
                                    }
                            }
                        }

                        Res.Append(Ch);
                    }

                    return Res.ToString();
                }

                public static IFormatProvider CurruntFormatProvider
                {
                    get
                    {
                        return null;
                    }
                }

                public static string FirstCapitalized(string Str)
                {
                    return Conversions.ToString(char.ToUpper(Str.Chars[0])) + Str.Substring(1).ToLower();
                }

                public static string EnumerableToString<T>(IEnumerable<T> Enumerable)
                {
                    var Res = new StringBuilder(Conversions.ToString('{'));
                    var Bl = true;

                    foreach (T I in Enumerable)
                    {
                        if (Bl)
                            Bl = false;
                        else
                            Res.Append(", ");
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
                            {
                                Res = Str.PadRight(Length, Ch);
                                break;
                            }

                        case TextAlignment.Center:
                            {
                                Res = Str.PadRight(Length - (N / 2), Ch).PadLeft(Length, Ch);
                                break;
                            }

                        case TextAlignment.Right:
                            {
                                Res = Str.PadLeft(Length, Ch);
                                break;
                            }

                        default:
                            {
                                Verify.FailArg(nameof(Alignment), "Invalid Alignment.");
                                break;
                            }
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
}

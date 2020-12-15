using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

using Media = System.Windows.Media;
using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class Math
        {

            /// <summary>
            /// Calculates the non-negative reminder of the two numbers specified.
            /// </summary>
            /// <param name="A">The dividend</param>
            /// <param name="B">The divisor</param>
            /// <returns>
            /// The reminder, R, of A divided by B. If it is a negative number, A + Abs(B) will be returned.
            /// The result is always between 0 and B - 1.
            /// </returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int NonNegMod(int A, int B)
            {
                A %= B;
                if (A >= 0)
                {
                    return A;
                }

                if (B > 0)
                {
                    return A + B;
                }

                return A - B;
            }

            /// <summary>
            /// Calculates the positive reminder of the two numbers specified.
            /// </summary>
            /// <param name="A">The dividend</param>
            /// <param name="B">The divisor</param>
            /// <returns>
            /// The reminder, R, of A divided by B. If it is not a positive number, A + Abs(B) will be returned.
            /// The result is always between 1 and B.
            /// </returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int PosMod(int A, int B)
            {
                A %= B;
                if (A > 0)
                {
                    return A;
                }

                if (B > 0)
                {
                    return A + B;
                }

                return A - B;
            }

            public static (ulong Low, ulong High) MultLongTo128U(ulong A, ulong B)
            {
                const int N = 32;
                const ulong NMask = (1UL << N) - 1;

                // A long has enough space to hold (int * int + 2 * int).
                // Proof:
                // (2^n - 1) * (2^n - 1) + 2 * (2^n - 1)
                // = ((2^n - 1) + 1)^2 - 1
                // = 2^2n - 1

                var A1 = A & NMask;
                var A2 = A >> N;
                var B1 = B & NMask;
                var B2 = B >> N;

                var Low = A1 * B1;
                var High = A2 * B2;

                var Mid = Low >> N;
                Low &= NMask;

                Mid += A1 * B2;
                High += Mid >> N;

                Mid &= NMask;

                Mid += A2 * B1;
                High += Mid >> N;
                Low += Mid << N;

                return (Low, High);
            }

            public static (long Low, long High) MultLongTo128(long A, long B)
            {
                // ToDo How this can work for negative numbers?
                throw new NotImplementedException();
            }

            public static int Power(int A, int B)
            {
                Verify.FalseArg(B < 0, nameof(B), $"{nameof(B)} must be non-negative.");
                var R = 1;
                while (B != 0)
                {
                    if ((B & 1) == 1)
                    {
                        R *= A;
                    }

                    A *= A;
                    B >>= 1;
                }
                return R;
            }

            public static long PowerL(long A, int B)
            {
                Verify.FalseArg(B < 0, nameof(B), $"{nameof(B)} must be non-negative.");
                var R = 1L;
                while (B != 0)
                {
                    if ((B & 1) == 1)
                    {
                        R *= A;
                    }

                    A *= A;
                    B >>= 1;
                }
                return R;
            }

            public static (int Root, int Reminder) SquareRoot(int A)
            {
                Verify.FalseArg(A < 0, nameof(A), $"{nameof(A)} must be non-negative.");

                var ARev = 0;
                var T = A;
                while (T != 0)
                {
                    ARev = (ARev << 2) | (T & 3);
                    T >>= 2;
                }

                var Reminder = 0;
                var Root = 0;
                while (A != 0)
                {
                    Reminder = (Reminder << 2) | (ARev & 3);

                    ARev >>= 2;
                    A >>= 2;

                    Root <<= 1;
                    var Root2 = (Root << 1) | 1;

                    if (Reminder >= Root2)
                    {
                        Root |= 1;
                        Reminder -= Root2;
                    }
                }

                return (Root, Reminder);
            }

            public static (long Root, long Reminder) SquareRootL(long A)
            {
                Verify.FalseArg(A < 0, nameof(A), $"{nameof(A)} must be non-negative.");

                var ARev = 0L;
                var T = A;
                while (T != 0)
                {
                    ARev = (ARev << 2) | (T & 3);
                    T >>= 2;
                }

                var Reminder = 0L;
                var Root = 0L;
                while (A != 0)
                {
                    Reminder = (Reminder << 2) | (ARev & 3);

                    ARev >>= 2;
                    A >>= 2;

                    Root <<= 1;
                    var Root2 = (Root << 1) | 1;

                    if (Reminder >= Root2)
                    {
                        Root |= 1;
                        Reminder -= Root2;
                    }
                }

                return (Root, Reminder);
            }

            public static int LeastPowerOfTwoOnMin(int Min)
            {
                if (Min < 1)
                {
                    return 1;
                }

                // If Min is a power of two, we should return Min, otherwise, Min * 2
                var T = (Min - 1) & Min;
                if (T == 0)
                {
                    return Min;
                }

                Min = T;

                while (true)
                {
                    T = (Min - 1) & Min;
                    if (T == 0)
                    {
                        return Min << 1;
                    }

                    Min = T;
                }
            }

            public static long LeastPowerOfTwoOnMin(long Min)
            {
                if (Min < 1)
                {
                    return 1;
                }

                // If Min is a power of two, we should return Min, otherwise, Min * 2
                var T = (Min - 1) & Min;
                if (T == 0)
                {
                    return Min;
                }

                Min = T;

                while (true)
                {
                    T = (Min - 1) & Min;
                    if (T == 0)
                    {
                        return Min << 1;
                    }

                    Min = T;
                }
            }

            public static int FloorDiv(int A, int B)
            {
                if (B < 0)
                {
                    A = -A;
                    B = -B;
                }
                if ((A >= 0) | ((A % B) == 0))
                {
                    return A / B;
                }

                return (A / B) - 1;
            }

            public static long FloorDiv(long A, long B)
            {
                if (B < 0)
                {
                    A = -A;
                    B = -B;
                }
                if ((A >= 0) | ((A % B) == 0))
                {
                    return A / B;
                }

                return (A / B) - 1;
            }

            public static int CeilDiv(int A, int B)
            {
                if (B < 0)
                {
                    A = -A;
                    B = -B;
                }
                if ((A < 0) | ((A % B) == 0))
                {
                    return A / B;
                }

                return (A / B) + 1;
            }

            public static long CeilDiv(long A, long B)
            {
                if (B < 0)
                {
                    A = -A;
                    B = -B;
                }
                if ((A < 0) | ((A % B) == 0))
                {
                    return A / B;
                }

                return (A / B) + 1;
            }

            public static int GreatestCommonDivisor(int A, int B)
            {
                if (B < 0)
                {
                    B = -B;
                }

                if (A < 0)
                {
                    A = -A;
                }

                while (B != 0)
                {
                    var C = A % B;
                    A = B;
                    B = C;
                }
                return A;
            }

            public static long GreatestCommonDivisor(long A, long B)
            {
                if (B < 0)
                {
                    B = -B;
                }

                if (A < 0)
                {
                    A = -A;
                }

                while (B != 0)
                {
                    var C = A % B;
                    A = B;
                    B = C;
                }
                return A;
            }

            public static int LeastCommonMultiple(int A, int B)
            {
                return (A / GreatestCommonDivisor(A, B)) * B;
            }

            public static long LeastCommonMultiple(long A, long B)
            {
                return (A / GreatestCommonDivisor(A, B)) * B;
            }

            public static (int Log, long Reminder) Logarithm(long N, long Base)
            {
                var Reminder = 0L;
                var Power = 1L;
                var Log = 0;
                while (N != 0)
                {
                    Reminder += (N % Base) * Power;
                    N /= Base;
                    Power *= Base;
                    Log += 1;
                }
                return (Log, Reminder);
            }

            public static bool IsOfIntegralType(object O)
            {
                return O is byte | O is ushort | O is uint | O is ulong | O is sbyte | O is short | O is int | O is long | O is float | O is double;
            }

            public static string ConvertToBase(long N, char[] Digits, char NegativeSign = '-')
            {
                var IsNegative = N < 0;
                if (IsNegative)
                {
                    N = -N;
                }

                var Res = new List<char>();
                var Base = Digits.Length;

                while (N != 0)
                {
                    Res.Add(Digits[N % Base]);
                    N /= Base;
                }

                if (IsNegative)
                {
                    Res.Add(NegativeSign);
                }

                Res.Reverse();

                return new string(Res.ToArray());
            }

            public static long ConvertFromBase(string N, char[] Digits, char NegativeSign = '-')
            {
                var I = 0;
                var IsNegative = N[I] == NegativeSign;
                if (IsNegative)
                {
                    I += 1;
                }

                var Res = 0L;
                var Base = Digits.Length;
                for (; I < N.Length; I++)
                {
                    var T = Array.IndexOf(Digits, N[I]);
                    Verify.False(T == -1, $"Invalid digit at index {I}.");
                    Res = (Res * Base) + T;
                }

                if (IsNegative)
                {
                    Res = -Res;
                }

                return Res;
            }

            public static string ConvertToBaseU(ulong N, char[] Digits)
            {
                var Res = new List<char>();
                var Base = (uint) Digits.Length;

                while (N != 0)
                {
                    Res.Add(Digits[N % Base]);
                    N /= Base;
                }

                Res.Reverse();

                return new string(Res.ToArray());
            }

            public static ulong ConvertFromBaseU(string N, char[] Digits)
            {
                var I = 0;

                var Res = 0UL;
                var Base = (uint) Digits.Length;
                for (; I < N.Length; I++)
                {
                    var T = Array.IndexOf(Digits, N[I]);
                    Verify.False(T == -1, $"Invalid digit at index {I}.");
                    Res = (Res * Base) + (ulong) T;
                }

                return Res;
            }

            public static string ConvertToBase(long N, int Base)
            {
                return ConvertToBase(N, Digits[Base]);
            }

            public static long ConvertFromBase(string N, int Base)
            {
                return ConvertFromBase(N, Digits[Base]);
            }

            public static string ConvertToBaseU(ulong N, int Base)
            {
                return ConvertToBaseU(N, Digits[Base]);
            }

            public static ulong ConvertFromBaseU(string N, int Base)
            {
                return ConvertFromBaseU(N, Digits[Base]);
            }

            public static string ConvertToBaseB(byte[] N, char[] Digits)
            {
                var Base = Digits.Length;
                var LogRem = Logarithm(Base, 2);
                var DigitBits = LogRem.Log;
                const int ByteBits = 8;

                Verify.TrueArg(LogRem.Reminder == 0, nameof(Digits), "Base must be a power of two.");

                var BlockSize = DigitBits / GreatestCommonDivisor(DigitBits, ByteBits);
                var Offset = ((BlockSize - (N.Length % BlockSize)) * ByteBits) % DigitBits;

                var Res = new char[(CeilDiv(N.Length, BlockSize) * ((BlockSize * ByteBits) / DigitBits)) + 1];
                var Index = 0;

                var J = 0;
                var T = 0;
                for (var I = 0; I < N.Length; I++)
                {
                    var Cur = (int) N[I];

                    J = 0;
                    var Size = DigitBits - Offset;
                    while ((J + Size) <= ByteBits)
                    {
                        J += Size;
                        var C = Cur >> (ByteBits - J);
                        C &= (1 << Size) - 1;
                        T = (T << Size) | C;

                        Res[Index] = Digits[T];

                        T = 0;
                        Offset = 0;
                    }

                    Size = ByteBits - J;
                    T = (T << Size) | (Cur & ((1 << Size) - 1));
                    Offset = (Offset + Size) % DigitBits;
                }

                Assert.True(J == ByteBits);

                return new string(Res);
            }

            public static byte[] ConvertFromBaseB(string N, char[] Digits)
            {
                var Base = Digits.Length;
                var LogRem = Logarithm(Base, 2);
                var DigitBits = LogRem.Log;
                const int ByteBits = 8;

                Verify.TrueArg(LogRem.Reminder == 0, nameof(Digits), "Base must be a power of two.");

                var BlockSize = ByteBits / GreatestCommonDivisor(DigitBits, ByteBits);
                var Offset = ((BlockSize - (N.Length % BlockSize)) * DigitBits) % ByteBits;

                var Res = new byte[(CeilDiv(N.Length, BlockSize) * ((BlockSize * DigitBits) / ByteBits)) + 1];
                var Index = 0;

                var J = 0;
                var T = 0;
                for (var I = 0; I < N.Length; I++)
                {
                    var Cur = Array.IndexOf(Digits, N[I]);
                    Verify.False(Cur == -1, $"Invalid digit at index {I}.");

                    J = 0;
                    var Size = ByteBits - Offset;
                    while ((J + Size) <= DigitBits)
                    {
                        J += Size;
                        var C = Cur >> (DigitBits - J);
                        C &= (1 << Size) - 1;
                        T = (T << Size) | C;

                        Res[Index] = (byte) T;

                        T = 0;
                        Offset = 0;
                    }

                    Size = DigitBits - J;
                    T = (T << Size) | (Cur & ((1 << Size) - 1));
                    Offset = (Offset + Size) % ByteBits;
                }

                Assert.True(J == DigitBits);

                return Res;
            }

            public static string ConvertToBaseB(byte[] N, int Base)
            {
                return ConvertToBaseB(N, Digits[Base]);
            }

            public static byte[] ConvertFromBaseB(string N, int Base)
            {
                return ConvertFromBaseB(N, Digits[Base]);
            }

            private static readonly char[][] Digits = new Func<char[][]>(() =>
            {
                var D = Collections.Concat(Collections.Range(10).Select(I => (char) ('0' + I)), Collections.Range(26).Select(I => (char) ('a' + I)))
                                .ToArray();
                return Collections.Range(2, D.Length - 2).Select(I => D.Subarray(0, I)).ToArray();
            }).Invoke();

#pragma warning disable IDE0052 // Remove unread private members
            /// <summary>The last character is the padding character.</summary>
            private static readonly char[] Base64Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=".ToCharArray();
#pragma warning restore IDE0052 // Remove unread private members
        }
    }
}

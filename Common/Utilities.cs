using System;
using Media = System.Windows.Media;
using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
        public static partial class Utilities
        {

            public static int CombineHashCodes(int H1, int H2)
            {
                // ToDo:
                // Dim Rol5 = ((CUInt(H1) << 5) Or (CUInt(H1) >> 27);
                // Return (CInt(Rol5) + H1) Xor H2;
                return ((H1 << 5) | (H1 >> 27)) ^ H2;
            }

            public static int CombineHashCodes(int H1, int H2, int H3)
            {
                return CombineHashCodes(CombineHashCodes(H1, H2), H3);
            }

            public static int CombineHashCodes(int H1, int H2, int H3, int H4)
            {
                return CombineHashCodes(CombineHashCodes(H1, H2), CombineHashCodes(H3, H4));
            }

            public static int CombineHashCodes(int H1, int H2, int H3, int H4, int H5)
            {
                return CombineHashCodes(CombineHashCodes(H1, H2), CombineHashCodes(H3, H4, H5));
            }

            public static int CombineHashCodes(int H1, int H2, int H3, int H4, int H5, int H6)
            {
                return CombineHashCodes(CombineHashCodes(H1, H2, H3), CombineHashCodes(H4, H5, H6));
            }

            public static int CombineHashCodes(int H1, int H2, int H3, int H4, int H5, int H6, int H7)
            {
                return CombineHashCodes(CombineHashCodes(H1, H2, H3), CombineHashCodes(H4, H5, H6, H7));
            }

            public static int CombineHashCodes(int H1, int H2, int H3, int H4, int H5, int H6, int H7, int H8)
            {
                return CombineHashCodes(CombineHashCodes(H1, H2, H3, H4), CombineHashCodes(H5, H6, H7, H8));
            }

            public static object EmptyObject = new object();

            public static class Typed<T>
            {
                public static readonly Func<T, T> IdentityFunc = X => X;
                public static readonly T[] EmptyArray = new T[0] { };
            }

            public static (double R, double G, double B) HslToRgb(double H, double S, double L)
            {
                throw new NotImplementedException();
            }

            public static (double R, double G, double B) HsbToRgb(double H, double S, double B)
            {
                throw new NotImplementedException();
            }

            public static void DoNothing()
            {
            }
        }
    }

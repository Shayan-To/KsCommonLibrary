using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

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

        public static string GenerateStringId(int length = 10)
        {
            var random = DefaultCacher<RandomNumberGenerator>.Value;
            var buffer = new byte[(length * 6 / 8) + 1];
            random.GetBytes(buffer);
            return Convert.ToBase64String(buffer)[..length].Replace('+', '-').Replace('/', '_');
        }

        public static async Task Delay(int milliseconds, CancellationToken cancellationToken = default)
        {
            if (!cancellationToken.CanBeCanceled)
            {
                await Task.Delay(milliseconds);
                return;
            }
            try
            {
                await Task.Delay(milliseconds, cancellationToken);
            }
            catch (TaskCanceledException)
            {
            }
        }

        public static async Task Delay(TimeSpan time, CancellationToken cancellationToken = default)
        {
            if (!cancellationToken.CanBeCanceled)
            {
                await Task.Delay(time);
                return;
            }
            try
            {
                await Task.Delay(time, cancellationToken);
            }
            catch (TaskCanceledException)
            {
            }
        }

        public static void DoNothing()
        {
        }

        public static void DoNothing<T>(T t)
        {
        }

        public static readonly object EmptyObject = new();

        public static class Typed<T>
        {
            public static readonly Func<T, T> IdentityFunc = X => X;
            public static readonly T[] EmptyArray = new T[0] { };
        }
    }
}

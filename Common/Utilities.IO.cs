using System.Net.Http;
using System.Threading.Tasks;

using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class IO
        {
            public static readonly HttpClient _HttpClient = new();

            public static async Task<string> DownloadURLAsync(string URL)
            {
                using var Reader = new SIO.StreamReader(await _HttpClient.GetStreamAsync(URL));
                var Res = Reader.ReadToEnd();
                return Res;
            }

            public static async Task DownloadURLToFileAsync(string URL, string Path)
            {
                using var WStream = await _HttpClient.GetStreamAsync(URL);
                using var FStream = SIO.File.Open(Path, SIO.FileMode.CreateNew, SIO.FileAccess.Write, SIO.FileShare.Read);
                FStream.Write(WStream);
            }

            public static int ReadAll(ReadCall Reader, byte[] Buffer, int Offset, int Length)
            {
                var N = Length;

                do
                {
                    var T = Reader.Invoke(Buffer, Offset + Length - N, N);
                    if (T == 0)
                    {
                        return Length - N;
                    }

                    N -= T;
                } while (N != 0);

                return Length;
            }

            public delegate void WriteCall(byte[] Buffer, int Offset, int Length);
            public delegate int ReadCall(byte[] Buffer, int Offset, int MaxLength);
        }
    }
}

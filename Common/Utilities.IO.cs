using System;
using System.Linq;
using System.Text;

using Media = System.Windows.Media;
using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class IO
        {

            public static bool EnsureExists(string Path)
            {
                if (!SIO.Directory.Exists(Path))
                {
                    SIO.Directory.CreateDirectory(Path);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static string DownloadURL(string URL)
            {
                var Request = System.Net.WebRequest.Create(URL);
                using var Response = Request.GetResponse();
                using var Reader = new SIO.StreamReader(Response.GetResponseStream());
                var Res = Reader.ReadToEnd();

                return Res;
            }

            public static void DownloadURLToFile(string URL, string Path)
            {
                var Request = System.Net.WebRequest.Create(URL);
                using var Response = Request.GetResponse();
                using var WStream = Response.GetResponseStream();
                using var FStream = SIO.File.Open(Path, SIO.FileMode.CreateNew, SIO.FileAccess.Write, SIO.FileShare.Read);
                FStream.Write(WStream);
            }

            /// <summary>
            /// Replaces invalid file name characters with '_'.
            /// </summary>
            /// <param name="Name"></param>
            /// <returns></returns>
            public static string CorrectFileName(string Name)
            {
                var Res = new StringBuilder(Name.Length);
                var Invalids = SIO.Path.GetInvalidFileNameChars();
                foreach (var Ch in Name)
                {
                    if (Invalids.Contains(Ch))
                    {
                        Res.Append('_');
                    }
                    else
                    {
                        Res.Append(Ch);
                    }
                }

                return Res.ToString().Trim();
            }

            public static int ReadAll(ReadCall Reader, byte[] Buffer, int Offset, int Length)
            {
                var N = Length;

                do
                {
                    var T = Reader.Invoke(Buffer, (Offset + Length) - N, N);
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

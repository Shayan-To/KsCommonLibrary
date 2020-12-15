using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class IO
        {
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

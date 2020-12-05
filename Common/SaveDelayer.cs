using System;

namespace Ks
{
    namespace Common
    {
        public class SaveDelayer
        {

            // ToDo Implement IDisposable support.

            public SaveDelayer(System.IO.Stream Stream, System.Text.Encoding Encoding = null)
            {
                this.TaskDelayer = new TaskDelayer(this.DoSave, TimeSpan.FromSeconds(10));

                if (Encoding == null)
                    Encoding = System.Text.Encoding.UTF8;

                this._Stream = Stream;
                this._Encoding = Encoding;
            }

            private void DoSave()
            {
                this.Stream.Position = 0;
                var N = this.Stream.Write(this.InStream, this.Length);
                this.Stream.SetLength(N);
            }

            public void Save(System.IO.Stream Stream, int Length, TaskDelayerRunningMode RunningMode)
            {
                this.InStream = Stream;
                this.Length = Length;
                this.TaskDelayer.RunTask(RunningMode);
            }

            public void Save(System.IO.Stream Stream, TaskDelayerRunningMode RunningMode)
            {
                this.Save(Stream, -1, RunningMode);
            }

            public void Save(byte[] Buffer, int Index, int Length, TaskDelayerRunningMode RunningMode)
            {
                var Strm = new System.IO.MemoryStream(Buffer, Index, Length);
                this.Save(Strm, RunningMode);
            }

            public void Save(byte[] Buffer, TaskDelayerRunningMode RunningMode)
            {
                this.Save(Buffer, 0, Buffer.Length, RunningMode);
            }

            public void Save(string Str, TaskDelayerRunningMode RunningMode)
            {
                this.Save(this.Encoding.GetBytes(Str), RunningMode);
            }

            private readonly System.IO.Stream _Stream;

            public System.IO.Stream Stream
            {
                get
                {
                    return this._Stream;
                }
            }

            private readonly System.Text.Encoding _Encoding;

            public System.Text.Encoding Encoding
            {
                get
                {
                    return this._Encoding;
                }
            }

            private readonly TaskDelayer TaskDelayer;
            private System.IO.Stream InStream;
            private int Length;
        }
    }
}

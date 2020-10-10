using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ks.Common
{
    public class MultiTextWriter : System.IO.TextWriter
    {
        public MultiTextWriter(IEnumerable<System.IO.TextWriter> TextWriters)
        {
            this.Bases = TextWriters.ToArray();
        }

        public MultiTextWriter(params System.IO.TextWriter[] TextWriters) : this((IEnumerable<System.IO.TextWriter>) TextWriters)
        {
        }

        public override void Write(string value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            foreach (var B in this.Bases)
            {
                B.Write(buffer, index, count);
            }
        }

        public override void Write(char value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(char[] buffer)
        {
            foreach (var B in this.Bases)
            {
                B.Write(buffer);
            }
        }

        public override void Write(string format, object arg0)
        {
            foreach (var B in this.Bases)
            {
                B.Write(format, arg0);
            }
        }

        public override void Write(string format, object arg0, object arg1)
        {
            foreach (var B in this.Bases)
            {
                B.Write(format, arg0, arg1);
            }
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            foreach (var B in this.Bases)
            {
                B.Write(format, arg0, arg1, arg2);
            }
        }

        public override void Write(string format, params object[] arg)
        {
            foreach (var B in this.Bases)
            {
                B.Write(format, arg);
            }
        }

        public override void Write(bool value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(decimal value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(double value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(int value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(long value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(object value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(float value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(uint value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void Write(ulong value)
        {
            foreach (var B in this.Bases)
            {
                B.Write(value);
            }
        }

        public override void WriteLine()
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine();
            }
        }

        public override void WriteLine(char[] buffer)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(buffer);
            }
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(buffer, index, count);
            }
        }

        public override void WriteLine(string format, object arg0)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(format, arg0);
            }
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(format, arg0, arg1);
            }
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(format, arg0, arg1, arg2);
            }
        }

        public override void WriteLine(string format, params object[] arg)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(format, arg);
            }
        }

        public override void WriteLine(bool value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(char value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(decimal value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(double value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(int value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(long value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(object value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(float value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(string value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(uint value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override void WriteLine(ulong value)
        {
            foreach (var B in this.Bases)
            {
                B.WriteLine(value);
            }
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            return Task.WhenAll(this.Bases.Select(B => B.WriteAsync(buffer, index, count)));
        }

        public override Task WriteAsync(char value)
        {
            return Task.WhenAll(this.Bases.Select(B => B.WriteAsync(value)));
        }

        public override Task WriteAsync(string value)
        {
            return Task.WhenAll(this.Bases.Select(B => B.WriteAsync(value)));
        }

        public override Task WriteLineAsync()
        {
            return Task.WhenAll(this.Bases.Select(B => B.WriteLineAsync()));
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            return Task.WhenAll(this.Bases.Select(B => B.WriteLineAsync(buffer, index, count)));
        }

        public override Task WriteLineAsync(char value)
        {
            return Task.WhenAll(this.Bases.Select(B => B.WriteLineAsync(value)));
        }

        public override Task WriteLineAsync(string value)
        {
            return Task.WhenAll(this.Bases.Select(B => B.WriteLineAsync(value)));
        }

        public override void Close()
        {
            foreach (var B in this.Bases)
            {
                B.Close();
            }
        }

        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
            throw new NotSupportedException();
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var B in this.Bases)
            {
                B.Dispose();
            }
        }

        public override void Flush()
        {
            foreach (var B in this.Bases)
            {
                B.Flush();
            }
        }

        public override Task FlushAsync()
        {
            return Task.WhenAll(this.Bases.Select(B => B.FlushAsync()));
        }

        public override object InitializeLifetimeService()
        {
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            var Res = new System.Text.StringBuilder();
            Res.Append(nameof(MultiTextWriter)).Append('{');

            var Bl = false;
            foreach (var B in this.Bases)
            {
                if (Bl)
                {
                    Res.Append(',');
                }

                Bl = true;
                Res.Append(B.ToString());
            }

            Res.Append('}');
            return Res.ToString();
        }

        public override System.Text.Encoding Encoding => this.Bases.FirstOrDefault()?.Encoding;

        public override string NewLine
        {
            get => this.Bases.FirstOrDefault()?.NewLine;
            set
            {
                foreach (var B in this.Bases)
                {
                    B.NewLine = value;
                }
            }
        }

        public override IFormatProvider FormatProvider => this.Bases.FirstOrDefault()?.FormatProvider;

        private readonly System.IO.TextWriter[] Bases;
    }
}

using System.Threading.Tasks;
using System.Diagnostics;
using System;

namespace Ks.Common
{
    public class DebugTextWriter : System.IO.TextWriter
    {
        private DebugTextWriter()
        {
        }

        public override void Write(string value)
        {
            Debug.Write(value);
        }

        public override void Write(char[] buffer)
        {
            this.Write(new string(buffer));
        }

        public override void Write(char[] buffer, int index, int count)
        {
            this.Write(new string(buffer, index, count));
        }

        public override void Write(string format, object arg0)
        {
            this.Write(string.Format(this.FormatProvider, format, arg0));
        }

        public override void Write(string format, object arg0, object arg1)
        {
            this.Write(string.Format(this.FormatProvider, format, arg0, arg1));
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            this.Write(string.Format(this.FormatProvider, format, arg0, arg1, arg2));
        }

        public override void Write(string format, params object[] arg)
        {
            this.Write(string.Format(this.FormatProvider, format, arg));
        }

        public override void Write(char value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void Write(bool value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void Write(decimal value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void Write(double value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void Write(int value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void Write(long value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void Write(object value)
        {
            this.Write(string.Format(this.FormatProvider, "{0}", value));
        }

        public override void Write(float value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void Write(uint value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void Write(ulong value)
        {
            this.Write(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }

        public override void WriteLine()
        {
            this.WriteLine("");
        }

        public override void WriteLine(char[] buffer)
        {
            this.WriteLine(new string(buffer));
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            this.WriteLine(new string(buffer, index, count));
        }

        public override void WriteLine(string format, object arg0)
        {
            this.WriteLine(string.Format(this.FormatProvider, format, arg0));
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            this.WriteLine(string.Format(this.FormatProvider, format, arg0, arg1));
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            this.WriteLine(string.Format(this.FormatProvider, format, arg0, arg1, arg2));
        }

        public override void WriteLine(string format, params object[] arg)
        {
            this.WriteLine(string.Format(this.FormatProvider, format, arg));
        }

        public override void WriteLine(bool value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(char value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(decimal value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(double value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(int value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(long value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(object value)
        {
            this.WriteLine(string.Format(this.FormatProvider, "{0}", value));
        }

        public override void WriteLine(float value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(uint value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override void WriteLine(ulong value)
        {
            this.WriteLine(value.ToString(this.FormatProvider));
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            this.Write(buffer, index, count);
            return Task.FromResult<Void>(null);
        }

        public override Task WriteAsync(char value)
        {
            this.Write(value);
            return Task.FromResult<Void>(null);
        }

        public override Task WriteAsync(string value)
        {
            this.Write(value);
            return Task.FromResult<Void>(null);
        }

        public override Task WriteLineAsync()
        {
            this.WriteLine();
            return Task.FromResult<Void>(null);
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            this.WriteLine(buffer, index, count);
            return Task.FromResult<Void>(null);
        }

        public override Task WriteLineAsync(char value)
        {
            this.WriteLine(value);
            return Task.FromResult<Void>(null);
        }

        public override Task WriteLineAsync(string value)
        {
            this.WriteLine(value);
            return Task.FromResult<Void>(null);
        }

        public override void Close()
        {
        }

        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
            return null;
        }

        protected override void Dispose(bool disposing)
        {
        }

        public override void Flush()
        {
        }

        public override Task FlushAsync()
        {
            this.Flush();
            return Task.FromResult<Void>(null);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public override string ToString()
        {
            return nameof(DebugTextWriter);
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return System.Text.Encoding.UTF8;
            }
        }

        public override string NewLine
        {
            get
            {
                return Environment.NewLine;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public override IFormatProvider FormatProvider
        {
            get
            {
                return System.Globalization.CultureInfo.InvariantCulture;
            }
        }

        public static DebugTextWriter Instance { get; } = new DebugTextWriter();
    }
}

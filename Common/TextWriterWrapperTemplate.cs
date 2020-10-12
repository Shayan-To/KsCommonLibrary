using System;
using System.Threading.Tasks;

namespace Ks.Common
{
    public class TextWriterWrapper : System.IO.TextWriter
    {
        public TextWriterWrapper(System.IO.TextWriter TextWriter)
        {
            this.Base = TextWriter;
        }

        public override void Write(string value)
        {
            this.Base.Write(value);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            this.Base.Write(buffer, index, count);
        }

        public override void Write(char value)
        {
            this.Base.Write(value);
        }

        public override void Write(char[] buffer)
        {
            this.Base.Write(buffer);
        }

        public override void Write(string format, object arg0)
        {
            this.Base.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            this.Base.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            this.Base.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            this.Base.Write(format, arg);
        }

        public override void Write(bool value)
        {
            this.Base.Write(value);
        }

        public override void Write(decimal value)
        {
            this.Base.Write(value);
        }

        public override void Write(double value)
        {
            this.Base.Write(value);
        }

        public override void Write(int value)
        {
            this.Base.Write(value);
        }

        public override void Write(long value)
        {
            this.Base.Write(value);
        }

        public override void Write(object value)
        {
            this.Base.Write(value);
        }

        public override void Write(float value)
        {
            this.Base.Write(value);
        }

        public override void Write(uint value)
        {
            this.Base.Write(value);
        }

        public override void Write(ulong value)
        {
            this.Base.Write(value);
        }

        public override void WriteLine()
        {
            this.Base.WriteLine();
        }

        public override void WriteLine(char[] buffer)
        {
            this.Base.WriteLine(buffer);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            this.Base.WriteLine(buffer, index, count);
        }

        public override void WriteLine(string format, object arg0)
        {
            this.Base.WriteLine(format, arg0);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            this.Base.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            this.Base.WriteLine(format, arg0, arg1, arg2);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            this.Base.WriteLine(format, arg);
        }

        public override void WriteLine(bool value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(char value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(decimal value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(string value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            this.Base.WriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            this.Base.WriteLine(value);
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            return this.Base.WriteAsync(buffer, index, count);
        }

        public override Task WriteAsync(char value)
        {
            return this.Base.WriteAsync(value);
        }

        public override Task WriteAsync(string value)
        {
            return this.Base.WriteAsync(value);
        }

        public override Task WriteLineAsync()
        {
            return this.Base.WriteLineAsync();
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            return this.Base.WriteLineAsync(buffer, index, count);
        }

        public override Task WriteLineAsync(char value)
        {
            return this.Base.WriteLineAsync(value);
        }

        public override Task WriteLineAsync(string value)
        {
            return this.Base.WriteLineAsync(value);
        }

        public override void Close()
        {
            this.Base.Close();
        }

        public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
        {
            return this.Base.CreateObjRef(requestedType);
        }

        protected override void Dispose(bool disposing)
        {
            this.Base.Dispose();
        }

        public override void Flush()
        {
            this.Base.Flush();
        }

        public override Task FlushAsync()
        {
            return this.Base.FlushAsync();
        }

        public override object InitializeLifetimeService()
        {
            return this.Base.InitializeLifetimeService();
        }

        public override string ToString()
        {
            return string.Concat(nameof(TextWriterWrapper), "{", this.Base.ToString(), "}");
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return this.Base.Encoding;
            }
        }

        public override string NewLine
        {
            get
            {
                return this.Base.NewLine;
            }
            set
            {
                this.Base.NewLine = value;
            }
        }

        public override IFormatProvider FormatProvider
        {
            get
            {
                return this.Base.FormatProvider;
            }
        }

        private readonly System.IO.TextWriter Base;
    }
}

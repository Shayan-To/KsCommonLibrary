using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System;

namespace Ks
{
    namespace Common
    {
        public class TextWriterTimeStampAdder : System.IO.TextWriter
        {
            public TextWriterTimeStampAdder(System.IO.TextWriter TextWriter, Func<string> GetTimeStampDelegate)
            {
                this.Base = TextWriter;
                this.GetTimeStampDelegate = GetTimeStampDelegate;
            }

            public TextWriterTimeStampAdder(System.IO.TextWriter TextWriter) : this(TextWriter, () => Utilities.Representation.GetTimeStamp())
            {
            }

            private string GetTimeStamp()
            {
                return this.GetTimeStampDelegate.Invoke() + " :: ";
            }

            public override void Write(string value)
            {
                throw new NotSupportedException();
            }

            public override void Write(char[] buffer, int index, int count)
            {
                throw new NotSupportedException();
            }

            public override void Write(char value)
            {
                throw new NotSupportedException();
            }

            public override void Write(char[] buffer)
            {
                throw new NotSupportedException();
            }

            public override void Write(string format, object arg0)
            {
                throw new NotSupportedException();
            }

            public override void Write(string format, object arg0, object arg1)
            {
                throw new NotSupportedException();
            }

            public override void Write(string format, object arg0, object arg1, object arg2)
            {
                throw new NotSupportedException();
            }

            public override void Write(string format, params object[] arg)
            {
                throw new NotSupportedException();
            }

            public override void Write(bool value)
            {
                throw new NotSupportedException();
            }

            public override void Write(decimal value)
            {
                throw new NotSupportedException();
            }

            public override void Write(double value)
            {
                throw new NotSupportedException();
            }

            public override void Write(int value)
            {
                throw new NotSupportedException();
            }

            public override void Write(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(object value)
            {
                throw new NotSupportedException();
            }

            public override void Write(float value)
            {
                throw new NotSupportedException();
            }

            public override void Write(uint value)
            {
                throw new NotSupportedException();
            }

            public override void Write(ulong value)
            {
                throw new NotSupportedException();
            }

            public override void WriteLine(char[] buffer, int index, int count)
            {
                if (buffer == null)
                {
                    this.Base.WriteLine(this.GetTimeStamp());
                    if (this.AutoFlush)
                        this.Base.Flush();
                    return;
                }

                var Stamp = this.GetTimeStamp();
                var StampLength = Stamp.Length;
                this.Base.Write(Stamp);

                var SI = index;
                var Bl = false;
                var loopTo = index + count;
                for (int I = index; I <= loopTo; I++)
                {
                    char Ch = default(char);
                    if (I < (index + count))
                        Ch = buffer[I];
                    else
                        Ch = ControlChars.Lf;
                    if ((Ch == ControlChars.Cr) | (Ch == ControlChars.Lf))
                    {
                        if (Bl)
                            this.Base.Write(new string(' ', StampLength));
                        Bl = true;
                        this.Base.WriteLine(buffer, SI, I - SI);

                        if (I < ((index + count) - 1) && ((Ch == ControlChars.Cr) & (buffer[I + 1] == ControlChars.Lf)))
                            I += 1;
                        SI = I + 1;
                    }
                }

                if (this.AutoFlush)
                    this.Base.Flush();
            }

            public override void WriteLine(object value)
            {
                var Formattable = value as IFormattable;
                if (Formattable != null)
                    this.WriteLine(Formattable.ToString(null, this.Base.FormatProvider));
                else
                    this.WriteLine(value?.ToString());
            }

            public override void WriteLine()
            {
                this.WriteLine(null, 0, 0);
            }

            public override void WriteLine(char[] buffer)
            {
                this.WriteLine(buffer, 0, buffer.Length);
            }

            public override void WriteLine(string value)
            {
                this.WriteLine(value?.ToCharArray(), 0, value?.Length ?? 0);
            }

            public override void WriteLine(string format, object arg0)
            {
                this.WriteLine(string.Format(format, arg0));
            }

            public override void WriteLine(string format, object arg0, object arg1)
            {
                this.WriteLine(string.Format(format, arg0, arg1));
            }

            public override void WriteLine(string format, object arg0, object arg1, object arg2)
            {
                this.WriteLine(string.Format(format, arg0, arg1, arg2));
            }

            public override void WriteLine(string format, params object[] arg)
            {
                this.WriteLine(string.Format(format, arg));
            }

            public override void WriteLine(bool value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override void WriteLine(char value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override void WriteLine(decimal value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override void WriteLine(double value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override void WriteLine(int value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override void WriteLine(long value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override void WriteLine(float value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override void WriteLine(uint value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override void WriteLine(ulong value)
            {
                this.WriteLine(value.ToString(this.Base.FormatProvider));
            }

            public override Task WriteAsync(char[] buffer, int index, int count)
            {
                throw new NotSupportedException();
            }

            public override Task WriteAsync(char value)
            {
                throw new NotSupportedException();
            }

            public override Task WriteAsync(string value)
            {
                throw new NotSupportedException();
            }

            public override async Task WriteLineAsync(char[] buffer, int index, int count)
            {
                if (buffer == null)
                {
                    await this.Base.WriteLineAsync(this.GetTimeStamp());
                    if (this.AutoFlush)
                        await this.Base.FlushAsync();
                    return;
                }

                var Stamp = this.GetTimeStamp();
                var StampLength = Stamp.Length;
                await this.Base.WriteAsync(Stamp);

                var SI = index;
                var Bl = false;
                var loopTo = index + count;
                for (int I = index; I <= loopTo; I++)
                {
                    char Ch = default(char);
                    if (I < (index + count))
                        Ch = buffer[I];
                    else
                        Ch = ControlChars.Lf;
                    if ((Ch == ControlChars.Cr) | (Ch == ControlChars.Lf))
                    {
                        if (Bl)
                            await this.Base.WriteAsync(new string(' ', StampLength));
                        Bl = true;
                        await this.Base.WriteLineAsync(buffer, SI, I - SI);

                        if (I < ((index + count) - 1) && ((Ch == ControlChars.Cr) & (buffer[I + 1] == ControlChars.Lf)))
                            I += 1;
                        SI = I + 1;
                    }
                }

                if (this.AutoFlush)
                    await this.Base.FlushAsync();
            }

            public override async Task WriteLineAsync()
            {
                await this.WriteLineAsync(null, 0, 0);
            }

            public override async Task WriteLineAsync(char value)
            {
                await this.WriteLineAsync(value.ToString(this.Base.FormatProvider));
            }

            public override async Task WriteLineAsync(string value)
            {
                await this.WriteLineAsync(value?.ToCharArray(), 0, value?.Length ?? 0);
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
                return string.Concat(nameof(TextWriterTimeStampAdder), "{", this.Base.ToString(), "}");
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

            private bool _AutoFlush = true;

            public bool AutoFlush
            {
                get
                {
                    return this._AutoFlush;
                }
                set
                {
                    this._AutoFlush = value;
                }
            }

            private readonly System.IO.TextWriter Base;
            private readonly Func<string> GetTimeStampDelegate;
        }
    }
}

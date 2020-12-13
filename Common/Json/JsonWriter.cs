using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common
    {
        public class JsonWriter : IDisposable
        {
            public JsonWriter(System.IO.TextWriter Output, bool LeaveOpen = false)
            {
                this.Out = Output;
                this.LeaveOpen = LeaveOpen;
                this.Reset();
            }

            public JsonWriter(System.Text.StringBuilder StringBuilder) : this(new System.IO.StringWriter(StringBuilder))
            {
            }

            private void WriteEscaped(string S)
            {
                var PrevStart = 0;
                var I = 0;
                var loopTo = S.Length - 1;
                for (I = 0; I <= loopTo; I++)
                {
                    var Ch = S[I];
                    var ECh = Ch;
                    if (EscapeDic.TryGetValue(Ch, out ECh))
                    {
                        this.Out.Write(S.Substring(PrevStart, I - PrevStart));
                        this.Out.Write('\\');
                        this.Out.Write(ECh);
                        PrevStart = I + 1;
                    }
                    else if (char.IsControl(Ch))
                    {
                        this.Out.Write(S.Substring(PrevStart, I - PrevStart));
                        this.Out.Write(@"\u");
                        this.Out.Write(Convert.ToString(Strings.AscW(Ch), 16).PadLeft(4, '0'));
                        PrevStart = I + 1;
                    }
                }
                this.Out.Write(S.Substring(PrevStart, I - PrevStart));
            }

            private void WriteNewLine()
            {
                this.Out.WriteLine();
                var loopTo = this.CurrentIndent - 1;
                for (var I = 0; I <= loopTo; I++)
                    this.Out.Write(this.IndentString);
            }

            private void WriteSeparator(bool NewLineRequired = false)
            {
                if (this.HasValueBefore)
                {
                    this.Out.Write(',');
                    if (!NewLineRequired & !this.MultiLine & this.AddSpaces)
                        this.Out.Write(' ');
                }
                if (this.HasKeyBefore & this.AddSpaces)
                    this.Out.Write(' ');

                if ((this.MultiLine & !this.HasKeyBefore) | NewLineRequired)
                {
                    if ((int)this.State != (int)WriterState.Begin)
                        this.WriteNewLine();
                }
            }

            public void WriteValue(string Value, bool Quoted)
            {
                Verify.False((int)this.State == (int)WriterState.End, "Cannot write after write is finished.");
                Verify.True(((int)this.State == (int)WriterState.Dictionary).Implies(this.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {nameof(this.WriteKey)} instead.");

                this.WriteSeparator();

                if (Quoted)
                {
                    this.Out.Write('"');
                    this.WriteEscaped(Value);
                    this.Out.Write('"');
                }
                else
                    this.Out.Write(Value);

                if ((int)this.State == (int)WriterState.Begin)
                    this.State = WriterState.End;
                this.HasKeyBefore = false;
                this.HasValueBefore = true;
            }

            public void WriteKey(string Name)
            {
                Verify.False((int)this.State == (int)WriterState.End, "Cannot write after write is finished.");
                Verify.True((int)this.State == (int)WriterState.Dictionary, "Cannot write a key outside a dictionary.");
                Verify.False(this.HasKeyBefore, "Cannot write a key immediately after another.");

                this.WriteSeparator();

                this.Out.Write('"');
                this.WriteEscaped(Name);
                this.Out.Write("\":");

                this.HasValueBefore = false;
                this.HasKeyBefore = true;
            }

            public Opening OpenList(bool MultiLine = false)
            {
                Verify.False((int)this.State == (int)WriterState.End, "Cannot write after write is finished.");
                Verify.True(((int)this.State == (int)WriterState.Dictionary).Implies(this.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {nameof(this.WriteKey)} instead.");

                var R = new Opening(this, ']', ((int)this.State == (int)WriterState.Begin) ? WriterState.End : this.State, this.MultiLine);

                this.WriteSeparator(this.OpeningBraceOnNewLine & MultiLine);
                this.Out.Write('[');

                this.MultiLine = MultiLine;
                this.HasValueBefore = false;
                this.HasKeyBefore = false;
                this.State = WriterState.List;

                if (MultiLine)
                    this.CurrentIndent += 1;

                return R;
            }

            public Opening OpenDictionary(bool MultiLine = false)
            {
                Verify.False((int)this.State == (int)WriterState.End, "Cannot write after write is finished.");
                Verify.True(((int)this.State == (int)WriterState.Dictionary).Implies(this.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {nameof(this.WriteKey)} instead.");

                var R = new Opening(this, '}', ((int)this.State == (int)WriterState.Begin) ? WriterState.End : this.State, this.MultiLine);

                this.WriteSeparator(this.OpeningBraceOnNewLine & MultiLine);
                this.Out.Write('{');

                this.MultiLine = MultiLine;
                this.HasValueBefore = false;
                this.HasKeyBefore = false;
                this.State = WriterState.Dictionary;

                if (MultiLine)
                    this.CurrentIndent += 1;

                return R;
            }

            private void CloseOpening(char ClosingChar, WriterState PreviousState, bool PreviousMultiline)
            {
                Verify.False(this.HasKeyBefore, "Cannot close while a key is pending its value.");
                Verify.False((int)this.State == (int)WriterState.End, "Cannot write after write is finished.");

                if (this.MultiLine)
                {
                    this.CurrentIndent -= 1;
                    this.WriteNewLine();
                }

                this.Out.Write(ClosingChar);

                this.MultiLine = PreviousMultiline;
                this.State = PreviousState;
                this.HasValueBefore = true;
            }

            private void Reset()
            {
                this.State = WriterState.Begin;
                this.HasValueBefore = false;
                this.HasKeyBefore = false;
                this.MultiLine = false;
                this.CurrentIndent = 0;
            }

            private static readonly Dictionary<char, char> EscapeDic = new Dictionary<char, char>()
            {
                {
                    '"',
                    '"'
                },
                {
                    '/',
                    '/'
                },
                {
                    '\\',
                    '\\'
                },
                {
                    (char)0x8,
                    'b'
                },
                {
                    (char)0xC,
                    'f'
                },
                {
                    (char)0xA,
                    'n'
                },
                {
                    (char)0xD,
                    'r'
                },
                {
                    (char)0x9,
                    't'
                }
            };

            private readonly System.IO.TextWriter Out;
            private readonly bool LeaveOpen;

            private bool HasValueBefore;
            private bool HasKeyBefore;
            private bool MultiLine;
            private int CurrentIndent;
            private WriterState State = WriterState.Begin;

            protected virtual void Dispose(bool Disposing)
            {
                if (!this.IsDisposed)
                {
                    if (Disposing)
                    {
                        if (!this.LeaveOpen)
                            this.Out.Dispose();
                    }
                }
                this._IsDisposed = true;
            }

            public void Dispose()
            {
                this.Dispose(true);
            }

            private bool _IsDisposed;

            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
            public bool IsDisposed
            {
                get
                {
                    return this._IsDisposed;
                }
            }

            private string _IndentString = "  ";

            public string IndentString
            {
                get
                {
                    return this._IndentString;
                }
                set
                {
                    this._IndentString = value;
                }
            }

            private bool _OpeningBraceOnNewLine = false;

            public bool OpeningBraceOnNewLine
            {
                get
                {
                    return this._OpeningBraceOnNewLine;
                }
                set
                {
                    this._OpeningBraceOnNewLine = value;
                }
            }

            private bool _AddSpaces = true;

            public bool AddSpaces
            {
                get
                {
                    return this._AddSpaces;
                }
                set
                {
                    this._AddSpaces = value;
                }
            }

            public struct Opening : IDisposable
            {
                internal Opening(JsonWriter Writer, char ClosingChar, WriterState PreviousState, bool PreviousMultiline)
                {
                    this.Writer = Writer;
                    this.ClosingChar = ClosingChar;
                    this.PreviousState = PreviousState;
                    this.PreviousMultiline = PreviousMultiline;
                }

                private void Dispose()
                {
                    this.Writer.CloseOpening(this.ClosingChar, this.PreviousState, this.PreviousMultiline);
                }

                public void Close()
                {
                    this.Dispose();
                }

                private readonly char ClosingChar;
                private readonly WriterState PreviousState;
                private readonly bool PreviousMultiline;
                private readonly JsonWriter Writer;
            }

            internal enum WriterState
            {
                Begin,
                Dictionary,
                List,
                End
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Ks.Common
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
            for (; I < S.Length; I++)
            {
                var Ch = S[I];
                if (EscapeDic.TryGetValue(Ch, out var ECh))
                {
                    this.Out.Write(S[PrevStart..I]);
                    this.Out.Write('\\');
                    this.Out.Write(ECh);
                    PrevStart = I + 1;
                }
                else if (char.IsControl(Ch))
                {
                    this.Out.Write(S[PrevStart..I]);
                    this.Out.Write(@"\u");
                    this.Out.Write(Utilities.Math.ConvertToBase(Ch, 16).PadLeft(4, '0'));
                    PrevStart = I + 1;
                }
            }
            this.Out.Write(S[PrevStart..I]);
        }

        private void WriteNewLine()
        {
            this.Out.WriteLine();
            for (var I = 0; I < this.CurrentIndent; I++)
            {
                this.Out.Write(this.IndentString);
            }
        }

        private void WriteSeparator(bool NewLineRequired = false)
        {
            if (this.HasValueBefore)
            {
                this.Out.Write(',');
                if (!NewLineRequired & !this.MultiLine & this.AddSpaces)
                {
                    this.Out.Write(' ');
                }
            }
            if (this.HasKeyBefore & this.AddSpaces)
            {
                this.Out.Write(' ');
            }

            if ((this.MultiLine & !this.HasKeyBefore) | NewLineRequired)
            {
                if (this.State != WriterState.Begin)
                {
                    this.WriteNewLine();
                }
            }
        }

        public void WriteValue(string Value, bool Quoted)
        {
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");
            Verify.True((this.State == WriterState.Dictionary).Implies(this.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {nameof(this.WriteKey)} instead.");

            this.WriteSeparator();

            if (Quoted)
            {
                this.Out.Write('"');
                this.WriteEscaped(Value);
                this.Out.Write('"');
            }
            else
            {
                this.Out.Write(Value);
            }

            if (this.State == WriterState.Begin)
            {
                this.State = WriterState.End;
            }

            this.HasKeyBefore = false;
            this.HasValueBefore = true;
        }

        public void WriteKey(string Name)
        {
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");
            Verify.True(this.State == WriterState.Dictionary, "Cannot write a key outside a dictionary.");
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
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");
            Verify.True((this.State == WriterState.Dictionary).Implies(this.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {nameof(this.WriteKey)} instead.");

            var R = new Opening(this, ']', (this.State == WriterState.Begin) ? WriterState.End : this.State, this.MultiLine);

            this.WriteSeparator(this.OpeningBraceOnNewLine & MultiLine);
            this.Out.Write('[');

            this.MultiLine = MultiLine;
            this.HasValueBefore = false;
            this.HasKeyBefore = false;
            this.State = WriterState.List;

            if (MultiLine)
            {
                this.CurrentIndent += 1;
            }

            return R;
        }

        public Opening OpenDictionary(bool MultiLine = false)
        {
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");
            Verify.True((this.State == WriterState.Dictionary).Implies(this.HasKeyBefore), $"Cannot write a value in place of a key in a dictionary. Use {nameof(this.WriteKey)} instead.");

            var R = new Opening(this, '}', (this.State == WriterState.Begin) ? WriterState.End : this.State, this.MultiLine);

            this.WriteSeparator(this.OpeningBraceOnNewLine & MultiLine);
            this.Out.Write('{');

            this.MultiLine = MultiLine;
            this.HasValueBefore = false;
            this.HasKeyBefore = false;
            this.State = WriterState.Dictionary;

            if (MultiLine)
            {
                this.CurrentIndent += 1;
            }

            return R;
        }

        private void CloseOpening(char ClosingChar, WriterState PreviousState, bool PreviousMultiline)
        {
            Verify.False(this.HasKeyBefore, "Cannot close while a key is pending its value.");
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");

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
                {'"', '"'},
                {'/', '/'},
                {'\\', '\\'},
                {(char)0x08, 'b'},
                {(char)0x0C, 'f'},
                {(char)0x0A, 'n'},
                {(char)0x0D, 'r'},
                {(char)0x09, 't'}
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
                    {
                        this.Out.Dispose();
                    }
                }
            }
            this.IsDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        public bool IsDisposed { get; private set; }

        public string IndentString { get; set; } = "  ";

        public bool OpeningBraceOnNewLine { get; set; } = false;

        public bool AddSpaces { get; set; } = true;

        public struct Opening : IDisposable
        {
            internal Opening(JsonWriter Writer, char ClosingChar, WriterState PreviousState, bool PreviousMultiline)
            {
                this.Writer = Writer;
                this.ClosingChar = ClosingChar;
                this.PreviousState = PreviousState;
                this.PreviousMultiline = PreviousMultiline;
            }

            void IDisposable.Dispose()
            {
                this.Close();
            }

            public void Close()
            {
                this.Writer.CloseOpening(this.ClosingChar, this.PreviousState, this.PreviousMultiline);
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

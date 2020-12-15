using System;
using System.Collections.Generic;

namespace Ks.Common
{
    public class XmlWriter : IDisposable
    {
        public XmlWriter(System.IO.TextWriter Output, bool LeaveOpen = false)
        {
            this.Out = Output;
            this.LeaveOpen = LeaveOpen;
            this.Reset();
        }

        public XmlWriter(System.Text.StringBuilder StringBuilder) : this(new System.IO.StringWriter(StringBuilder))
        {
        }

        private bool NeedsEscaping(string S)
        {
            for (var I = 0; I < S.Length; I++)
            {
                var Ch = S[I];
                if (EscapeDic.ContainsKey(Ch) | char.IsControl(Ch) | (char.IsWhiteSpace(Ch) & (Ch != ' ')))
                {
                    return true;
                }
            }
            return false;
        }

        private void WriteEscaped(string S)
        {
            var PrevStart = 0;
            var I = 0;
            for (; I < S.Length; I++)
            {
                var Ch = S[I];
                if (EscapeDic.TryGetValue(Ch, out var Esc))
                {
                    this.Out.Write(S.Substring(PrevStart, I - PrevStart));
                    this.Out.Write(Esc);
                    PrevStart = I + 1;
                }
                else if (char.IsControl(Ch) | (char.IsWhiteSpace(Ch) & (Ch != ' ')))
                {
                    this.Out.Write(S.Substring(PrevStart, I - PrevStart));
                    this.Out.Write("&#x");
                    this.Out.Write(Utilities.Math.ConvertToBase(Ch, 16));
                    this.Out.Write(";");
                    PrevStart = I + 1;
                }
            }
            this.Out.Write(S.Substring(PrevStart, I - PrevStart));
        }

        private void WriteNewLine(bool IsAttribute, bool AfterAttributes = false)
        {
            Assert.True(AfterAttributes.Implies(IsAttribute));

            this.Out.WriteLine();
            for (var I = 0; I < this.CurrentIndent - 1; I++)
            {
                this.Out.Write(this.IndentString);
            }

            if (IsAttribute)
            {
                if (!AfterAttributes)
                {
                    if (this.AttributeDynamicIndent)
                    {
                        this.Out.Write(new string(' ', this.AttributeIndent));
                    }
                    else
                    {
                        this.Out.Write(this.IndentString);
                    }
                }
            }
            else if (this.CurrentIndent != 0)
            {
                this.Out.Write(this.IndentString);
            }
        }

        private void GetOutOfTag()
        {
            if (this.IsTagOpen)
            {
                if (this.AttributeEachOnNewLine)
                {
                    this.WriteNewLine(true, true);
                }

                this.Out.Write(">");
            }

            this.IsTagOpen = false;
            this.TagJustOpened = false;
        }

        public Opening OpenTag(string Name, bool MultiLine = false, bool MultiLineAttributes = false)
        {
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");
            Verify.True(MultiLineAttributes.Implies(MultiLine), "Cannot have multi-line attributes on a non-multi-line tag.");

            Verify.FalseArg(this.NeedsEscaping(Name), nameof(Name), "Name contains unacceptable characters.");

            var R = new Opening(this, Name, this.TagIdCounter, (this.State == WriterState.Begin) ? WriterState.End : this.State, this.MultiLine, this.TagId);

            this.TagId = this.TagIdCounter;
            this.TagIdCounter += 1;

            if (this.State == WriterState.Begin)
            {
                if (this.AddXmlDeclaration)
                {
                    this.Out.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
                    this.WriteNewLine(false);
                }
            }

            this.GetOutOfTag();

            if (this.MultiLine)
            {
                this.WriteNewLine(false);
            }

            var T = $"<{Name}";
            this.Out.Write(T);

            this.AttributeIndent = T.Length + 1;

            this.MultiLine = MultiLine;
            this.MultiLineAttributes = MultiLineAttributes;

            this.IsTagOpen = true;
            this.TagJustOpened = true;

            this.HasValueBefore = false;
            this.State = WriterState.Document;

            if (MultiLine)
            {
                this.CurrentIndent += 1;
            }

            return R;
        }

        public void WriteAttribute(string Key, string Value)
        {
            Verify.False(this.State == WriterState.Begin, "Cannot write before a tag is opened.");
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");
            Verify.True(this.IsTagOpen, "Cannot write an attribute when the tag is cloned.");

            Verify.FalseArg(this.NeedsEscaping(Key), nameof(Key), "Key contains unacceptable characters.");

            if (this.MultiLineAttributes)
            {
                if (!this.TagJustOpened | this.AttributeEachOnNewLine)
                {
                    this.WriteNewLine(true);
                }
                else
                {
                    this.Out.Write(' ');
                }
            }
            else
            {
                this.Out.Write(' ');
            }

            this.Out.Write(Key);
            this.Out.Write("=\"");
            this.WriteEscaped(Value);
            this.Out.Write('"');

            this.TagJustOpened = false;
        }

        public void WriteValue(string Value)
        {
            Verify.False(this.State == WriterState.Begin, "Cannot write before a tag is opened.");
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");
            Verify.False(this.HasValueBefore, "Cannot write two consecutive values.");

            this.GetOutOfTag();

            if (this.MultiLine)
            {
                this.WriteNewLine(false);
            }

            this.WriteEscaped(Value);

            this.HasValueBefore = true;
        }

        private void CloseOpening(string Name, int TagId, WriterState PreviousState, bool PreviousMultiline, int PreviousTagId)
        {
            Verify.False(this.State == WriterState.Begin, "Cannot write before a tag is opened.");
            Verify.False(this.State == WriterState.End, "Cannot write after write is finished.");
            Verify.True(this.TagId == TagId, $"{this.TagId} {TagId}Invalid closing of tag. Nested structure is not respected.");

            if (this.MultiLine)
            {
                this.CurrentIndent -= 1;
            }

            if (this.IsTagOpen)
            {
                this.Out.Write(" />");
                this.IsTagOpen = false;
            }
            else
            {
                this.GetOutOfTag();

                if (this.MultiLine)
                {
                    this.WriteNewLine(false);
                }

                this.Out.Write($"</{Name}>");
            }

            this.TagId = PreviousTagId;
            this.MultiLine = PreviousMultiline;
            this.State = PreviousState;

            this.HasValueBefore = false;
        }

        private void Reset()
        {
            this.State = WriterState.Begin;

            this.IsTagOpen = false;
            this.TagJustOpened = false;

            this.MultiLine = false;
            this.MultiLineAttributes = false;

            this.CurrentIndent = 0;
            this.AttributeIndent = 0;

            this.HasValueBefore = false;
        }

        private static readonly Dictionary<char, string> EscapeDic = new Dictionary<char, string>()
            {
                {'"', "&quot;"},
                {'\'', "&apos;"},
                {'&', "&amp;"},
                {'<', "&lt;"},
                {'>', "&gt;"}
            };

        private readonly System.IO.TextWriter Out;
        private readonly bool LeaveOpen;

        private int TagId;
        private int TagIdCounter = 0;
        private bool IsTagOpen;
        private bool TagJustOpened;
        private bool MultiLineAttributes;
        private int AttributeIndent;
        private int CurrentIndent;
        private bool MultiLine;
        private bool HasValueBefore;
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

        public bool AttributeDynamicIndent { get; set; } = true;

        public bool AttributeEachOnNewLine { get; } = false;

        public bool AddXmlDeclaration { get; set; } = true;

        public struct Opening : IDisposable
        {
            internal Opening(XmlWriter Writer, string Name, int TagId, WriterState PreviousState, bool PreviousMultiline, int PreviousTagId)
            {
                this.Writer = Writer;
                this.Name = Name;
                this.TagId = TagId;
                this.PreviousState = PreviousState;
                this.PreviousMultiline = PreviousMultiline;
                this.PreviousTagId = PreviousTagId;
            }

            void IDisposable.Dispose()
            {
                this.Close();
            }

            public void Close()
            {
                this.Writer.CloseOpening(this.Name, this.TagId, this.PreviousState, this.PreviousMultiline, this.PreviousTagId);
            }

            private readonly string Name;
            private readonly int TagId;
            private readonly WriterState PreviousState;
            private readonly bool PreviousMultiline;
            private readonly int PreviousTagId;
            private readonly XmlWriter Writer;
        }

        internal enum WriterState
        {
            Begin,
            Document,
            End
        }
    }
}

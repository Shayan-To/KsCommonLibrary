using System;

namespace Ks.Common
{
    internal class CsvParser
    {
        public void ParseCsv(CsvData Csv, string Str, bool HasHeaders = true, char Delimiter = ',', bool NormalizeLineEndings = true)
        {
            this.Str = Str.ToCharArray();
            this.Index = 0;
            this.NormalizeLineEndings = true;
            this.DelimiterChar = Delimiter;
            this.Delimiter = Delimiter.ToString();
            this.Csv = Csv;

            this.ParseCsv(HasHeaders);
        }

        private static void EnsureColumns(CsvData Data, int I)
        {
            while (I >= Data.Columns.Count)
                Data.Columns.Add();
        }

        private void ParseCsv(bool HasHeaders)
        {
            var Res = this.Csv;
            Res.Clear();

            Res.HasHeaders = HasHeaders;
            if (HasHeaders)
            {
                while (true)
                {
                    var T = this.ReadToken();
                    var D = T;
                    var ShouldPeek = true;

                    // If a delimiter happens right after another, then an empty field should be considered between them.
                    if ((T == NewLine) | T == null | (T == Delimiter))
                    {
                        D = "";
                        ShouldPeek = false;
                    }

                    Res.Columns.Add(D);
                    if (ShouldPeek)
                        T = this.PeekToken();
                    if ((T == NewLine) | T == null | (T == Delimiter))
                    {
                        if (ShouldPeek)
                            this.ReadToken();
                        if (T == Delimiter)
                            continue;
                        else
                            break;
                    }

                    throw new ArgumentException("Invalid CSV.");
                }
            }

            CsvEntry Entry = null;
            var IsLastEntryEmpty = false;
            var I = 0;
            var NewEntry = true;

            while (true)
            {
                if (NewEntry)
                {
                    Entry = Res.Entries.Add();
                    I = 0;
                    IsLastEntryEmpty = true;
                    NewEntry = false;
                }

                var T = this.ReadToken();
                var D = T;
                var ShouldPeek = true;

                // The last empty line. We have to remove the last entry from the data.
                if (T == null & IsLastEntryEmpty)
                    break;

                // If a delimiter happens right after another, then an empty field should be considered between them.
                if (T == null | (T == NewLine) | (T == Delimiter))
                {
                    D = "";
                    ShouldPeek = false;
                }

                EnsureColumns(Res, I);
                Entry[I] = D;
                I += 1;
                IsLastEntryEmpty = false;
                if (ShouldPeek)
                    T = this.PeekToken();
                if (T == null)
                    break;
                if ((T == NewLine) | (T == Delimiter))
                {
                    if (ShouldPeek)
                        this.ReadToken();
                    if (T == NewLine)
                        NewEntry = true;
                    continue;
                }

                throw new ArgumentException("Invalid CSV.");
            }

            if (IsLastEntryEmpty)
                Res.Entries.Remove(Res.Entries.Count - 1);
        }

        private bool ReadToken(string T)
        {
            if (this.PeekToken() == T)
            {
                this.ReadToken();
                return true;
            }
            return false;
        }

        private string PeekToken()
        {
            var I = this.Index;
            var R = this.ReadToken();
            this.Index = I;
            return R;
        }

        /// <summary>
        /// Possible Values:
        /// Nothing
        /// NewLine
        /// ","
        /// Field
        /// </summary>
        private string ReadToken()
        {
            if (this.IsFinished())
                return null;

            if (this.ReadChar('\r'))
            {
                this.ReadChar('\n');
                return NewLine;
            }

            if (this.ReadChar('\n'))
                return NewLine;

            if (this.ReadChar(this.DelimiterChar))
                return this.Delimiter;

            var Res = new System.Text.StringBuilder();

            if (this.ReadChar('"'))
            {
                while (true)
                {
                    if (this.IsFinished())
                        throw new ArgumentException("Invalid CSV.");
                    if (this.ReadChar('"'))
                    {
                        if (!this.ReadChar('"'))
                            break;
                        Res.Append('"');
                        continue;
                    }

                    if (this.NormalizeLineEndings && this.ReadChar('\r'))
                    {
                        this.ReadChar('\n');
                        Res.Append(NewLine);
                        continue;
                    }

                    if (this.NormalizeLineEndings && this.ReadChar('\n'))
                    {
                        Res.Append(NewLine);
                        continue;
                    }

                    Res.Append(this.ReadChar());
                }
            }
            else
                while (true)
                {
                    var T = this.PeekChar();
                    if (!T.HasValue | (T == this.DelimiterChar) | (T == '\r') | (T == '\n'))
                        break;
                    Res.Append(this.ReadChar());
                }

            return Res.ToString();
        }

        private bool ReadChar(char Ch)
        {
            if (this.PeekChar() == Ch)
            {
                this.ReadChar();
                return true;
            }
            return false;
        }

        private char? ReadChar()
        {
            if (this.IsFinished())
                return default;
            var R = this.Str[Index];
            Index += 1;
            return R;
        }

        private char? PeekChar()
        {
            if (this.IsFinished())
                return default;
            return this.Str[Index];
        }

        private bool IsFinished()
        {
            return this.Index == this.Str.Length;
        }

        public static CsvParser Instance { get; } = new CsvParser();

        private static string NewLine = "\r\n";

        private string Delimiter;
        private char DelimiterChar;
        private bool NormalizeLineEndings;
        private char[] Str;
        private int Index;
        private CsvData Csv;
    }
}

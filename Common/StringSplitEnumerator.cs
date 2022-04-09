using System;
using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public class StringSplitEnumerator : IEnumerator<string>
    {
        public StringSplitEnumerator(string Str, StringSplitOptions Options, params char[] Chars)
        {
            this.String = Str;
            this._Chars = (char[]) Chars.Clone();
            Array.Sort(this._Chars);
            this.Options = Options;
            this.Reset();
        }

        public StringSplitEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (this.Index == this.String.Length)
            {
                return false;
            }

            var Start = this.Index;
            this.SkipText();
            this.Current = this.String[Start..this.Index];
            this.SkipDelimiter();

            return true;
        }

        public void Reset()
        {
            this.Index = 0;
            this.GiveRest = false;
            if (this.Options == StringSplitOptions.RemoveEmptyEntries)
            {
                this.SkipDelimiter();
            }
        }

        public void UnifyRest()
        {
            this.GiveRest = true;
        }

        private void SkipDelimiter()
        {
            if (this.Options == StringSplitOptions.RemoveEmptyEntries)
            {
                while (this.Index != this.String.Length && Array.BinarySearch(this._Chars, this.String[this.Index]) != -1)
                {
                    this.Index += 1;
                }
            }
        }

        private void SkipText()
        {
            if (this.GiveRest)
            {
                this.Index = this.String.Length;
            }

            while (this.Index != this.String.Length && Array.BinarySearch(this._Chars, this.String[this.Index]) == -1)
            {
                this.Index += 1;
            }
        }

        public void Dispose()
        {
        }

        public string Current { get; private set; }

        object IEnumerator.Current => this.Current;

        public string String { get; }

        private readonly char[] _Chars;

        public char Chars => this._Chars[this.Index];

        public StringSplitOptions Options { get; }

        private int Index;
        private bool GiveRest;
    }
}

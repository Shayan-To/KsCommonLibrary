using System.Collections.Generic;
using System;
using System.Collections;

namespace Ks.Common
{
        public class StringSplitEnumerator : IEnumerator<string>
        {
            public StringSplitEnumerator(string Str, StringSplitOptions Options, params char[] Chars)
            {
                this._String = Str;
                this._Chars = (char[])Chars.Clone();
                Array.Sort(this._Chars);
                this._Options = Options;
                this.Reset();
            }

            public StringSplitEnumerator GetEnumerator()
            {
                return this;
            }

            public bool MoveNext()
            {
                if (this.Index == this._String.Length)
                    return false;

                var Start = this.Index;
                this.SkipText();
                this._Current = this._String.Substring(Start, this.Index - Start);
                this.SkipDelimiter();

                return true;
            }

            public void Reset()
            {
                this.Index = 0;
                this.GiveRest = false;
                if (this._Options == StringSplitOptions.RemoveEmptyEntries)
                    this.SkipDelimiter();
            }

            public void UnifyRest()
            {
                this.GiveRest = true;
            }

            private void SkipDelimiter()
            {
                if (this._Options == StringSplitOptions.RemoveEmptyEntries)
                {
                    while (this.Index != this._String.Length && Array.BinarySearch(this._Chars, this._String[this.Index]) != -1)
                        this.Index += 1;
                }
            }

            private void SkipText()
            {
                if (this.GiveRest)
                    this.Index = this._String.Length;
                while (this.Index != this._String.Length && Array.BinarySearch(this._Chars, this._String[this.Index]) == -1)
                    this.Index += 1;
            }

            public void Dispose()
            {
            }

            private string _Current;

            public string Current
            {
                get
                {
                    return this._Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this._Current;
                }
            }

            private readonly string _String;

            public string String
            {
                get
                {
                    return this._String;
                }
            }

            private readonly char[] _Chars;

            public char Chars
            {
                get
                {
                    return this._Chars[Index];
                }
            }

            private readonly StringSplitOptions _Options;

            public StringSplitOptions Options
            {
                get
                {
                    return this._Options;
                }
            }

            private int Index;
            private bool GiveRest;
        }
    }

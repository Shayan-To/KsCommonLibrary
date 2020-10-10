namespace Ks
{
    namespace Common
    {
        public class WordReader
        {
            public WordReader(System.IO.TextReader TextReader)
            {
                this._TextReader = TextReader;
            }

            public string ReadWord()
            {
                while (true)
                {
                    if (this.Index == this.BufferSize)
                    {
                        this.BufferSize = this.TextReader.Read(this.Buffer, 0, this.Buffer.Length);
                        if (this.BufferSize == 0)
                            return null;
                        this.Index = 0;
                    }

                    if (!char.IsWhiteSpace(this.Buffer[this.Index]))
                        break;
                    this.Index += 1;
                }

                var Res = "";
                var StartIndex = this.Index;
                while (true)
                {
                    if (this.Index == this.BufferSize)
                    {
                        if (StartIndex != this.Index)
                        {
                            Assert.True(StartIndex < this.Index);
                            Res += new string(this.Buffer, StartIndex, this.Index - StartIndex);
                        }

                        this.BufferSize = this.TextReader.Read(this.Buffer, 0, this.Buffer.Length);
                        if (this.BufferSize == 0)
                            break;
                        this.Index = 0;
                    }

                    if (char.IsWhiteSpace(this.Buffer[this.Index]))
                    {
                        Res += new string(this.Buffer, StartIndex, this.Index - StartIndex);
                        break;
                    }

                    this.Index += 1;
                }

                return Res;
            }

            private readonly System.IO.TextReader _TextReader;

            public System.IO.TextReader TextReader
            {
                get
                {
                    return this._TextReader;
                }
            }

            private readonly char[] Buffer = new char[4096];
            private int BufferSize = 0;
            private int Index;
        }
    }
}

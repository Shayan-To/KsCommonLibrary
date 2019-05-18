namespace Ks
{
    namespace Common
    {
        public class CsvData
        {
            public CsvData(int EntriesCapacity)
            {
                this._Columns = new CsvColumnList(this);
                this._Entries = new CsvEntryList(this, EntriesCapacity);
            }

            public CsvData()
            {
                this._Columns = new CsvColumnList(this);
                this._Entries = new CsvEntryList(this);
            }

            public static CsvData Parse(string Str, bool HasHeaders = true, char Delimiter = ',', bool NormalizeLineEndings = true)
            {
                var Res = new CsvData();
                CsvParser.Instance.ParseCsv(Res, Str, HasHeaders, Delimiter, NormalizeLineEndings);
                return Res;
            }

            public void ParseIn(string Str, bool HasHeaders = true, char Delimiter = ',', bool NormalizeLineEndings = true)
            {
                CsvParser.Instance.ParseCsv(this, Str, HasHeaders, Delimiter, NormalizeLineEndings);
            }

            public override string ToString()
            {
                return this.ToString(true);
            }

            private static void WriteField(string Field, bool UseQuotes, System.Text.StringBuilder Out)
            {
                if (Field.StartsWith("\"") | Field.Contains("\r") | Field.Contains("\n"))
                    UseQuotes = true;

                if (!UseQuotes)
                    Out.Append(Field);

                Out.Append('"');

                foreach (var C in Field)
                {
                    if (C == '"')
                        Out.Append("\"\"");
                    else
                        Out.Append(C);
                }

                Out.Append('"');
            }

            public string ToString(bool UseQuotes = true, char Delimiter = ',')
            {
                var Res = new System.Text.StringBuilder();

                if (this.HasHeaders)
                {
                    var Bl = true;
                    foreach (var C in this.Columns)
                    {
                        if (Bl)
                            Bl = false;
                        else
                            Res.Append(Delimiter);
                        WriteField(C.HeaderName, UseQuotes, Res);
                    }
                    Res.AppendLine();
                }

                var ColsCount = this.Columns.Count;

                foreach (var E in this.Entries)
                {
                    var Bl = true;
                    var loopTo = ColsCount - 1;
                    for (int I = 0; I <= loopTo; I++)
                    {
                        if (Bl)
                            Bl = false;
                        else
                            Res.Append(Delimiter);
                        WriteField(E[I], UseQuotes, Res);
                    }
                    Res.AppendLine();
                }

                return Res.ToString();
            }

            public void Clear()
            {
                this.Entries.Clear();
                this.Columns.Clear();
            }

            private bool _HasHeaders = true;

            public bool HasHeaders
            {
                get
                {
                    return this._HasHeaders;
                }
                set
                {
                    if (this._HasHeaders != value)
                    {
                        this._HasHeaders = value;
                        this.Columns.ReportHasHeadersChanged(value);
                    }
                }
            }

            private readonly CsvColumnList _Columns;

            public CsvColumnList Columns
            {
                get
                {
                    return this._Columns;
                }
            }

            private readonly CsvEntryList _Entries;

            public CsvEntryList Entries
            {
                get
                {
                    return this._Entries;
                }
            }
        }
    }
}

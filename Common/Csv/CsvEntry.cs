using System.Collections.Generic;

namespace Ks.Common
{
    public class CsvEntry
    {
        internal CsvEntry(CsvData Parent)
        {
            this._Parent = Parent;
        }

        internal void Detach()
        {
            this.IsDetached = true;
        }

        public string this[int Index]
        {
            get
            {
                Verify.False(this.IsDetached, "Entry is detached.");
                Verify.TrueArg(Index < this._Parent.Columns.Count, "Index was outside range.");
                if (Index >= this.Data.Count)
                {
                    return "";
                }

                return this.Data[Index];
            }
            set
            {
                Verify.False(this.IsDetached, "Entry is detached.");
                Verify.TrueArg(Index < this._Parent.Columns.Count, "Index was outside range.");
                if (value == null)
                {
                    value = "";
                }

                if ((value.Length == 0) & (Index >= this.Data.Count))
                {
                    return;
                }

                while (Index >= this.Data.Count)
                {
                    this.Data.Add("");
                }

                this.Data[Index] = value;
            }
        }

        public string this[string HeaderName]
        {
            get
            {
                return this[this.Parent.Columns[HeaderName].Index];
            }
            set
            {
                this[this.Parent.Columns[HeaderName].Index] = value;
            }
        }

        public string this[CsvColumn Column]
        {
            get
            {
                Verify.TrueArg(Column.Parent == this._Parent, "Column", "Given column is not part of this csv data.");
                return this[Column.Index];
            }
            set
            {
                Verify.TrueArg(Column.Parent == this._Parent, "Column", "Given column is not part of this csv data.");
                this[Column.Index] = value;
            }
        }

        private readonly CsvData _Parent;

        public CsvData Parent
        {
            get
            {
                Verify.False(this.IsDetached, "Entry is detached.");
                return this._Parent;
            }
        }

        internal readonly List<string> Data = new List<string>();
        private bool IsDetached = false;
    }
}

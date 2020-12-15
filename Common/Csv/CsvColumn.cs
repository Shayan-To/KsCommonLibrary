namespace Ks.Common
{
    public class CsvColumn
    {
        internal CsvColumn(CsvData Parent)
        {
            this._Parent = Parent;
            this.Parent.Columns.ReportHeaderNameChanged(this, null, "");
        }

        internal void Detach()
        {
            this.IsDetached = true;
            this.Parent.Columns.ReportHeaderNameChanged(this, this._HeaderName, null);
        }

        internal string _HeaderName = "";

        public string HeaderName
        {
            get
            {
                Verify.False(this.IsDetached, "Header is detached.");
                Verify.True(this.Parent.HasHeaders, "The CSV does not have headers.");
                return this._HeaderName;
            }
            set
            {
                Verify.False(this.IsDetached, "Header is detached.");
                Verify.True(this.Parent.HasHeaders, "The CSV does not have headers.");
                if (value == null)
                {
                    value = "";
                }

                if (this._HeaderName != value)
                {
                    var OldValue = this._HeaderName;
                    this._HeaderName = value;
                    this.Parent.Columns.ReportHeaderNameChanged(this, OldValue, value);
                }
            }
        }

        private int _Index;

        public int Index
        {
            get
            {
                Verify.False(this.IsDetached, "Header is detached.");
                return this._Index;
            }
            internal set => this._Index = value;
        }

        private readonly CsvData _Parent;

        public CsvData Parent
        {
            get
            {
                Verify.False(this.IsDetached, "Header is detached.");
                return this._Parent;
            }
        }

        private bool IsDetached = false;
    }
}

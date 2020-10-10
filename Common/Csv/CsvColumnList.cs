using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class CsvColumnList : IReadOnlyList<CsvColumn>
    {
        internal CsvColumnList(CsvData Parent)
        {
            this.Parent = Parent;
        }

        internal void ReportHeaderNameChanged(CsvColumn Column, string OldName, string NewName)
        {
            if (NewName != null)
            {
                this.Names[NewName].Add(Column);
            }

            if (OldName != null)
            {
                Assert.True(this.Names[OldName].Remove(Column));
            }
        }

        internal void ReportHasHeadersChanged(bool HasHeaders)
        {
            if (HasHeaders)
            {
                this.Names.Clear();
                foreach (var C in this.List)
                {
                    C._HeaderName = "";
                }
            }
        }

        private void UpdateIndexes(int Start, int End = -1)
        {
            if (End == -1)
            {
                End = this.Count;
            }

            for (; Start < End; Start++)
            {
                this.List[Start].Index = Start;
            }
        }

        public CsvColumn Insert(int Index, string HeaderName)
        {
            Verify.True(this.Parent.HasHeaders, "The CSV does not have headers.");

            var R = this.Insert(Index);
            R.HeaderName = HeaderName;
            return R;
        }

        public CsvColumn Insert(int Index)
        {
            var R = new CsvColumn(this.Parent);

            this.List.Insert(Index, R);
            this.UpdateIndexes(Index);

            this.Parent.Entries.ReportColumnInsert(Index);

            return R;
        }

        public void Remove(CsvColumn Column)
        {
            Verify.TrueArg(Column.Parent == this.Parent, "Column", "Given column is not part of this csv data.");
            var Index = Column.Index;

            this.List.RemoveAt(Index);

            Column.Detach();

            this.Parent.Entries.ReportColumnRemove(Index);
            this.UpdateIndexes(Index);
        }

        public void Clear()
        {
            foreach (var C in this.List)
            {
                C.Detach();
            }

            this.List.Clear();
            this.Parent.Entries.ReportColumnsClear();
        }

        public void Move(CsvColumn Column, int NewIndex)
        {
            Verify.TrueArg(Column.Parent == this.Parent, "Column", "Given column is not part of this csv data.");
            var OldIndex = Column.Index;
            Assert.True(this.List[OldIndex] == Column);

            if (OldIndex == NewIndex)
            {
                return;
            }

            this.List.Move(OldIndex, NewIndex);

            var A = NewIndex;
            var B = OldIndex;
            if (A > B)
            {
                var C = A;
                A = B;
                B = C;
            }
            this.UpdateIndexes(A, B + 1);

            this.Parent.Entries.ReportColumnMove(OldIndex, NewIndex);
        }

        public CsvColumn Add()
        {
            return this.Insert(this.Count);
        }

        public CsvColumn Add(string HeaderName)
        {
            return this.Insert(this.Count, HeaderName);
        }

        public void Remove(int Index)
        {
            this.Remove(this[Index]);
        }

        public void Remove(string HeaderName)
        {
            this.Remove(this[HeaderName]);
        }

        public void Move(int Index, int NewIndex)
        {
            this.Move(this[Index], NewIndex);
        }

        public void Move(string HeaderName, int NewIndex)
        {
            this.Move(this[HeaderName], NewIndex);
        }

        public List<CsvColumn>.Enumerator GetEnumerator()
        {
            return this.List.GetEnumerator();
        }

        IEnumerator<CsvColumn> IEnumerable<CsvColumn>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public CsvColumn this[int Index] => this.List[Index];

        public CsvColumn this[string HeaderName]
        {
            get
            {
                Verify.True(this.Parent.HasHeaders, "The CSV does not have headers.");
                return this.Names[HeaderName].SingleOrDefault();
            }
        }

        public int Count => this.List.Count;

        public CsvData Parent { get; }

        private readonly List<CsvColumn> List = new List<CsvColumn>();
        private readonly MultiDictionary<string, CsvColumn> Names = new MultiDictionary<string, CsvColumn>();
    }
}

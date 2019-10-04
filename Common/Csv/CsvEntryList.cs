using System.Collections.Generic;
using System.Collections;

namespace Ks
{
    namespace Common
    {
        public class CsvEntryList : IReadOnlyList<CsvEntry>
        {
            internal CsvEntryList(CsvData Parent)
            {
                this._Parent = Parent;
                this.List = new List<CsvEntry>();
            }

            internal CsvEntryList(CsvData Parent, int Capacity)
            {
                this._Parent = Parent;
                this.List = new List<CsvEntry>(Capacity);
            }

            internal void ReportColumnInsert(int Index)
            {
                foreach (var E in this.List)
                {
                    if (Index < E.Data.Count)
                        E.Data.Insert(Index, "");
                }
            }

            internal void ReportColumnRemove(int Index)
            {
                foreach (var E in this.List)
                {
                    if (Index < E.Data.Count)
                        E.Data.RemoveAt(Index);
                }
            }

            internal void ReportColumnsClear()
            {
                foreach (var E in this.List)
                    E.Data.Clear();
            }

            internal void ReportColumnMove(int OldIndex, int NewIndex)
            {
                foreach (var E in this.List)
                {
                    if (OldIndex < E.Data.Count)
                    {
                        var T = E.Data[OldIndex];
                        E.Data.RemoveAt(OldIndex);
                        if (NewIndex < E.Data.Count)
                            E.Data.Insert(NewIndex, T);
                        else
                            E[NewIndex] = T;
                    }
                    else if (NewIndex < E.Data.Count)
                        E.Data.Insert(NewIndex, "");
                }
            }

            public List<CsvEntry>.Enumerator GetEnumerator()
            {
                return this.List.GetEnumerator();
            }

            private IEnumerator<CsvEntry> IEnumerable_1_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public CsvEntry Insert(int Index)
            {
                var R = new CsvEntry(this.Parent);
                this.List.Insert(Index, R);
                return R;
            }

            public CsvEntry Add()
            {
                var R = new CsvEntry(this.Parent);
                this.List.Add(R);
                return R;
            }

            public void Remove(int Index)
            {
                this.List[Index].Detach();
                this.List.RemoveAt(Index);
            }

            public void Remove(CsvEntry Entry)
            {
                Verify.TrueArg(Entry.Parent == this.Parent, "Entry", "Given entry is not part of this csv data.");
                Entry.Detach();
                this.List.Remove(Entry);
            }

            public void Move(int Index, int NewIndex)
            {
                this.List.Move(Index, NewIndex);
            }

            public void Move(CsvEntry Entry, int NewIndex)
            {
                this.List.Move(this.List.IndexOf(Entry), NewIndex);
            }

            public void Clear()
            {
                foreach (var E in this.List)
                    E.Detach();
                this.List.Clear();
            }

            public int IndexOf(CsvEntry Entry)
            {
                Verify.TrueArg(Entry.Parent == this.Parent, "Entry", "Given entry is not part of this csv data.");
                return this.List.IndexOf(Entry);
            }

            public bool Contains(CsvEntry Entry)
            {
                Verify.TrueArg(Entry.Parent == this.Parent, "Entry", "Given entry is not part of this csv data.");
                return this.List.Contains(Entry);
            }

            public int Count
            {
                get
                {
                    return this.List.Count;
                }
            }

            public CsvEntry this[int Index]
            {
                get
                {
                    return this.List[Index];
                }
            }

            private readonly CsvData _Parent;

            public CsvData Parent
            {
                get
                {
                    return this._Parent;
                }
            }

            private readonly List<CsvEntry> List;
        }
    }
}

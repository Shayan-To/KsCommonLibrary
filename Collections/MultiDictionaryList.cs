﻿using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks
{
    namespace Common
    {
        public struct MultiDictionaryList<TKey, TValue> : IList<TValue>, IList
        {
            public MultiDictionaryList(MultiDictionary<TKey, TValue> Parent, TKey Key, List<TValue> List)
            {
                this._Parent = Parent;
                this._Key = Key;
                this.List = List;
                this.Version = Parent.Version;
            }

            private void CheckChanges()
            {
                if (this.Version != this.Parent.Version)
                {
                    List<TValue> T = null;
                    if (this.Parent.Dic.TryGetValue(this.Key, out T))
                        this.List = T;
                    else
                        this.List = null;
                    this.Version = this.Parent.Version;
                }
            }

            private void BeforeInp()
            {
                this.CheckChanges();
                if (this.List == null)
                {
                    this.List = new List<TValue>();
                    this.Parent.ReportKeyFilled(this);
                }
            }

            private void AfterOut()
            {
                this.CheckChanges();
                if (this.List.Count == 0)
                {
                    this.List = null;
                    this.Parent.ReportKeyEmpty(this);
                }
            }

            public void Add(TValue item)
            {
                this.CheckChanges();
                this.BeforeInp();
                this.List.Add(item);
            }

            public void Insert(int index, TValue item)
            {
                this.CheckChanges();
                this.BeforeInp();
                this.List.Insert(index, item);
            }

            public void Clear()
            {
                this.CheckChanges();
                if (this.List != null)
                {
                    this.List.Clear();
                    this.AfterOut();
                }
            }

            public void RemoveAt(int index)
            {
                this.CheckChanges();
                if (this.List == null)
                    throw new ArgumentOutOfRangeException(nameof(index));
                this.List.RemoveAt(index);
                this.AfterOut();
            }

            public bool Remove(TValue item)
            {
                this.CheckChanges();
                if (this.List == null)
                    return false;
                var R = this.List.Remove(item);
                this.AfterOut();
                return R;
            }

            public bool Contains(TValue item)
            {
                this.CheckChanges();
                if (this.List == null)
                    return false;
                return this.List.Contains(item);
            }

            public int IndexOf(TValue item)
            {
                this.CheckChanges();
                if (this.List == null)
                    return -1;
                return (this.List ?? EmptyList).IndexOf(item);
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                this.CheckChanges();
                if (this.List != null)
                    this.List.CopyTo(array, arrayIndex);
            }

            void ICollection.CopyTo(Array array, int index)
            {
                this.CheckChanges();
                if (this.List != null)
                    ((IList)this.List).CopyTo(array, index);
            }

            public List<TValue>.Enumerator GetEnumerator()
            {
                this.CheckChanges();
                return (this.List ?? EmptyList).GetEnumerator();
            }

            public TValue this[int index]
            {
                get
                {
                    this.CheckChanges();
                    if (this.List == null)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    return this.List[index];
                }
                set
                {
                    this.CheckChanges();
                    if (this.List == null)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    this.List[index] = value;
                }
            }

            public int Count
            {
                get
                {
                    this.CheckChanges();
                    if (this.List == null)
                        return 0;
                    return this.List.Count;
                }
            }

            // The following methods call the ones from above.

            bool IList.Contains(object value)
            {
                return this.Contains((TValue)value);
            }

            int IList.IndexOf(object value)
            {
                return this.IndexOf((TValue)value);
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            int IList.Add(object value)
            {
                this.Add((TValue)value);
                return this.Count - 1;
            }

            void IList.Insert(int index, object value)
            {
                this.Insert(index, (TValue)value);
            }

            void IList.Remove(object value)
            {
                this.Remove((TValue)value);
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (TValue)value;
                }
            }

            bool IList.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            bool ICollection<TValue>.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            private readonly MultiDictionary<TKey, TValue> _Parent;

            public MultiDictionary<TKey, TValue> Parent
            {
                get
                {
                    return this._Parent;
                }
            }

            private readonly TKey _Key;

            public TKey Key
            {
                get
                {
                    return this._Key;
                }
            }

            private byte Version;
            internal List<TValue> List;
            private static readonly List<TValue> EmptyList = new List<TValue>();
        }
    }
}
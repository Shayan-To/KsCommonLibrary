using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks.Common
{
        public class JsonDynamicList : JsonDynamicBase, IReadOnlyList<JsonDynamicBase>, IList<JsonDynamicBase>, IList
        {
            public void Insert(int index, JsonDynamicBase item)
            {
                this.Base.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                this.Base.RemoveAt(index);
            }

            public void Clear()
            {
                this.Base.Clear();
            }

            public JsonDynamicBase this[int index]
            {
                get
                {
                    return this.Base[index];
                }
                set
                {
                    this.Base[index] = value;
                }
            }

            public int Count
            {
                get
                {
                    return this.Base.Count;
                }
            }

            protected IEnumerator<JsonDynamicBase> _GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator<JsonDynamicBase> IEnumerable<JsonDynamicBase>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public List<JsonDynamicBase>.Enumerator GetEnumerator()
            {
                return this.Base.GetEnumerator();
            }

            public virtual int IndexOf(JsonDynamicBase item)
            {
                for (var I = 0; I < this.Count; I++)
                {
                    if (object.Equals(item, this[I]))
                        return I;
                }
                return -1;
            }

            void ICollection.CopyTo(Array array, int index)
            {
                this.CopyTo(array, index);
            }

            public virtual void CopyTo(JsonDynamicBase[] array, int arrayIndex)
            {
                this.CopyTo((Array)array, arrayIndex);
            }

            protected virtual void CopyTo(Array array, int index)
            {
                Verify.TrueArg(array.Rank == 1, nameof(array), "Array's rank must be 1.");
                Verify.TrueArg((index + this.Count) <= array.Length, nameof(array), "Array does not have enough length to copy the collection.");
                for (var I = 0; I < this.Count; I++)
                {
                    array.SetValue(this[I], index);
                    index += 1;
                }
            }

            public virtual bool Remove(JsonDynamicBase item)
            {
                var I = this.IndexOf(item);
                if (I == -1)
                    return false;

                this.RemoveAt(I);
                return true;
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (JsonDynamicBase)value;
                }
            }

            JsonDynamicBase IReadOnlyList<JsonDynamicBase>.this[int index]
            {
                get
                {
                    return this[index];
                }
            }

            bool IList.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            bool ICollection<JsonDynamicBase>.IsReadOnly
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

            public bool Contains(JsonDynamicBase item)
            {
                return this.IndexOf(item) != -1;
            }

            int IList.Add(object value)
            {
                this.Add((JsonDynamicBase)value);
                return this.Count - 1;
            }

            bool IList.Contains(object value)
            {
                return this.Contains((JsonDynamicBase)value);
            }

            int IList.IndexOf(object value)
            {
                return this.IndexOf((JsonDynamicBase)value);
            }

            void IList.Insert(int index, object value)
            {
                this.Insert(index, (JsonDynamicBase)value);
            }

            void IList.Remove(object value)
            {
                this.Remove((JsonDynamicBase)value);
            }

            public virtual void Add(JsonDynamicBase item)
            {
                this.Insert(this.Count, item);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this._GetEnumerator();
            }

            private readonly List<JsonDynamicBase> Base = new List<JsonDynamicBase>();
        }
    }

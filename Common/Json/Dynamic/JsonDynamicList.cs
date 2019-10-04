using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks
{
    namespace Common
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

            protected IEnumerator<JsonDynamicBase> IEnumerable_1_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public List<JsonDynamicBase>.Enumerator GetEnumerator()
            {
                return this.Base.GetEnumerator();
            }

            public virtual int IndexOf(JsonDynamicBase item)
            {
                var loopTo = this.Count - 1;
                for (int I = 0; I <= loopTo; I++)
                {
                    if (object.Equals(item, this[I]))
                        return I;
                }
                return -1;
            }

            public virtual void CopyTo(JsonDynamicBase[] array, int arrayIndex)
            {
                this.CopyTo((Array)array, arrayIndex);
            }

            protected virtual void CopyTo(Array array, int index)
            {
                Verify.TrueArg(array.Rank == 1, nameof(array), "Array's rank must be 1.");
                Verify.TrueArg((index + this.Count) <= array.Length, nameof(array), "Array does not have enough length to copy the collection.");
                var loopTo = this.Count - 1;
                for (int I = 0; I <= loopTo; I++)
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

            private object IList_Item
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

            private JsonDynamicBase IReadOnlyList_Item
            {
                get
                {
                    return this[index];
                }
            }

            protected virtual bool IList_IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            private bool IList_IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            private object IList_SyncRoot
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            private bool IList_IsSynchronized
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

            private int IList_Add(object value)
            {
                this.Add((JsonDynamicBase)value);
                return this.Count - 1;
            }

            private bool IList_Contains(object value)
            {
                return this.Contains((JsonDynamicBase)value);
            }

            private int IList_IndexOf(object value)
            {
                return this.IndexOf((JsonDynamicBase)value);
            }

            private void IList_Insert(int index, object value)
            {
                this.Insert(index, (JsonDynamicBase)value);
            }

            private void IList_Remove(object value)
            {
                this.Remove((JsonDynamicBase)value);
            }

            public virtual void Add(JsonDynamicBase item)
            {
                this.Insert(this.Count, item);
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.IEnumerable_1_GetEnumerator();
            }

            private readonly List<JsonDynamicBase> Base = new List<JsonDynamicBase>();
        }
    }
}

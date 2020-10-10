using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Specialized;

namespace Ks.Common
{
        public class OneToOneOrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
        {

            // ToDo Get an equality comparer and use it on the dic, and also the list operations.

            public OneToOneOrderedDictionary(Func<TValue, TKey> KeySelector)
            {
                this.KeySelector = KeySelector;
                this._Keys = this._Items.SelectAsList(V => this.KeySelector.Invoke(V));
                this._Values = this._Items.AsReadOnly();
            }

            public virtual ICollection<TValue> Values
            {
                get
                {
                    return this._Values;
                }
            }

            public virtual TValue this[int index]
            {
                get
                {
                    return this._Items[index];
                }
                set
                {
                    var PKey = this._Keys[index];
                    Assert.True(this._Dic.Remove(PKey));

                    this._Dic.Add(this.KeySelector.Invoke(value), value);
                    this._Items[index] = value;
                }
            }

            /// <summary>
        /// Sets or adds the provided value. The value will be added to the end of the collection if not present.
        /// </summary>
        /// <returns>True if the collection was expanded, and false otherwise, when the key was already in the collection.</returns>
            public virtual bool Set(TValue Value)
            {
                var Key = this.KeySelector.Invoke(Value);
                var PValue = default(TValue);

                if (this._Dic.TryGetValue(Key, out PValue))
                {
                    this._Items[this._Items.IndexOf(PValue)] = Value;
                    this._Dic[Key] = Value;
                    return false;
                }

                this._Items.Add(Value);
                this._Dic.Add(Key, Value);
                return true;
            }

            public virtual void Clear()
            {
                this._Items.Clear();
                this._Dic.Clear();
            }

            public virtual void Insert(int Index, TValue Value)
            {
                var Key = this.KeySelector.Invoke(Value);
                this._Dic.Add(Key, Value);
                this._Items.Insert(Index, Value);
            }

            public virtual void RemoveAt(int index)
            {
                var Key = this._Keys[index];
                Assert.True(this._Dic.Remove(Key));
                this._Items.RemoveAt(index);
            }

            public virtual bool RemoveKey(TKey key)
            {
                var Value = default(TValue);
                if (!this._Dic.TryGetValue(key, out Value))
                    return false;

                Assert.True(this._Dic.Remove(key));
                Assert.True(this._Items.Remove(Value));
                return true;
            }

            public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return this._Items.Select(V => new KeyValuePair<TKey, TValue>(this.KeySelector.Invoke(V), V)).GetEnumerator();
            }

            public virtual TValue this[TKey key]
            {
                get
                {
                    return this._Dic[key];
                }
            }

            TValue IDictionary<TKey, TValue>.this[TKey key]
            {
                get
                {
                    return this[key];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            public virtual int Count
            {
                get
                {
                    return this._Items.Count;
                }
            }

            public virtual ICollection<TKey> Keys
            {
                get
                {
                    return this._Keys;
                }
            }

            void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
            {
                throw new NotSupportedException();
            }

            void IOrderedDictionary<TKey, TValue>.Insert(int index, TKey key, TValue value)
            {
                throw new NotSupportedException();
            }

            public virtual bool ContainsKey(TKey key)
            {
                return this._Dic.ContainsKey(key);
            }

            public virtual int IndexOf(TKey key)
            {
                return this._Items.IndexOf(this._Dic[key]);
            }

            public virtual bool TryGetValue(TKey key, out TValue value)
            {
                return this._Dic.TryGetValue(key, out value);
            }

            public void Add(TValue Value)
            {
                this.Insert(this.Count, Value);
            }

            bool IList.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            bool IDictionary.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
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

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            object IOrderedDictionary.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            KeyValuePair<TKey, TValue> IList<KeyValuePair<TKey, TValue>>.this[int index]
            {
                get
                {
                    var Value = this[index];
                    return new KeyValuePair<TKey, TValue>(this.KeySelector.Invoke(Value), Value);
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            int IList.Add(object value)
            {
                throw new NotSupportedException();
            }

            void IList<KeyValuePair<TKey, TValue>>.Insert(int index, KeyValuePair<TKey, TValue> item)
            {
                throw new NotSupportedException();
            }

            void IList.Insert(int index, object value)
            {
                throw new NotSupportedException();
            }

            void IOrderedDictionary.Insert(int index, object key, object value)
            {
                throw new NotSupportedException();
            }

            void IList.Remove(object value)
            {
                this.ICollection_Remove((KeyValuePair<TKey, TValue>)value);
            }

            public bool RemoveValue(TValue Value)
            {
                return this.RemoveKey(this.KeySelector.Invoke(Value));
            }

            private int IList_IndexOf(KeyValuePair<TKey, TValue> item)
            {
                var R = this.IndexOf(item.Key);
                if (R == -1)
                    return -1;
                if (!object.Equals(item.Value, this[R]))
                    return -1;
                return R;
            }
            int IList<KeyValuePair<TKey, TValue>>.IndexOf(KeyValuePair<TKey, TValue> item)
            {
                return this.IList_IndexOf(item);
            }

            int IList.IndexOf(object value)
            {
                return this.IList_IndexOf((KeyValuePair<TKey, TValue>)value);
            }

            IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
            {
                return this.IDictionary_GetEnumerator();
            }

            bool IList.Contains(object value)
            {
                return this.ICollection_Contains((KeyValuePair<TKey, TValue>)value);
            }

            private bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            object IDictionary.this[object key]
            {
                get
                {
                    return this[(TKey)key];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            bool IDictionary.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            bool ICollection.IsSynchronized
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

            void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
            {
                throw new NotSupportedException();
            }

            void IDictionary.Add(object key, object value)
            {
                throw new NotSupportedException();
            }

            private bool ICollection_Remove(KeyValuePair<TKey, TValue> item)
            {
                var V = default(TValue);
                if (!this.TryGetValue(item.Key, out V))
                    return false;
                if (!object.Equals(V, item.Value))
                    return false;
                return this.RemoveKey(item.Key);
            }

            bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
            {
                return this.ICollection_Remove(item);
            }

            bool IDictionary<TKey, TValue>.Remove(TKey key)
            {
                return this.RemoveKey(key);
            }

            void IDictionary.Remove(object key)
            {
                this.RemoveKey((TKey)key);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private bool ICollection_Contains(KeyValuePair<TKey, TValue> item)
            {
                var V = default(TValue);
                if (!this.TryGetValue(item.Key, out V))
                    return false;
                return object.Equals(V, item.Value);
            }

            bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
            {
                return this.ICollection_Contains(item);
            }

            bool IDictionary.Contains(object key)
            {
                return this.ContainsKey((TKey)key);
            }

            ICollection IDictionary.Keys
            {
                get
                {
                    return (ICollection)this.Keys;
                }
            }

            ICollection IDictionary.Values
            {
                get
                {
                    return (ICollection)this.Values;
                }
            }

            public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                Verify.True((arrayIndex + this.Count) <= array.Length, "Array is not large enough.");
                foreach (var I in this)
                {
                    array[arrayIndex] = I;
                    arrayIndex += 1;
                }
            }

            void ICollection.CopyTo(Array array, int index)
            {
                Verify.True((index + this.Count) <= array.GetLength(0), "Array is not large enough.");
                foreach (var I in this)
                {
                    array.SetValue(I, index);
                    index += 1;
                }
            }

            IDictionaryEnumerator IDictionary.GetEnumerator()
            {
                return this.IDictionary_GetEnumerator();
            }

            private IDictionaryEnumerator IDictionary_GetEnumerator()
            {
                return new DictionaryEnumerator<TKey, TValue, IEnumerator<KeyValuePair<TKey, TValue>>>(this.GetEnumerator());
            }

            private readonly Dictionary<TKey, TValue> _Dic = new Dictionary<TKey, TValue>();
            private readonly List<TValue> _Items = new List<TValue>();
            private readonly IList<TKey> _Keys;
            private readonly IList<TValue> _Values;
            private readonly Func<TValue, TKey> KeySelector;
        }
    }

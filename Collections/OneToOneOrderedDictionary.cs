using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Specialized;

namespace Ks
{
    namespace Common
    {
        public class OneToOneOrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
        {

            // ToDo Get an equality comparer and use it on the dic, and also the list operations.

            public OneToOneOrderedDictionary(Func<TValue, TKey> KeySelector)
            {
                this.KeySelector = KeySelector;
            }

            public virtual ICollection<TValue> Values
            {
                get
                {
                    return this._Values;
                }
            }

            public virtual TValue ItemAt
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
                TValue PValue = default(TValue);

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
                TValue Value = default(TValue);
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

            private TValue IDictionary_Item
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

            private void IDictionary_Add(TKey key, TValue value)
            {
                throw new NotSupportedException();
            }

            public void Insert(int index, TKey key, TValue value)
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

            public virtual bool TryGetValue(TKey key, ref TValue value)
            {
                return this._Dic.TryGetValue(key, out value);
            }

            public void Add(TValue Value)
            {
                this.Insert(this.Count, Value);
            }

            private bool IList_IsReadOnly
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

            private object IList_ItemAt
            {
                get
                {
                    return this.IList_1_ItemAt;
                }
                set
                {
                    this.IList_1_ItemAt = (KeyValuePair<TKey, TValue>)value;
                }
            }

            private KeyValuePair<TKey, TValue> IList_1_ItemAt
            {
                get
                {
                    var Value = this.ItemAt;
                    return new KeyValuePair<TKey, TValue>(this.KeySelector.Invoke(Value), Value);
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            private int IList_Add(object value)
            {
                this.ICollection_Add((KeyValuePair<TKey, TValue>)value);
                return this.Count - 1;
            }

            private void IList_Insert(int index, KeyValuePair<TKey, TValue> item)
            {
                this.Insert(index, item.Key, item.Value);
            }

            private void IList_Insert(int index, object value)
            {
                this.IList_Insert(index, (KeyValuePair<TKey, TValue>)value);
            }

            private void IOrderedDictionary_Insert(int index, object key, object value)
            {
                this.Insert(index, (TKey)key, (TValue)value);
            }

            private void IList_Remove(object value)
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
                if (!object.Equals(item.Value, this.ItemAt))
                    return -1;
                return R;
            }

            private int IList_IndexOf(object value)
            {
                return this.IList_IndexOf((KeyValuePair<TKey, TValue>)value);
            }

            private IDictionaryEnumerator IOrderedDictionary_GetEnumerator()
            {
                return this.IDictionary_GetEnumerator();
            }

            private bool IList_Contains(object value)
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

            private object IDictionary_Item
            {
                get
                {
                    return this.IDictionary_Item;
                }
                set
                {
                    this.IDictionary_Item = (TValue)value;
                }
            }

            private bool IDictionary_IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            private bool ICollection_IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            private object ICollection_SyncRoot
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            private void ICollection_Add(KeyValuePair<TKey, TValue> item)
            {
                this.IDictionary_Add(item.Key, item.Value);
            }

            private void IDictionary_Add(object key, object value)
            {
                this.IDictionary_Add((TKey)key, (TValue)value);
            }

            private bool ICollection_Remove(KeyValuePair<TKey, TValue> item)
            {
                TValue V = default(TValue);
                if (!this.TryGetValue(item.Key, ref V))
                    return false;
                if (!object.Equals(V, item.Value))
                    return false;
                return this.RemoveKey(item.Key);
            }

            private void IDictionary_Remove(object key)
            {
                this.RemoveKey((TKey)key);
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private bool ICollection_Contains(KeyValuePair<TKey, TValue> item)
            {
                TValue V = default(TValue);
                if (!this.TryGetValue(item.Key, ref V))
                    return false;
                return object.Equals(V, item.Value);
            }

            private bool IDictionary_Contains(object key)
            {
                return this.ContainsKey((TKey)key);
            }

            private ICollection IDictionary_Keys
            {
                get
                {
                    return (ICollection)this.Keys;
                }
            }

            private ICollection IDictionary_Values
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

            private void CopyTo(Array array, int index)
            {
                Verify.True((index + this.Count) <= array.GetLength(0), "Array is not large enough.");
                foreach (var I in this)
                {
                    array.SetValue(I, index);
                    index += 1;
                }
            }

            private IDictionaryEnumerator IDictionary_GetEnumerator()
            {
                return new DictionaryEnumerator<TKey, TValue, IEnumerator<KeyValuePair<TKey, TValue>>>(this.GetEnumerator());
            }

            private readonly Dictionary<TKey, TValue> _Dic = new Dictionary<TKey, TValue>();
            private readonly List<TValue> _Items = new List<TValue>();
            private readonly IList<TKey> _Keys = this._Items.SelectAsList(V => this.KeySelector.Invoke(V));
            private readonly IList<TValue> _Values = this._Items.AsReadOnly();
            private readonly Func<TValue, TKey> KeySelector;
        }
    }
}

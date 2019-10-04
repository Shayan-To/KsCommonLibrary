using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks
{
    namespace Common
    {
        public class OneToOneDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
        {
            public OneToOneDictionary(Func<TValue, TKey> KeySelector) : this(new Dictionary<TKey, TValue>(), KeySelector)
            {
            }

            public OneToOneDictionary(IDictionary<TKey, TValue> BaseDictionary, Func<TValue, TKey> KeySelector)
            {
                this.BaseDictionary = BaseDictionary;
                this.KeySelector = KeySelector;
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

            public virtual int Count
            {
                get
                {
                    return this.BaseDictionary.Count;
                }
            }

            public virtual TValue this[TKey key]
            {
                get
                {
                    return this.BaseDictionary[key];
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

            public virtual ICollection<TKey> Keys
            {
                get
                {
                    return this.BaseDictionary.Keys;
                }
            }

            public virtual ICollection<TValue> Values
            {
                get
                {
                    return this.BaseDictionary.Values;
                }
            }

            // ToDo Keys and Values may have not implemented ICollection and cause problems when using IDictionary.

            private void IDictionary_Add(TKey key, TValue value)
            {
                throw new NotSupportedException();
            }

            public virtual void Add(TValue Value)
            {
                this.BaseDictionary.Add(this.KeySelector.Invoke(Value), Value);
            }

            /// <summary>
        /// Sets or adds the provided value.
        /// </summary>
        /// <returns>True if the collection was expanded (the key was not in the collection), and false otherwise.</returns>
            public virtual bool Set(TValue Value)
            {
                var Key = this.KeySelector.Invoke(Value);

                if (this.BaseDictionary.ContainsKey(Key))
                {
                    this.BaseDictionary[Key] = Value;
                    return false;
                }

                this.BaseDictionary.Add(Key, Value);
                return true;
            }

            public virtual void Clear()
            {
                this.BaseDictionary.Clear();
            }

            public virtual bool ContainsKey(TKey key)
            {
                return this.BaseDictionary.ContainsKey(key);
            }

            public virtual bool RemoveKey(TKey key)
            {
                return this.BaseDictionary.Remove(key);
            }

            public virtual bool RemoveValue(TValue Value)
            {
                return this.RemoveKey(this.KeySelector.Invoke(Value));
            }

            public virtual bool TryGetValue(TKey key, ref TValue value)
            {
                return this.BaseDictionary.TryGetValue(key, out value);
            }

            public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return this.BaseDictionary.GetEnumerator();
            }

            private readonly IDictionary<TKey, TValue> BaseDictionary;
            private readonly Func<TValue, TKey> KeySelector;
        }
    }
}

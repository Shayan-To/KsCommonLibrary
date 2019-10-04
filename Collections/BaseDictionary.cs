using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks
{
    namespace Common
    {
        public abstract class BaseDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>, IDictionary
        {
            public abstract int Count { get; private set; }

            protected virtual bool IsReadOnly
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
                    return this[(TKey)key];
                }
                set
                {
                    this[(TKey)key] = (TValue)value;
                }
            }

            private TValue IReadOnlyDictionary_Item
            {
                get
                {
                    return this[key];
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

            private bool IReadOnlyDictionary_TryGetValue(TKey key, out TValue value)
            {
                return this.TryGetValue(key, out value);
            }

            protected void ICollection_Add(KeyValuePair<TKey, TValue> item)
            {
                this.Add(item.Key, item.Value);
            }

            private void IDictionary_Add(object key, object value)
            {
                this.Add((TKey)key, (TValue)value);
            }

            protected bool ICollection_Remove(KeyValuePair<TKey, TValue> item)
            {
                TValue V = default(TValue);
                if (!this.TryGetValue(item.Key, out V))
                    return false;
                if (!object.Equals(V, item.Value))
                    return false;
                return this.Remove(item.Key);
            }

            private void IDictionary_Remove(object key)
            {
                this.Remove((TKey)key);
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.IEnumerator_1_GetEnumerator();
            }

            protected bool ICollection_Contains(KeyValuePair<TKey, TValue> item)
            {
                TValue V = default(TValue);
                if (!this.TryGetValue(item.Key, out V))
                    return false;
                return object.Equals(V, item.Value);
            }

            private bool IDictionary_Contains(object key)
            {
                return this.ContainsKey((TKey)key);
            }

            private bool IReadOnlyDictionary_ContainsKey(TKey key)
            {
                return this.ContainsKey(key);
            }

            public abstract TValue this[TKey key] { get; set; }

            protected virtual ICollection IDictionary_Keys
            {
                get
                {
                    return (ICollection)this.Keys;
                }
            }

            protected virtual ICollection IDictionary_Values
            {
                get
                {
                    return (ICollection)this.Values;
                }
            }

            private IEnumerable<TKey> IReadOnlyDictionary_Keys
            {
                get
                {
                    return this.Keys;
                }
            }

            private IEnumerable<TValue> IReadOnlyDictionary_Values
            {
                get
                {
                    return this.Values;
                }
            }

            public abstract ICollection<TKey> Keys { get; private set; }

            public abstract ICollection<TValue> Values { get; private set; }

            public abstract void Add(TKey key, TValue value);

            public abstract void Clear();

            public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                this.CopyTo((Array)array, arrayIndex);
            }

            protected virtual void CopyTo(Array array, int index)
            {
                Verify.TrueArg(array.Rank == 1, nameof(array), "Array's rank must be 1.");
                Verify.TrueArg((index + this.Count) <= array.Length, nameof(array), "Array does not have enough length to copy the collection.");
                foreach (var I in this)
                {
                    array.SetValue(I, index);
                    index += 1;
                }
            }

            public abstract bool ContainsKey(TKey key);

            public abstract bool Remove(TKey key);

            public abstract bool TryGetValue(TKey key, out TValue value);

            protected virtual IDictionaryEnumerator IDictionary_GetEnumerator()
            {
                return new DictionaryEnumerator<TKey, TValue, IEnumerator<KeyValuePair<TKey, TValue>>>(this.IEnumerator_1_GetEnumerator());
            }

            protected abstract IEnumerator<KeyValuePair<TKey, TValue>> IEnumerator_1_GetEnumerator();
        }
    }
}

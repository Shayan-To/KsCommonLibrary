using System;
using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public abstract class BaseDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>, IDictionary
    {
        public abstract int Count { get; }

        protected virtual bool IsReadOnly
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
                return this.IsReadOnly;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return this.IsReadOnly;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return this[(TKey) key];
            }
            set
            {
                this[(TKey) key] = (TValue) value;
            }
        }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                return this[key];
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

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return this.TryGetValue(key, out value);
        }

        protected void ICollection_Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.ICollection_Add(item);
        }

        void IDictionary.Add(object key, object value)
        {
            this.Add((TKey) key, (TValue) value);
        }

        protected bool ICollection_Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!this.TryGetValue(item.Key, out var V))
            {
                return false;
            }

            if (!object.Equals(V, item.Value))
            {
                return false;
            }

            return this.Remove(item.Key);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.ICollection_Remove(item);
        }

        void IDictionary.Remove(object key)
        {
            this.Remove((TKey) key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._GetEnumerator();
        }

        protected bool ICollection_Contains(KeyValuePair<TKey, TValue> item)
        {
            if (!this.TryGetValue(item.Key, out var V))
            {
                return false;
            }

            return object.Equals(V, item.Value);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.ICollection_Contains(item);
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey((TKey) key);
        }

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
        {
            return this.ContainsKey(key);
        }

        public abstract TValue this[TKey key] { get; set; }

        protected virtual ICollection IDictionary_Keys
        {
            get
            {
                return (ICollection) this.Keys;
            }
        }

        protected virtual ICollection IDictionary_Values
        {
            get
            {
                return (ICollection) this.Values;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return this.IDictionary_Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return this.IDictionary_Values;
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get
            {
                return this.Keys;
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                return this.Values;
            }
        }

        public abstract ICollection<TKey> Keys { get; }

        public abstract ICollection<TValue> Values { get; }

        public abstract void Add(TKey key, TValue value);

        public abstract void Clear();

        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this.CopyTo((Array) array, arrayIndex);
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

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo(array, index);
        }

        public abstract bool ContainsKey(TKey key);

        public abstract bool Remove(TKey key);

        public abstract bool TryGetValue(TKey key, out TValue value);

        protected virtual IDictionaryEnumerator GetDictionaryEnumerator()
        {
            return new DictionaryEnumerator<TKey, TValue, IEnumerator<KeyValuePair<TKey, TValue>>>(this._GetEnumerator());
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this.GetDictionaryEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return this._GetEnumerator();
        }

        protected abstract IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator();
    }
}

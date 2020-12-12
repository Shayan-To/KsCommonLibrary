using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks.Common
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

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
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

        object IDictionary.this[object key]
        {
            get
            {
                return this[(TKey) key];
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

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            var V = default(TValue);
            if (!this.TryGetValue(item.Key, out V))
                return false;
            if (!object.Equals(V, item.Value))
                return false;
            return this.RemoveKey(item.Key);
        }

        void IDictionary.Remove(object key)
        {
            this.RemoveKey((TKey) key);
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            return this.RemoveKey(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            var V = default(TValue);
            if (!this.TryGetValue(item.Key, out V))
                return false;
            return object.Equals(V, item.Value);
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey((TKey) key);
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return (ICollection) this.Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return (ICollection) this.Values;
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

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
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

        public virtual bool TryGetValue(TKey key, out TValue value)
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

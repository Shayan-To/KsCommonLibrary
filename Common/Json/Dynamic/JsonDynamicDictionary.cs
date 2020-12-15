using System;
using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public class JsonDynamicDictionary : JsonDynamicBase, IReadOnlyDictionary<string, JsonDynamicBase>, IDictionary<string, JsonDynamicBase>, IDictionary
    {
        public int Count => this.Base.Count;

        protected virtual bool IsReadOnly => false;

        bool ICollection<KeyValuePair<string, JsonDynamicBase>>.IsReadOnly => this.IsReadOnly;

        bool IDictionary.IsReadOnly => this.IsReadOnly;

        object IDictionary.this[object key]
        {
            get => this[(string) key];
            set => this[(string) key] = (JsonDynamicBase) value;
        }

        JsonDynamicBase IReadOnlyDictionary<string, JsonDynamicBase>.this[string key] => this[key];

        bool IDictionary.IsFixedSize => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => throw new NotSupportedException();

        bool IReadOnlyDictionary<string, JsonDynamicBase>.TryGetValue(string key, out JsonDynamicBase value)
        {
            return this.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<string, JsonDynamicBase>>.Add(KeyValuePair<string, JsonDynamicBase> item)
        {
            this.Add(item.Key, item.Value);
        }

        void IDictionary.Add(object key, object value)
        {
            this.Add((string) key, (JsonDynamicBase) value);
        }

        bool ICollection<KeyValuePair<string, JsonDynamicBase>>.Remove(KeyValuePair<string, JsonDynamicBase> item)
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

        void IDictionary.Remove(object key)
        {
            this.Remove((string) key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        bool ICollection<KeyValuePair<string, JsonDynamicBase>>.Contains(KeyValuePair<string, JsonDynamicBase> item)
        {
            if (!this.TryGetValue(item.Key, out var V))
            {
                return false;
            }

            return object.Equals(V, item.Value);
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey((string) key);
        }

        bool IReadOnlyDictionary<string, JsonDynamicBase>.ContainsKey(string key)
        {
            return this.ContainsKey(key);
        }

        public JsonDynamicBase this[string key]
        {
            get => this.Base[key];
            set => this.Base[key] = value;
        }

        ICollection IDictionary.Keys => (ICollection) this.Keys;

        ICollection IDictionary.Values => (ICollection) this.Values;

        IEnumerable<string> IReadOnlyDictionary<string, JsonDynamicBase>.Keys => this.Keys;

        IEnumerable<JsonDynamicBase> IReadOnlyDictionary<string, JsonDynamicBase>.Values => this.Values;

        public ICollection<string> Keys => this.Base.Keys;

        public ICollection<JsonDynamicBase> Values => this.Base.Values;

        public void Add(string key, JsonDynamicBase value)
        {
            this.Base.Add(key, value);
        }

        public void Clear()
        {
            this.Base.Clear();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo(array, index);
        }

        public virtual void CopyTo(KeyValuePair<string, JsonDynamicBase>[] array, int arrayIndex)
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

        public bool ContainsKey(string key)
        {
            return this.Base.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return this.Base.Remove(key);
        }

        public bool TryGetValue(string key, out JsonDynamicBase value)
        {
            return this.Base.TryGetValue(key, out value);
        }

        protected virtual IDictionaryEnumerator GetDictionaryEnumerator()
        {
            return new DictionaryEnumerator<string, JsonDynamicBase, IEnumerator<KeyValuePair<string, JsonDynamicBase>>>(this.GetEnumerator());
        }

        protected IEnumerator<KeyValuePair<string, JsonDynamicBase>> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this.GetDictionaryEnumerator();
        }

        IEnumerator<KeyValuePair<string, JsonDynamicBase>> IEnumerable<KeyValuePair<string, JsonDynamicBase>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Dictionary<string, JsonDynamicBase>.Enumerator GetEnumerator()
        {
            return this.Base.GetEnumerator();
        }

        private readonly Dictionary<string, JsonDynamicBase> Base = new Dictionary<string, JsonDynamicBase>();
    }
}

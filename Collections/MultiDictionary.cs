using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public class MultiDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, MultiDictionaryList<TKey, TValue>>
    {
        private void IncrementVersion()
        {
            unchecked
            {
                this.Version += 1;
            }
        }

        internal void ReportKeyEmpty(MultiDictionaryList<TKey, TValue> List)
        {
            Assert.True(this.Dic.Remove(List.Key));
            this.IncrementVersion();
        }

        internal void ReportKeyFilled(MultiDictionaryList<TKey, TValue> List)
        {
            this.Dic.Add(List.Key, List.List);
            this.IncrementVersion();
        }

        public void Clear()
        {
            this.Dic.Clear();
            this.IncrementVersion();
        }

        public int Count => this.Dic.Count;

        public MultiDictionaryList<TKey, TValue> this[TKey key]
        {
            get
            {
                if (this.Dic.TryGetValue(key, out var L))
                {
                    return new MultiDictionaryList<TKey, TValue>(this, key, L);
                }

                return new MultiDictionaryList<TKey, TValue>(this, key, null);
            }
        }

        public IEnumerable<TKey> Keys => this.Dic.Keys;

        public IEnumerable<MultiDictionaryList<TKey, TValue>> Values
        {
            get
            {
                foreach (var KV in this.Dic)
                {
                    yield return new MultiDictionaryList<TKey, TValue>(this, KV.Key, KV.Value);
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            return this.Dic.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, MultiDictionaryList<TKey, TValue>>> GetEnumerator()
        {
            foreach (var KV in this.Dic)
            {
                yield return new KeyValuePair<TKey, MultiDictionaryList<TKey, TValue>>(KV.Key, new MultiDictionaryList<TKey, TValue>(this, KV.Key, KV.Value));
            }
        }

        IEnumerator<KeyValuePair<TKey, MultiDictionaryList<TKey, TValue>>> IEnumerable<KeyValuePair<TKey, MultiDictionaryList<TKey, TValue>>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out MultiDictionaryList<TKey, TValue> value)
        {
            if (this.Dic.TryGetValue(key, out var L))
            {
                value = new MultiDictionaryList<TKey, TValue>(this, key, L);
                return true;
            }
            value = default;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal byte Version;
        internal readonly Dictionary<TKey, List<TValue>> Dic = new Dictionary<TKey, List<TValue>>();
    }
}

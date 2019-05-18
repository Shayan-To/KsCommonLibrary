using System.Collections.Generic;
using System.Collections;

namespace Ks
{
    namespace Common
    {
        public class MultiDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, MultiDictionaryList<TKey, TValue>>
        {
            private void IncrementVersion()
            {
                if ((int)this.Version == 255)
                    this.Version = 0;
                else
                    this.Version = System.Convert.ToByte(((int)this.Version + 1));
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

            public int Count
            {
                get
                {
                    return this.Dic.Count;
                }
            }

            public MultiDictionaryList<TKey, TValue> this[TKey key]
            {
                get
                {
                    List<TValue> L = null;
                    if (this.Dic.TryGetValue(key, out L))
                        return new MultiDictionaryList<TKey, TValue>(this, key, L);
                    return new MultiDictionaryList<TKey, TValue>(this, key, null);
                }
            }

            public IEnumerable<TKey> Keys
            {
                get
                {
                    return this.Dic.Keys;
                }
            }

            public IEnumerable<MultiDictionaryList<TKey, TValue>> Values
            {
                get
                {
                    foreach (var KV in this.Dic)
                        yield return new MultiDictionaryList<TKey, TValue>(this, KV.Key, KV.Value);
                }
            }

            public bool ContainsKey(TKey key)
            {
                return this.Dic.ContainsKey(key);
            }

            public IEnumerator<KeyValuePair<TKey, MultiDictionaryList<TKey, TValue>>> GetEnumerator()
            {
                foreach (var KV in this.Dic)
                    yield return new KeyValuePair<TKey, MultiDictionaryList<TKey, TValue>>(KV.Key, new MultiDictionaryList<TKey, TValue>(this, KV.Key, KV.Value));
            }

            private IEnumerator<KeyValuePair<TKey, MultiDictionaryList<TKey, TValue>>> IEnumerable_1_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public bool TryGetValue(TKey key, ref MultiDictionaryList<TKey, TValue> value)
            {
                List<TValue> L = null;
                if (this.Dic.TryGetValue(key, out L))
                {
                    value = new MultiDictionaryList<TKey, TValue>();
                    return true;
                }
                return false;
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            internal byte Version;
            internal readonly Dictionary<TKey, List<TValue>> Dic = new Dictionary<TKey, List<TValue>>();
        }
    }
}

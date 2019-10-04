using System.Collections.Generic;

namespace Ks
{
    namespace Common
    {
        public class NullExpandingDictionary
        {
            public static NullExpandingDictionary<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> Dic)
            {
                return new NullExpandingDictionary<TKey, TValue>(Dic);
            }
        }

        public class NullExpandingDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
        {
            public NullExpandingDictionary(IDictionary<TKey, TValue> Dic)
            {
                this.Dic = Dic;
            }

            public NullExpandingDictionary() : this(new Dictionary<TKey, TValue>())
            {
            }

            public override int Count
            {
                get
                {
                    return this.Dic.Count;
                }
            }

            protected override bool IsReadOnly
            {
                get
                {
                    return this.Dic.IsReadOnly;
                }
            }

            public override TValue this[TKey key]
            {
                get
                {
                    TValue R;
                    if (this.Dic.TryGetValue(key, out R))
                        return R;
                    return default(TValue);
                }
                set
                {
                    this.Dic[key] = value;
                }
            }

            public override ICollection<TKey> Keys
            {
                get
                {
                    return this.Dic.Keys;
                }
            }

            public override ICollection<TValue> Values
            {
                get
                {
                    return this.Dic.Values;
                }
            }

            public override void Add(TKey key, TValue value)
            {
                this.Dic.Add(key, value);
            }

            public override void Clear()
            {
                this.Dic.Clear();
            }

            public override bool ContainsKey(TKey key)
            {
                return this.Dic.ContainsKey(key);
            }

            public override bool Remove(TKey key)
            {
                return this.Dic.Remove(key);
            }

            public override bool TryGetValue(TKey key, ref TValue value)
            {
                value = this[key];
                return true;
            }

            protected override IEnumerator<KeyValuePair<TKey, TValue>> IEnumerator_1_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return this.Dic.GetEnumerator();
            }

            private readonly IDictionary<TKey, TValue> Dic;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Ks.Common
{
    public static class CreateInstanceDictionary
    {

        public static CreateInstanceDictionary<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> Dic, Func<TKey, TValue> Creator)
        {
            return new CreateInstanceDictionary<TKey, TValue>(Dic, Creator);
        }

        public static CreateInstanceDictionary<TKey, TValue> Create<TKey, TValue>(Func<TKey, TValue> Creator)
        {
            return new CreateInstanceDictionary<TKey, TValue>(Creator);
        }

        [Obsolete("Use other overloads.", true)]
        public static CreateInstanceDictionary<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> Dic, Func<TValue> Creator)
        {
            return new CreateInstanceDictionary<TKey, TValue>(Dic, K => Creator.Invoke());
        }

        [Obsolete("Use other overloads.", true)]
        public static CreateInstanceDictionary<TKey, TValue> Create<TKey, TValue>(Func<TValue> Creator)
        {
            return new CreateInstanceDictionary<TKey, TValue>(K => Creator.Invoke());
        }

        public static CreateInstanceDictionary<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> Dic) where TValue : new()
        {
            return new CreateInstanceDictionary<TKey, TValue>(Dic, K => new TValue());
        }

        public static CreateInstanceDictionary<TKey, TValue> Create<TKey, TValue>() where TValue : new()
        {
            return new CreateInstanceDictionary<TKey, TValue>(K => new TValue());
        }
    }

    public class CreateInstanceDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
    {
        public CreateInstanceDictionary(IDictionary<TKey, TValue> Dic, Func<TKey, TValue> Creator)
        {
            this.Dic = Dic;
            this.Creator = Creator;
        }

        public CreateInstanceDictionary(Func<TKey, TValue> Creator) : this(new Dictionary<TKey, TValue>(), Creator)
        {
        }

        public override void Clear()
        {
            this.Dic.Clear();
        }

        public override int Count
        {
            get
            {
                return this.Dic.Count;
            }
        }

        public override void Add(TKey key, TValue value)
        {
            this.Dic.Add(key, value);
        }

        public override bool ContainsKey(TKey key)
        {
            return this.Dic.ContainsKey(key);
        }

        public override TValue this[TKey key]
        {
            get
            {
                if (!this.Dic.TryGetValue(key, out var V))
                {
                    V = this.Creator.Invoke(key);
                    this.Dic.Add(key, V);
                }
                return V;
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

        public override bool Remove(TKey key)
        {
            return this.Dic.Remove(key);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            return this.Dic.TryGetValue(key, out value);
        }

        public override ICollection<TValue> Values
        {
            get
            {
                return this.Dic.Values;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.Dic.GetEnumerator();
        }

        protected override IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private readonly Func<TKey, TValue> Creator;
        private readonly IDictionary<TKey, TValue> Dic;
    }
}

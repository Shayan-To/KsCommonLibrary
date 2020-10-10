using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ks.Common
{
    public class PartialReadOnlyDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
    {
        public PartialReadOnlyDictionary(IDictionary<TKey, TValue> BaseDic)
        {
            this.BaseDic = BaseDic;
        }

        public void LockCurrentElements()
        {
            foreach (var K in this.Keys)
            {
                this.LockedKeys.Add(K);
            }
        }

        public void ResetLock()
        {
            this.LockedKeys.Clear();
        }

        public override int Count
        {
            get
            {
                return this.BaseDic.Count;
            }
        }

        public override TValue this[TKey key]
        {
            get => this.BaseDic[key];
            set
            {
                Verify.False(this.LockedKeys.Contains(key), "Key is locked.");
                this.BaseDic[key] = value;
            }
        }

        public override ICollection<TKey> Keys
        {
            get
            {
                return this.BaseDic.Keys;
            }
        }

        public override ICollection<TValue> Values
        {
            get
            {
                return this.BaseDic.Values;
            }
        }

        public override void Add(TKey key, TValue value)
        {
            this.BaseDic.Add(key, value);
        }

        public override void Clear()
        {
            var CurrentState = this.BaseDic.Where(KV => this.LockedKeys.Contains(KV.Key)).ToArray();
            this.BaseDic.Clear();
            foreach (var KV in CurrentState)
            {
                this.BaseDic.Add(KV.Key, KV.Value);
            }
        }

        public override bool ContainsKey(TKey key)
        {
            return this.BaseDic.ContainsKey(key);
        }

        public override bool Remove(TKey key)
        {
            Verify.False(this.LockedKeys.Contains(key), "Key is locked.");
            return this.BaseDic.Remove(key);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            return this.BaseDic.TryGetValue(key, out value);
        }

        protected override IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.BaseDic.GetEnumerator();
        }

        private readonly IDictionary<TKey, TValue> BaseDic;
        private readonly HashSet<TKey> LockedKeys = new HashSet<TKey>();
    }
}

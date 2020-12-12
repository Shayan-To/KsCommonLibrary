using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks.Common
{
    public class NullAcceptingDictionary
    {
        public static NullAcceptingDictionary<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> Dic) where TKey : class
        {
            return new NullAcceptingDictionary<TKey, TValue>(Dic);
        }
    }

    public class NullAcceptingDictionary<TKey, TValue> : BaseDictionary<TKey, TValue> where TKey : class
    {
        public NullAcceptingDictionary(IDictionary<TKey, TValue> Dic)
        {
            this.Dic = Dic;
            this.KeysCollection = new MergedCollection<TKey>(new TKey[1] { null }, this.Dic.Keys);
            this.ValuesCollection = new MergedCollection<TValue>(this.NullValue, this.Dic.Values);
        }

        public NullAcceptingDictionary() : this(new Dictionary<TKey, TValue>())
        {
        }

        protected override bool IsReadOnly
        {
            get
            {
                return this.Dic.IsReadOnly;
            }
        }

        public override int Count
        {
            get
            {
                if (this.HasNullValue)
                    return this.Dic.Count + 1;
                return this.Dic.Count;
            }
        }

        public override TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    if (!this.HasNullValue)
                        throw new KeyNotFoundException();
                    return this.NullValue[0];
                }
                return this.Dic[key];
            }
            set
            {
                if (key == null)
                {
                    if (this.IsReadOnly)
                        throw new NotSupportedException("Collection is read only.");
                    this.NullValue[0] = value;
                    this.HasNullValue = true;
                    return;
                }
                this.Dic[key] = value;
            }
        }

        public override ICollection<TKey> Keys
        {
            get
            {
                if (!this.HasNullValue)
                    return this.Dic.Keys;
                return this.KeysCollection;
            }
        }

        public override ICollection<TValue> Values
        {
            get
            {
                if (!this.HasNullValue)
                    return this.Dic.Values;
                return this.ValuesCollection;
            }
        }

        protected override void CopyTo(Array Array, int ArrayIndex)
        {
            if (this.HasNullValue)
            {
                Array.SetValue(new KeyValuePair<TKey, TValue>(null, this.NullValue[0]), ArrayIndex);
                ArrayIndex += 1;
            }

            var Dic = this.Dic as IDictionary;
            if (Dic != null)
            {
                Dic.CopyTo(Array, ArrayIndex);
                return;
            }

            foreach (var KV in Dic)
            {
                Array.SetValue(KV, ArrayIndex);
                ArrayIndex += 1;
            }
        }

        public override void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                if (this.IsReadOnly)
                    throw new NotSupportedException();
                this.NullValue[0] = value;
                this.HasNullValue = true;
                return;
            }
            this.Dic.Add(key, value);
        }

        public override void Clear()
        {
            if (this.IsReadOnly)
                throw new NotSupportedException();
            this.NullValue[0] = default;
            this.HasNullValue = false;
            this.Dic.Clear();
        }

        public override bool ContainsKey(TKey key)
        {
            if (key == null)
                return this.HasNullValue;
            return this.Dic.ContainsKey(key);
        }

        public override bool Remove(TKey key)
        {
            if (key == null)
            {
                if (this.IsReadOnly)
                    throw new NotSupportedException();
                if (!this.HasNullValue)
                    return false;
                this.NullValue[0] = default;
                this.HasNullValue = false;
                return true;
            }
            return this.Dic.Remove(key);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                if (this.HasNullValue)
                {
                    value = this.NullValue[0];
                    return true;
                }
                value = default;
                return false;
            }

            return this.Dic.TryGetValue(key, out value);
        }

        private IEnumerator<KeyValuePair<TKey, TValue>> GetEnumeratorWithNull()
        {
            yield return new KeyValuePair<TKey, TValue>(null, this.NullValue[0]);
            foreach (var KV in this.Dic)
                yield return KV;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (this.HasNullValue)
                return this.GetEnumeratorWithNull();
            return this.Dic.GetEnumerator();
        }

        protected override IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private readonly IDictionary<TKey, TValue> Dic;
        private bool HasNullValue;
        private readonly TValue[] NullValue = new TValue[1];

        private readonly MergedCollection<TKey> KeysCollection;
        private readonly MergedCollection<TValue> ValuesCollection;
    }
}

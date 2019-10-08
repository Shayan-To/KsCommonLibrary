using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace Ks
{
    namespace Common
    {
        public class ConcurrentOrderedDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>, IOrderedDictionary<TKey, TValue>
        {
            public ConcurrentOrderedDictionary() : this(new OrderedDictionary<TKey, TValue>())
            {
            }

            public ConcurrentOrderedDictionary(OrderedDictionary<TKey, TValue> BaseDictionary) : this(BaseDictionary, new object())
            {
            }

            public ConcurrentOrderedDictionary(OrderedDictionary<TKey, TValue> BaseDictionary, object LockObject)
            {
                this.BaseDic = BaseDictionary;
                this.LockObject = LockObject;
            }

            public override int Count
            {
                get
                {
                    lock (this.LockObject)
                        return this.BaseDic.Count;
                }
            }

            public override TValue this[TKey key]
            {
                get
                {
                    lock (this.LockObject)
                        return this.BaseDic[key];
                }
                set
                {
                    lock (this.LockObject)
                        this.BaseDic[key] = value;
                }
            }

            public override ICollection<TKey> Keys
            {
                get
                {
                    return (ICollection<TKey>)this.KeysList;
                }
            }

            public override ICollection<TValue> Values
            {
                get
                {
                    return (ICollection<TValue>)this.ValuesList;
                }
            }

            private IReadOnlyList<TKey> _KeysList;

            public IReadOnlyList<TKey> KeysList
            {
                get
                {
                    lock (this.LockObject)
                    {
                        if (this._KeysList == null)
                        {
                            this._KeysList = new ConcurrentList<TKey>((IList<TKey>)this.BaseDic.KeysList, this.LockObject);
                        }
                        return this._KeysList;
                    }
                }
            }

            private IReadOnlyList<TValue> _ValuesList;

            public IReadOnlyList<TValue> ValuesList
            {
                get
                {
                    lock (this.LockObject)
                    {
                        if (this._ValuesList == null)
                        {
                            this._ValuesList = this.KeysList.SelectAsList(K => this[K]);
                        }
                        return this._ValuesList;
                    }
                }
            }

            public KeyValuePair<TKey, TValue> this[int index]
            {
                get
                {
                    lock (this.LockObject)
                        return this.BaseDic[index];
                }
                set
                {
                    lock (this.LockObject)
                        this.BaseDic[index] = value;
                }
            }

            bool IList.IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            public override void Add(TKey key, TValue value)
            {
                lock (this.LockObject)
                    this.BaseDic.Add(key, value);
            }

            public override void Clear()
            {
                lock (this.LockObject)
                    this.BaseDic.Clear();
            }

            public void Insert(int index, TKey key, TValue value)
            {
                lock (this.LockObject)
                    this.BaseDic.Insert(index, key, value);
            }

            public void RemoveAt(int index)
            {
                lock (this.LockObject)
                    this.BaseDic.RemoveAt(index);
            }

            public override bool ContainsKey(TKey key)
            {
                lock (this.LockObject)
                    return this.BaseDic.ContainsKey(key);
            }

            public int IndexOf(TKey key)
            {
                lock (this.LockObject)
                    return this.BaseDic.IndexOf(key);
            }

            public override bool Remove(TKey key)
            {
                lock (this.LockObject)
                    return this.BaseDic.Remove(key);
            }

            public override bool TryGetValue(TKey key, out TValue value)
            {
                lock (this.LockObject)
                    return this.BaseDic.TryGetValue(key, out value);
            }

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                lock (this.LockObject)
                {
                    foreach (var KV in this.BaseDic)
                        yield return KV;
                }
            }

            protected override IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator()
            {
                return this.GetEnumerator();
            }

            object IOrderedDictionary.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (KeyValuePair<TKey, TValue>)value;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = (KeyValuePair<TKey, TValue>)value;
                }
            }

            int IList.Add(object value)
            {
                lock (this.LockObject)
                {
                    this.ICollection_Add((KeyValuePair<TKey, TValue>)value);
                    return this.Count - 1;
                }
            }

            void IList<KeyValuePair<TKey, TValue>>.Insert(int index, KeyValuePair<TKey, TValue> item)
            {
                this.Insert(index, item.Key, item.Value);
            }

            void IList.Insert(int index, object value)
            {
                var kv = (KeyValuePair<TKey, TValue>)value;
                this.Insert(index, kv.Key, kv.Value);
            }

            void IOrderedDictionary.Insert(int index, object key, object value)
            {
                this.Insert(index, (TKey)key, (TValue)value);
            }

            void IList.Remove(object value)
            {
                this.ICollection_Remove((KeyValuePair<TKey, TValue>)value);
            }

            private int IList_IndexOf(KeyValuePair<TKey, TValue> item)
            {
                var R = default(int);
                TValue T;

                lock (this.LockObject)
                {
                    R = this.IndexOf(item.Key);
                    T = this[R].Value;
                }

                if (R == -1)
                    return -1;
                if (!object.Equals(item.Value, T))
                    return -1;
                return R;
            }

            int IList<KeyValuePair<TKey, TValue>>.IndexOf(KeyValuePair<TKey, TValue> item)
            {
                return this.IList_IndexOf(item);
            }

            int IList.IndexOf(object value)
            {
                return this.IList_IndexOf((KeyValuePair<TKey, TValue>)value);
            }

            IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
            {
                return this.GetDictionaryEnumerator();
            }

            bool IList.Contains(object value)
            {
                return this.ICollection_Contains((KeyValuePair<TKey, TValue>)value);
            }

            private readonly OrderedDictionary<TKey, TValue> BaseDic;
            private readonly object LockObject;
        }
    }
}

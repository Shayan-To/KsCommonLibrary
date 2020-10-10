using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace Ks.Common
{
    public class OrderedDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>, IOrderedDictionary<TKey, TValue>
    {

        // ToDo Do the current done changes (in this commit) to the other ordered dictionaries.

        public OrderedDictionary() : this(EqualityComparer<TKey>.Default)
        {
        }

        public OrderedDictionary(IEqualityComparer<TKey> Comparer)
        {
            this.Comparer = Comparer;
            this._Dic = new Dictionary<TKey, TValue>(Comparer);
        }

        public override int Count
        {
            get
            {
                return this._Keys.Count;
            }
        }

        /// <summary>
        /// When setting, adds the value to the end of collection if not present.
        /// </summary>
        public override TValue this[TKey key]
        {
            get
            {
                return this._Dic[key];
            }
            set
            {
                if (!this._Dic.ContainsKey(key))
                {
                    this._Keys.Add(key);
                }

                this._Dic[key] = value;
            }
        }

        public override ICollection<TKey> Keys
        {
            get
            {
                return (ICollection<TKey>) this.KeysList;
            }
        }

        public override ICollection<TValue> Values
        {
            get
            {
                return (ICollection<TValue>) this.ValuesList;
            }
        }

        private IReadOnlyList<TKey> _KeysList;

        public IReadOnlyList<TKey> KeysList
        {
            get
            {
                if (this._KeysList == null)
                {
                    this._KeysList = this._Keys.AsReadOnly();
                }
                return this._KeysList;
            }
        }

        private IReadOnlyList<TValue> _ValuesList;

        public IReadOnlyList<TValue> ValuesList
        {
            get
            {
                if (this._ValuesList == null)
                {
                    this._ValuesList = this._Keys.SelectAsList(K => this._Dic[K]);
                }
                return this._ValuesList;
            }
        }

        public KeyValuePair<TKey, TValue> this[int index]
        {
            get
            {
                var Key = this._Keys[index];
                return new KeyValuePair<TKey, TValue>(Key, this._Dic[Key]);
            }
            set
            {
                var Key = this._Keys[index];

                Assert.True(this._Dic.Remove(Key));

                this._Dic.Add(value.Key, value.Value);
                this._Keys[index] = value.Key;
            }
        }

        object IOrderedDictionary.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (KeyValuePair<TKey, TValue>) value;
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
            this._Keys.Add(key);
            this._Dic.Add(key, value);
        }

        public override void Clear()
        {
            this._Keys.Clear();
            this._Dic.Clear();
        }

        public void Insert(int index, TKey key, TValue value)
        {
            this._Keys.Insert(index, key);
            this._Dic.Add(key, value);
        }

        public void RemoveAt(int index)
        {
            var Key = this._Keys[index];
            this._Keys.RemoveAt(index);
            Assert.True(this._Dic.Remove(Key));
        }

        public override bool ContainsKey(TKey key)
        {
            return this._Dic.ContainsKey(key);
        }

        public int IndexOf(TKey key)
        {
            for (var I = 0; I < this._Keys.Count; I++)
            {
                if (this.Comparer.Equals(key, this._Keys[I]))
                {
                    return I;
                }
            }
            return -1;
        }

        public override bool Remove(TKey key)
        {
            if (!this._Dic.Remove(key))
            {
                return false;
            }

            var I = this.IndexOf(key);
            Assert.True(I != -1);
            this._Keys.RemoveAt(I);
            return true;
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            return this._Dic.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this._Keys.Select(K => new KeyValuePair<TKey, TValue>(K, this._Dic[K])).GetEnumerator();
        }

        protected override IEnumerator<KeyValuePair<TKey, TValue>> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (KeyValuePair<TKey, TValue>) value;
            }
        }

        int IList.Add(object value)
        {
            this.ICollection_Add((KeyValuePair<TKey, TValue>) value);
            return this.Count - 1;
        }

        void IList<KeyValuePair<TKey, TValue>>.Insert(int index, KeyValuePair<TKey, TValue> item)
        {
            this.Insert(index, item.Key, item.Value);
        }

        void IList.Insert(int index, object value)
        {
            var kv = (KeyValuePair<TKey, TValue>) value;
            this.Insert(index, kv.Key, kv.Value);
        }

        void IOrderedDictionary.Insert(int index, object key, object value)
        {
            this.Insert(index, (TKey) key, (TValue) value);
        }

        void IList.Remove(object value)
        {
            this.ICollection_Remove((KeyValuePair<TKey, TValue>) value);
        }

        private int IList_IndexOf(KeyValuePair<TKey, TValue> item)
        {
            var R = this.IndexOf(item.Key);
            if (R == -1)
            {
                return -1;
            }

            if (!object.Equals(item.Value, this[R].Value))
            {
                return -1;
            }

            return R;
        }

        int IList<KeyValuePair<TKey, TValue>>.IndexOf(KeyValuePair<TKey, TValue> item)
        {
            return this.IList_IndexOf(item);
        }

        int IList.IndexOf(object value)
        {
            return this.IList_IndexOf((KeyValuePair<TKey, TValue>) value);
        }

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
        {
            return this.GetDictionaryEnumerator();
        }

        bool IList.Contains(object value)
        {
            return this.ICollection_Contains((KeyValuePair<TKey, TValue>) value);
        }

        private readonly IEqualityComparer<TKey> Comparer;
        private readonly Dictionary<TKey, TValue> _Dic;
        private readonly List<TKey> _Keys = new List<TKey>();
    }
}

using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public struct DictionaryEnumerator<TKey, TValue, T> : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator where T : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        public DictionaryEnumerator(T BaseEnumerator)
        {
            this.BaseEnumerator = BaseEnumerator;
        }

        public KeyValuePair<TKey, TValue> Current
        {
            get
            {
                return this.BaseEnumerator.Current;
            }
        }

        public DictionaryEntry Entry
        {
            get
            {
                var Current = this.Current;
                return new DictionaryEntry(Current.Key, Current.Value);
            }
        }

        public object Key
        {
            get
            {
                return this.Current.Key;
            }
        }

        public object Value
        {
            get
            {
                return this.Current.Value;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public void Dispose()
        {
            this.BaseEnumerator.Dispose();
        }

        public void Reset()
        {
            this.BaseEnumerator.Reset();
        }

        public bool MoveNext()
        {
            return this.BaseEnumerator.MoveNext();
        }

        private readonly T BaseEnumerator;
    }
}

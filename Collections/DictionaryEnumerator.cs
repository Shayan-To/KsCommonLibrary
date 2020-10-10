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

        public KeyValuePair<TKey, TValue> Current => this.BaseEnumerator.Current;

        public DictionaryEntry Entry
        {
            get
            {
                var Current = this.Current;
                return new DictionaryEntry(Current.Key, Current.Value);
            }
        }

        public object Key => this.Current.Key;

        public object Value => this.Current.Value;

        object IEnumerator.Current => this.Current;

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

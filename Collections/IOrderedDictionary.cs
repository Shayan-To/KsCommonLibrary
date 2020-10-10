using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace Ks.Common
{
        public interface IOrderedDictionary<TKey, TValue> : IOrderedDictionary, IList, IList<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IDictionary
        {
            void Insert(int index, TKey key, TValue value);
        }
    }

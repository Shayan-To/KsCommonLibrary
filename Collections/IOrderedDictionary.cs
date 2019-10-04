using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;

namespace Ks
{
    namespace Common
    {
        public interface IOrderedDictionary<TKey, TValue> : IOrderedDictionary, IList, IList<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IDictionary
        {
            new void Insert(int index, TKey key, TValue value);
        }
    }
}

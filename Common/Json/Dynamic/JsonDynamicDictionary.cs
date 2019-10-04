using System.Collections.Generic;
using System.Collections;
using System;

namespace Ks
{
    namespace Common
    {
        public class JsonDynamicDictionary : JsonDynamicBase, IReadOnlyDictionary<string, JsonDynamicBase>, IDictionary<string, JsonDynamicBase>, IDictionary
        {
            public int Count
            {
                get
                {
                    return this.Base.Count;
                }
            }

            protected virtual bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            private object IDictionary_Item
            {
                get
                {
                    return this[(string)key];
                }
                set
                {
                    this[(string)key] = (JsonDynamicBase)value;
                }
            }

            private JsonDynamicBase IReadOnlyDictionary_Item
            {
                get
                {
                    return this[key];
                }
            }

            private bool IDictionary_IsFixedSize
            {
                get
                {
                    return false;
                }
            }

            private bool ICollection_IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            private object ICollection_SyncRoot
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            private bool IReadOnlyDictionary_TryGetValue(string key, ref JsonDynamicBase value)
            {
                return this.TryGetValue(key, ref value);
            }

            protected void ICollection_Add(KeyValuePair<string, JsonDynamicBase> item)
            {
                this.Add(item.Key, item.Value);
            }

            private void IDictionary_Add(object key, object value)
            {
                this.Add((string)key, (JsonDynamicBase)value);
            }

            protected bool ICollection_Remove(KeyValuePair<string, JsonDynamicBase> item)
            {
                JsonDynamicBase V = null;
                if (!this.TryGetValue(item.Key, ref V))
                    return false;
                if (!object.Equals(V, item.Value))
                    return false;
                return this.Remove(item.Key);
            }

            private void IDictionary_Remove(object key)
            {
                this.Remove((string)key);
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            protected bool ICollection_Contains(KeyValuePair<string, JsonDynamicBase> item)
            {
                JsonDynamicBase V = null;
                if (!this.TryGetValue(item.Key, ref V))
                    return false;
                return object.Equals(V, item.Value);
            }

            private bool IDictionary_Contains(object key)
            {
                return this.ContainsKey((string)key);
            }

            private bool IReadOnlyDictionary_ContainsKey(string key)
            {
                return this.ContainsKey(key);
            }

            public JsonDynamicBase this[string key]
            {
                get
                {
                    return this.Base[key];
                }
                set
                {
                    this.Base[key] = value;
                }
            }

            public JsonDynamicValue ItemValue
            {
                get
                {
                    return (JsonDynamicValue)this.Base[key];
                }
                set
                {
                    this.Base[key] = value;
                }
            }

            protected virtual ICollection IDictionary_Keys
            {
                get
                {
                    return (ICollection)this.Keys;
                }
            }

            protected virtual ICollection IDictionary_Values
            {
                get
                {
                    return (ICollection)this.Values;
                }
            }

            private IEnumerable<string> IReadOnlyDictionary_Keys
            {
                get
                {
                    return this.Keys;
                }
            }

            private IEnumerable<JsonDynamicBase> IReadOnlyDictionary_Values
            {
                get
                {
                    return this.Values;
                }
            }

            public ICollection<string> Keys
            {
                get
                {
                    return this.Base.Keys;
                }
            }

            public ICollection<JsonDynamicBase> Values
            {
                get
                {
                    return this.Base.Values;
                }
            }

            public void Add(string key, JsonDynamicBase value)
            {
                this.Base.Add(key, value);
            }

            public void Clear()
            {
                this.Base.Clear();
            }

            public virtual void CopyTo(KeyValuePair<string, JsonDynamicBase>[] array, int arrayIndex)
            {
                this.CopyTo((Array)array, arrayIndex);
            }

            protected virtual void CopyTo(Array array, int index)
            {
                Verify.TrueArg(array.Rank == 1, nameof(array), "Array's rank must be 1.");
                Verify.TrueArg((index + this.Count) <= array.Length, nameof(array), "Array does not have enough length to copy the collection.");
                foreach (var I in this)
                {
                    array.SetValue(I, index);
                    index += 1;
                }
            }

            public bool ContainsKey(string key)
            {
                return this.Base.ContainsKey(key);
            }

            public bool Remove(string key)
            {
                return this.Base.Remove(key);
            }

            public bool TryGetValue(string key, ref JsonDynamicBase value)
            {
                return this.Base.TryGetValue(key, out value);
            }

            protected virtual IDictionaryEnumerator IDictionary_GetEnumerator()
            {
                return new DictionaryEnumerator<string, JsonDynamicBase, IEnumerator<KeyValuePair<string, JsonDynamicBase>>>(this.GetEnumerator());
            }

            protected IEnumerator<KeyValuePair<string, JsonDynamicBase>> IEnumerator_1_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public Dictionary<string, JsonDynamicBase>.Enumerator GetEnumerator()
            {
                return this.Base.GetEnumerator();
            }

            private readonly Dictionary<string, JsonDynamicBase> Base = new Dictionary<string, JsonDynamicBase>();
        }
    }
}

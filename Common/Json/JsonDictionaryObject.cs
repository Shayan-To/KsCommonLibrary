using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Microsoft.VisualBasic.CompilerServices;

namespace Ks
{
    namespace Common
    {
        public class JsonDictionaryObject : JsonObject, IReadOnlyDictionary<string, JsonObject>
        {
            public JsonDictionaryObject(IEnumerable<KeyValuePair<string, JsonObject>> Items)
            {
                this.List = Items.ToArray();
                Array.Sort(this.List, CompareKeyHash);
                var loopTo = this.List.Length - 2;
                for (var I = 0; I <= loopTo; I++)
                    Verify.False(Operators.CompareString(this.List[I].Key, this.List[I + 1].Key, TextCompare: false) == 0, "Cannot have two items with the same key.");
            }

            public int Count
            {
                get
                {
                    return this.List.Length;
                }
            }

            public JsonObject this[string Key]
            {
                get
                {
                    JsonObject Value = null;
                    Verify.True(this.TryGetValue(Key, out Value), "Key not found.");
                    return Value;
                }
            }

            public JsonObject GetItemOrDefault(string Key)
            {
                JsonObject Value = null;
                this.TryGetValue(Key, out Value);
                return Value;
            }

            public IEnumerable<string> Keys
            {
                get
                {
                    return this.List.Select(KV => KV.Key);
                }
            }

            public IEnumerable<JsonObject> Values
            {
                get
                {
                    return this.List.Select(KV => KV.Value);
                }
            }

            public bool ContainsKey(string key)
            {
                JsonObject Value = null;
                return this.TryGetValue(key, out Value);
            }

            public IEnumerator<KeyValuePair<string, JsonObject>> GetEnumerator()
            {
                return ((IReadOnlyList<KeyValuePair<string, JsonObject>>)this.List).GetEnumerator();
            }

            public bool TryGetValue(string Key, out JsonObject Value)
            {
                var T = this.List.BinarySearch(new KeyValuePair<string, JsonObject>(Key, null), CompareKeyHash);
                var loopTo = (T.StartIndex + T.Length) - 1;
                for (var I = T.StartIndex; I <= loopTo; I++)
                {
                    if (Operators.CompareString(this.List[I].Key, Key, TextCompare: false) == 0)
                    {
                        Value = this.List[I].Value;
                        return true;
                    }
                }
                Value = default;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private readonly KeyValuePair<string, JsonObject>[] List;

            private static readonly Comparison<KeyValuePair<string, JsonObject>> CompareKeyHash = (A, B) => A.Key.GetHashCode().CompareTo(B.Key.GetHashCode());
        }
    }
}

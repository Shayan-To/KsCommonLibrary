using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ks.Common
{
    public class JsonDictionaryObject : JsonObject, IReadOnlyDictionary<string, JsonObject>
    {
        public JsonDictionaryObject(IEnumerable<KeyValuePair<string, JsonObject>> Items)
        {
            this.List = Items.ToArray();
            Array.Sort(this.List, CompareKeyHash);
            for (var I = 0; I < this.List.Length - 1; I++)
            {
                Verify.False(this.List[I].Key == this.List[I + 1].Key, "Cannot have two items with the same key.");
            }
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
                Verify.True(this.TryGetValue(Key, out var Value), "Key not found.");
                return Value;
            }
        }

        public JsonObject GetItemOrDefault(string Key)
        {
            this.TryGetValue(Key, out var Value);
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
            return this.TryGetValue(key, out var Value);
        }

        public IEnumerator<KeyValuePair<string, JsonObject>> GetEnumerator()
        {
            return ((IReadOnlyList<KeyValuePair<string, JsonObject>>) this.List).GetEnumerator();
        }

        public bool TryGetValue(string Key, out JsonObject Value)
        {
            var T = this.List.BinarySearch(new KeyValuePair<string, JsonObject>(Key, null), CompareKeyHash);
            for (var I = T.Index; I < T.Index + T.Count; I++)
            {
                if (this.List[I].Key == Key)
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

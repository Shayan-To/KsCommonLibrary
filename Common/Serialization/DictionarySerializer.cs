using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class DictionarySerializer<TKey, TValue> : Serializer<IEnumerable<KeyValuePair<TKey, TValue>>>
    {
        public DictionarySerializer() : base(nameof(Dictionary<object, object>))
        {
        }

        public override void SetT(FormatterSetProxy Formatter, IEnumerable<KeyValuePair<TKey, TValue>> Obj)
        {
            Formatter.Set(nameof(Enumerable.Count), Obj.Count());
            foreach (var I in Obj)
            {
                Formatter.Set(null, I);
            }
        }

        public override IEnumerable<KeyValuePair<TKey, TValue>> GetT(FormatterGetProxy Formatter)
        {
            var Count = default(int);
            Count = Formatter.Get<int>(nameof(Count));

            var R = new Dictionary<TKey, TValue>(Count);
            for (var I = 0; I < Count; I++)
            {
                var T = Formatter.Get<KeyValuePair<TKey, TValue>>(null);
                R.Add(T.Key, T.Value);
            }

            return R;
        }
    }

    public class KeyValuePairSerializer<TKey, TValue> : Serializer<KeyValuePair<TKey, TValue>>
    {
        public KeyValuePairSerializer() : base(nameof(KeyValuePair<object, object>))
        {
        }

        public override void SetT(FormatterSetProxy Formatter, KeyValuePair<TKey, TValue> Obj)
        {
            Formatter.Set(nameof(Obj.Key), Obj.Key);
            Formatter.Set(nameof(Obj.Value), Obj.Value);
        }

        public override KeyValuePair<TKey, TValue> GetT(FormatterGetProxy Formatter)
        {
            var Key = default(TKey);
            Key = Formatter.Get<TKey>(nameof(Key));
            var Value = default(TValue);
            Value = Formatter.Get<TValue>(nameof(Value));

            return new KeyValuePair<TKey, TValue>(Key, Value);
        }
    }
}

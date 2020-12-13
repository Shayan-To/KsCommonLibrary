using System.Linq;
using System.Collections.Generic;

namespace Ks
{
    namespace Common
    {
        public class SerializerCollection : OneToOneOrderedDictionary<string, Serializer>
        {
            public SerializerCollection() : base(S => S.Id)
            {
            }

            internal void LockCurrentElements()
            {
                foreach (var I in this.Values)
                    this.LockedItems.Add(I);
            }

            public override void Clear()
            {
                var State = this.LockedItems.ToArray();
                base.Clear();
                foreach (var I in State)
                    this.Add(I);
            }

            public override void RemoveAt(int index)
            {
                Verify.False(this.LockedItems.Contains(this[index]), "Serializer is locked.");
                base.RemoveAt(index);
            }

            public override bool RemoveKey(string key)
            {
                Verify.False(this.LockedItems.Contains(this[key]), "Serializer is locked.");
                return base.RemoveKey(key);
            }

            public override bool Set(Serializer Value)
            {
                Verify.False(this.LockedItems.Contains(Value), "Serializer is locked.");
                return base.Set(Value);
            }

            public override Serializer this[int index]
            {
                get
                {
                    return base[index];
                }
                set
                {
                    Verify.False(this.LockedItems.Contains(value), "Serializer is locked.");
                    base[index] = value;
                }
            }

            private readonly HashSet<Serializer> LockedItems = new HashSet<Serializer>();
        }
    }
}

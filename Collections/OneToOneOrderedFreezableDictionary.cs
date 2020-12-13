using System;

namespace Ks
{
    namespace Common
    {
        public class OneToOneOrderedFreezableDictionary<TKey, TValue> : OneToOneOrderedDictionary<TKey, TValue>
        {
            public OneToOneOrderedFreezableDictionary(Func<TValue, TKey> KeySelector) : base(KeySelector)
            {
            }


            public void Freeze()
            {
                if (this.FreezeCalled)
                    return;
                this.FreezeCalled = true;
                this.OnFreezing();
                this._IsFreezed = true;
                this.OnFroze();
            }

            protected virtual void OnFreezing()
            {
            }

            protected virtual void OnFroze()
            {
            }

            protected void VerifyWrite()
            {
                Verify.False(this._IsFreezed, "Cannot change a freezed object.");
            }

            private bool _IsFreezed;

            public bool IsFrozen
            {
                get
                {
                    return this._IsFreezed;
                }
            }

            private bool FreezeCalled;

            public override TValue this[int index]
            {
                get
                {
                    return base[index];
                }
                set
                {
                    this.VerifyWrite();
                    base[index] = value;
                }
            }

            public override bool Set(TValue Value)
            {
                this.VerifyWrite();
                return base.Set(Value);
            }

            public override void Clear()
            {
                this.VerifyWrite();
                base.Clear();
            }

            public override void Insert(int Index, TValue Value)
            {
                this.VerifyWrite();
                base.Insert(Index, Value);
            }

            public override void RemoveAt(int index)
            {
                this.VerifyWrite();
                base.RemoveAt(index);
            }

            public override bool RemoveKey(TKey key)
            {
                this.VerifyWrite();
                return base.RemoveKey(key);
            }
        }
    }
}

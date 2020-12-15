using System;

namespace Ks.Common
{
    public class OneToOneFreezableDictionary<TKey, TValue> : OneToOneDictionary<TKey, TValue>
    {
        public OneToOneFreezableDictionary(Func<TValue, TKey> KeySelector) : base(KeySelector)
        {
        }


        public void Freeze()
        {
            if (this.FreezeCalled)
            {
                return;
            }

            this.FreezeCalled = true;
            this.OnFreezing();
            this.IsFrozen = true;
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
            Verify.False(this.IsFrozen, "Cannot change a freezed object.");
        }

        public bool IsFrozen { get; private set; }

        private bool FreezeCalled;

        public override void Add(TValue Value)
        {
            this.VerifyWrite();
            base.Add(Value);
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

        public override bool RemoveKey(TKey key)
        {
            this.VerifyWrite();
            return base.RemoveKey(key);
        }
    }
}

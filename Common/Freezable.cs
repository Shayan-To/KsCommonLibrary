namespace Ks.Common
{
    public class Freezable : MVVM.BindableBase
    {
        public void Freeze()
        {
            if (this.FreezeCalled)
            {
                return;
            }

            this.FreezeCalled = true;

            this.OnFreezing();
            this.IsFrozen = true;
            this.OnFrozen();
        }

        protected virtual void OnFreezing()
        {
        }

        protected virtual void OnFrozen()
        {
        }

        protected void VerifyWrite()
        {
            Verify.False(this.IsFrozen, "Cannot change a frozen object.");
        }

        protected void VerifyFrozen()
        {
            Verify.True(this.IsFrozen, "The object has to be frozen to perform this operation.");
        }

        public bool IsFrozen { get; private set; }

        private bool FreezeCalled;
    }
}

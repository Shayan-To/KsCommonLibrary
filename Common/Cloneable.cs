using System;

namespace Ks.Common
{
    public abstract class Cloneable<T> : ICloneable where T : Cloneable<T>
    {

        public T Clone()
        {
            var c = this.CloneOverride();
            Verify.True(this.GetType() == c.GetType(), "Invalid cloneable implementation.");
            return c;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        protected abstract T CloneOverride();
    }
}

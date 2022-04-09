using System;

namespace Ks.Common
{
    public abstract class Model : Cloneable<Model>, IEquatable<Model>
    {
        public abstract bool Equals(/* [AllowNull] */ Model other);
    }
}

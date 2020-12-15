using System;

namespace Ks.Common
{
    public struct CNullable<T>
    {
        public CNullable(T Value)
        {
            this._Value = Value;
            this.HasValue = true;
        }

        public static implicit operator CNullable<T>(T O)
        {
            return new CNullable<T>(O);
        }

        public static explicit operator T(CNullable<T> O)
        {
            return O.Value;
        }

        private readonly T _Value;

        public T Value
        {
            get
            {
                if (!this.HasValue)
                {
                    throw new NullReferenceException();
                }

                return this._Value;
            }
        }

        public bool HasValue { get; }
    }
}

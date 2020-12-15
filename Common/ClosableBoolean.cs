using System;

namespace Ks.Common
{
    [Obsolete("Cannot be used due to stupidity of the compiler.")]
    public struct ClosableBoolean : IDisposable
    {
        public static implicit operator bool(ClosableBoolean O)
        {
            return O.Value;
        }

        public static implicit operator ClosableBoolean(bool O)
        {
            return new ClosableBoolean() { Value = O };
        }

        public static bool operator !(ClosableBoolean O)
        {
            return !O.Value;
        }

        public static bool operator &(ClosableBoolean O1, bool O2)
        {
            return O1.Value & O2;
        }

        public static bool operator |(ClosableBoolean O1, bool O2)
        {
            return O1.Value | O2;
        }

        public static bool operator ^(ClosableBoolean O1, bool O2)
        {
            return O1.Value ^ O2;
        }

        public static bool operator &(bool O1, ClosableBoolean O2)
        {
            return O1 & O2.Value;
        }

        public static bool operator |(bool O1, ClosableBoolean O2)
        {
            return O1 | O2.Value;
        }

        public static bool operator ^(bool O1, ClosableBoolean O2)
        {
            return O1 ^ O2.Value;
        }

        public static bool operator true(ClosableBoolean O)
        {
            return O.Value;
        }

        public static bool operator false(ClosableBoolean O)
        {
            return !O.Value;
        }

        public void Dispose()
        {
            this.Value = false;
        }

        public bool Value { get; private set; }
    }
}

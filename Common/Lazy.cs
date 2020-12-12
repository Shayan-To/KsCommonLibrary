using System;

namespace Ks.Common
{
    public struct Lazy<TRes>
    {
        public Lazy(Func<TRes> Func)
        {
            this._Func = Func;
            this._Value = default;
            this.ValueCalculated = default;
        }

        public void Reset()
        {
            // Console.WriteLine("Resetting...")
            this.ValueCalculated = false;
        }

        private readonly Func<TRes> _Func;

        public Func<TRes> Func
        {
            get
            {
                return this._Func;
            }
        }

        private TRes _Value;
        private bool ValueCalculated;

        public TRes Value
        {
            get
            {
                if (!this.ValueCalculated)
                {
                    this._Value = this._Func.Invoke();
                    this.ValueCalculated = true;
                }

                return this._Value;
            }
        }
    }
}

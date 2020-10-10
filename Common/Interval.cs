namespace Ks.Common
{
        public struct Interval
        {
            public Interval(double Start, bool IsStartInclusive, double End, bool IsEndInclusive)
            {
                this._Start = Start;
                this._End = End;
                this._IsStartInclusive = IsStartInclusive;
                this._IsEndInclusive = IsEndInclusive;
            }

            public int Compare(double Value)
            {
                if (Value < this.Start)
                    return -1;
                if (this.End < Value)
                    return 1;
                if ((this.Start == Value) & !this.IsStartInclusive)
                    return -1;
                if ((Value == this.End) & !this.IsEndInclusive)
                    return 1;
                return 0;
            }

            private readonly double _Start;

            public double Start
            {
                get
                {
                    return this._Start;
                }
            }

            private readonly double _End;

            public double End
            {
                get
                {
                    return this._End;
                }
            }

            private readonly bool _IsStartInclusive;

            public bool IsStartInclusive
            {
                get
                {
                    return this._IsStartInclusive;
                }
            }

            private readonly bool _IsEndInclusive;

            public bool IsEndInclusive
            {
                get
                {
                    return this._IsEndInclusive;
                }
            }
        }
    }

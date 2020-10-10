namespace Ks.Common
{
    public struct Interval
    {
        public Interval(double Start, bool IsStartInclusive, double End, bool IsEndInclusive)
        {
            this.Start = Start;
            this.End = End;
            this.IsStartInclusive = IsStartInclusive;
            this.IsEndInclusive = IsEndInclusive;
        }

        public int Compare(double Value)
        {
            if (Value < this.Start)
            {
                return -1;
            }

            if (this.End < Value)
            {
                return 1;
            }

            if ((this.Start == Value) & !this.IsStartInclusive)
            {
                return -1;
            }

            if ((Value == this.End) & !this.IsEndInclusive)
            {
                return 1;
            }

            return 0;
        }

        public double Start { get; }

        public double End { get; }

        public bool IsStartInclusive { get; }

        public bool IsEndInclusive { get; }
    }
}

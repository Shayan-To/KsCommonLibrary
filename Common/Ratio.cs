namespace Ks.Common
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct Ratio
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        private Ratio(int Numerator, int Denumenator, Void NoSimplify)
        {
            this.Numerator = Numerator;
            this._Denumenator = Denumenator;
        }

        public Ratio(int Numerator, int Denumenator)
        {
            Verify.True(Denumenator != 0, "Division by zero.");
            var GCD = Utilities.Math.GreatestCommonDivisor(Numerator, Denumenator);
            if (Denumenator < 0)
            {
                Numerator = -Numerator;
                Denumenator = -Denumenator;
            }
            this.Numerator = Numerator / GCD;
            this._Denumenator = Denumenator / GCD;
        }

        public Ratio(int Num) : this(Num, 1, null)
        {
        }

        public static Ratio operator +(Ratio Left, Ratio Right)
        {
            var GCD = Utilities.Math.GreatestCommonDivisor(Left.Denumenator, Right.Denumenator);
            if (GCD == 0)
            {
                if (Left == Zero)
                {
                    return Right;
                }
                else
                {
                    return Left;
                }
            }
            return new Ratio((Left.Numerator * (Right.Denumenator / GCD)) + (Right.Numerator * (Left.Denumenator / GCD)), (Left.Denumenator / GCD) * Right.Denumenator);
        }

        public static Ratio operator -(Ratio Right)
        {
            return new Ratio(-Right.Numerator, Right.Denumenator, null);
        }

        public static Ratio operator -(Ratio Left, Ratio Right)
        {
            return Left + -Right;
        }

        public static Ratio operator *(Ratio Left, Ratio Right)
        {
            var GCD1 = Utilities.Math.GreatestCommonDivisor(Left.Numerator, Right.Denumenator);
            var GCD2 = Utilities.Math.GreatestCommonDivisor(Left.Denumenator, Right.Numerator);
            if ((GCD1 == 0) | (GCD2 == 0))
            {
                return Zero;
            }

            return new Ratio((Left.Numerator / GCD1) * (Right.Numerator / GCD2), (Left.Denumenator / GCD2) * (Right.Denumenator / GCD1), null);
        }

        public static Ratio operator /(Ratio Left, Ratio Right)
        {
            Verify.False(Right.Numerator == 0, "Division by zero.");
            if (Right.Numerator < 0)
            {
                return Left * new Ratio(-Right.Denumenator, -Right.Numerator, null);
            }
            else
            {
                return Left * new Ratio(Right.Denumenator, Right.Numerator, null);
            }
        }

        public static bool operator <(Ratio Left, Ratio Right)
        {
            if ((Left.Denumenator == Zero) | (Right.Denumenator == Zero))
            {
                return Left.Numerator < Right.Numerator;
            }

            return (Left.Numerator * Right.Denumenator) < (Right.Numerator * Left.Denumenator);
        }

        public static bool operator >(Ratio Left, Ratio Right)
        {
            return Right < Left;
        }

        public static bool operator <=(Ratio Left, Ratio Right)
        {
            return !(Right < Left);
        }

        public static bool operator >=(Ratio Left, Ratio Right)
        {
            return !(Left < Right);
        }

        public static bool operator ==(Ratio Left, Ratio Right)
        {
            if ((Left.Denumenator == Zero) | (Right.Denumenator == Zero))
            {
                return Left.Numerator == Right.Numerator;
            }

            return (Left.Numerator == Right.Numerator) & (Right.Denumenator == Left.Denumenator);
        }

        public static bool operator !=(Ratio Left, Ratio Right)
        {
            return !(Left == Right);
        }

        public static Ratio operator +(int Left, Ratio Right)
        {
            return new Ratio(Left) + Right;
        }

        public static Ratio operator -(int Left, Ratio Right)
        {
            return new Ratio(Left) - Right;
        }

        public static Ratio operator *(int Left, Ratio Right)
        {
            return new Ratio(Left) * Right;
        }

        public static Ratio operator /(int Left, Ratio Right)
        {
            return new Ratio(Left) / Right;
        }

        public static bool operator <(int Left, Ratio Right)
        {
            return new Ratio(Left) < Right;
        }

        public static bool operator >(int Left, Ratio Right)
        {
            return new Ratio(Left) > Right;
        }

        public static bool operator <=(int Left, Ratio Right)
        {
            return new Ratio(Left) <= Right;
        }

        public static bool operator >=(int Left, Ratio Right)
        {
            return new Ratio(Left) >= Right;
        }

        public static bool operator ==(int Left, Ratio Right)
        {
            return new Ratio(Left) == Right;
        }

        public static bool operator !=(int Left, Ratio Right)
        {
            return new Ratio(Left) != Right;
        }

        public static Ratio operator +(Ratio Left, int Right)
        {
            return Left + new Ratio(Right);
        }

        public static Ratio operator -(Ratio Left, int Right)
        {
            return Left - new Ratio(Right);
        }

        public static Ratio operator *(Ratio Left, int Right)
        {
            return Left * new Ratio(Right);
        }

        public static Ratio operator /(Ratio Left, int Right)
        {
            return Left / new Ratio(Right);
        }

        public static bool operator <(Ratio Left, int Right)
        {
            return Left < new Ratio(Right);
        }

        public static bool operator >(Ratio Left, int Right)
        {
            return Left > new Ratio(Right);
        }

        public static bool operator <=(Ratio Left, int Right)
        {
            return Left <= new Ratio(Right);
        }

        public static bool operator >=(Ratio Left, int Right)
        {
            return Left >= new Ratio(Right);
        }

        public static bool operator ==(Ratio Left, int Right)
        {
            return Left == new Ratio(Right);
        }

        public static bool operator !=(Ratio Left, int Right)
        {
            return Left != new Ratio(Right);
        }

        public static explicit operator int(Ratio Value)
        {
            Verify.True(Value.Denumenator == 1, "Not an integer.");
            return Value.Numerator;
        }

        public static explicit operator double(Ratio Value)
        {
            return (double) Value.Numerator / Value.Denumenator;
        }

        public static implicit operator Ratio(int Value)
        {
            return new Ratio(Value);
        }

        public int Floor()
        {
            return this.Numerator / this.Denumenator;
        }

        public override string ToString()
        {
            return this.Numerator.ToString() + ((this.Denumenator != 1) ? ("/" + this.Denumenator.ToString()) : "");
        }

        public int Numerator { get; }

        private readonly int _Denumenator;

        public int Denumenator
        {
            get
            {
                if ((this._Denumenator == 0) & (this.Numerator == 0))
                {
                    return 1;
                }

                return this._Denumenator;
            }
        }

        public static readonly Ratio Zero;
    }
}

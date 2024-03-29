using System;
using System.Collections;
using System.Collections.Generic;

namespace Ks.Common
{
    public class ComparableCollection<T> : List<T>, IComparable<ComparableCollection<T>>, IStructuralComparable, IStructuralEquatable
    {
        public ComparableCollection()
        {
        }

        public ComparableCollection(int Capacity) : base(Capacity)
        {
        }

        public ComparableCollection(IEnumerable<T> Collection) : base(Collection)
        {
        }

        public int CompareTo(ComparableCollection<T> Other)
        {
            return this.CompareTo(Other, System.Collections.Generic.Comparer<object>.Default);
        }

        public override bool Equals(object Obj)
        {
            return this.Equals(Obj, System.Collections.Generic.EqualityComparer<object>.Default);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(System.Collections.Generic.EqualityComparer<object>.Default);
        }

        public int CompareTo(object Other, IComparer Comparer)
        {
            var O = (ComparableCollection<T>) Other;
            var count = Math.Min(this.Count, O.Count);
            for (var I = 0; I < count; I++)
            {
                var C = Comparer.Compare(this[I], O[I]);
                if (C != 0)
                {
                    return C;
                }
            }

            return this.Count - O.Count;
        }

        public bool Equals(object Other, IEqualityComparer Comparer)
        {
            if (!(Other is ComparableCollection<T>))
            {
                return false;
            }

            var O = (ComparableCollection<T>) Other;

            if (this.Count != O.Count)
            {
                return false;
            }

            for (var I = 0; I < this.Count; I++)
            {
                if (!Comparer.Equals(this[I], O[I]))
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(IEqualityComparer Comparer)
        {
            var Bl = true;
            var R = 0;
            foreach (var I in this)
            {
                if (Bl)
                {
                    R = Comparer.GetHashCode(I);
                    Bl = false;
                }
                else
                {
                    R = Utilities.CombineHashCodes(R, Comparer.GetHashCode(I));
                }
            }
            return R;
        }

        public ComparableCollection<T> Clone()
        {
            return new ComparableCollection<T>(this);
        }

        public static bool operator >(ComparableCollection<T> Left, ComparableCollection<T> Right)
        {
            return Left.CompareTo(Right) > 0;
        }

        public static bool operator <(ComparableCollection<T> Left, ComparableCollection<T> Right)
        {
            return Left.CompareTo(Right) < 0;
        }

        public static bool operator >=(ComparableCollection<T> Left, ComparableCollection<T> Right)
        {
            return Left.CompareTo(Right) >= 0;
        }

        public static bool operator <=(ComparableCollection<T> Left, ComparableCollection<T> Right)
        {
            return Left.CompareTo(Right) <= 0;
        }

        public static bool operator ==(ComparableCollection<T> Left, ComparableCollection<T> Right)
        {
            return Left.Equals(Right);
        }

        public static bool operator !=(ComparableCollection<T> Left, ComparableCollection<T> Right)
        {
            return Left.Equals(Right);
        }
    }
}

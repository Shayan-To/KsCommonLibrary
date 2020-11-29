using System.Collections.Generic;

namespace Ks.Common
{
    public class EnumerableCacher<T> : BaseReadOnlyList<T>
    {
        public EnumerableCacher(IEnumerable<T> Enumerable)
        {
            this.Enumerator = Enumerable.GetEnumerator();
        }

        public override int Count
        {
            get
            {
                this.DrainEnumerable();
                return this.List.Count;
            }
        }

        public override T this[int Index]
        {
            get
            {
                Verify.TrueArg(this.TryGetValue(Index, out var Res), nameof(Index), "Index out of range.");
                return Res;
            }
        }

        public void DrainEnumerable()
        {
            this.TryGetValue(int.MaxValue, out _);
        }

        public override IEnumerator<T> GetEnumerator()
        {
            var I = 0;
            while (this.TryGetValue(I, out var T))
            {
                yield return T;
                I += 1;
            }
        }

        public bool TryGetValue(int Index, out T Value)
        {
            for (var I = this.List.Count; I <= Index; I++)
            {
                if (!this.Enumerator.MoveNext())
                {
                    Value = default;
                    return false;
                }
                this.List.Add(this.Enumerator.Current);
            }

            Value = this.List[Index];
            return true;
        }

        private readonly IEnumerator<T> Enumerator;
        private readonly List<T> List = new List<T>();
    }
}

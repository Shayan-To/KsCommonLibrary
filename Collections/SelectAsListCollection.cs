using System.Collections.Generic;
using System;

namespace Ks.Common
{
    public class SelectAsListCollection<TIn, TOut> : BaseReadOnlyList<TOut>
    {
        public SelectAsListCollection(IReadOnlyList<TIn> List, Func<TIn, TOut> Func)
        {
            this.List = List;
            this.Func = Func;
        }

        public SelectAsListCollection(IReadOnlyList<TIn> List, Func<TIn, int, TOut> Func)
        {
            this.List = List;
            this.FuncIndexed = Func;
        }

        protected TOut GetMappedElement(TIn Inp, int Index)
        {
            if (this.Func != null)
            {
                return this.Func.Invoke(Inp);
            }

            return this.FuncIndexed.Invoke(Inp, Index);
        }

        public override int Count
        {
            get
            {
                return this.List.Count;
            }
        }

        public override TOut this[int Index]
        {
            get
            {
                return this.GetMappedElement(this.List[Index], Index);
            }
        }

        public override IEnumerator<TOut> GetEnumerator()
        {
            var Ind = 0;
            foreach (var I in this.List)
            {
                yield return this.GetMappedElement(I, Ind);
                Ind += 1;
            }
        }

        protected readonly IReadOnlyList<TIn> List;
        protected readonly Func<TIn, TOut> Func;
        protected readonly Func<TIn, int, TOut> FuncIndexed;
    }
}

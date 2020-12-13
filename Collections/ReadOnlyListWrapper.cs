using System.Collections.Generic;

namespace Ks
{
    namespace Common
    {
        public class ReadOnlyListWrapper<T> : BaseReadOnlyList<T>
        {
            public ReadOnlyListWrapper(IList<T> List)
            {
                this.List = List;
            }

            public ReadOnlyListWrapper(IReadOnlyList<T> List)
            {
                this.ROList = List;
            }

            public override int Count
            {
                get
                {
                    if (this.List != null)
                        return this.List.Count;
                    else
                        return this.ROList.Count;
                }
            }

            public override T this[int Index]
            {
                get
                {
                    if (this.List != null)
                        return this.List[Index];
                    else
                        return this.ROList[Index];
                }
            }

            public override IEnumerator<T> GetEnumerator()
            {
                if (this.List != null)
                    return this.List.GetEnumerator();
                else
                    return this.ROList.GetEnumerator();
            }

            private readonly IList<T> List;
            private readonly IReadOnlyList<T> ROList;
        }
    }
}

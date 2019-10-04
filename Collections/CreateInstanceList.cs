﻿using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common
    {
        public sealed class CreateInstanceList
        {
            private CreateInstanceList()
            {
                throw new NotSupportedException();
            }

            public static CreateInstanceList<T> Create<T>(IList<T> List, Func<int, T> Creator)
            {
                return new CreateInstanceList<T>(List, Creator);
            }

            public static CreateInstanceList<T> Create<T>(Func<int, T> Creator)
            {
                return new CreateInstanceList<T>(Creator);
            }

            public static CreateInstanceList<T> Create<T>(IList<T> List) where T : new()
            {
                return new CreateInstanceList<T>(List, I => new T());
            }

            public static CreateInstanceList<T> Create<T>() where T : new()
            {
                return new CreateInstanceList<T>(I => new T());
            }
        }

        public class CreateInstanceList<T> : BaseList<T>
        {
            public CreateInstanceList(IList<T> List, Func<int, T> Creator)
            {
                this.List = List;
                this.Creator = Creator;
            }

            public CreateInstanceList(Func<int, T> Creator) : this(new List<T>(), Creator)
            {
            }

            public override void Clear()
            {
                this.List.Clear();
            }

            public override int Count
            {
                get
                {
                    return this.List.Count;
                }
            }

            public override void Insert(int Index, T Item)
            {
                this.List.Insert(Index, Item);
            }

            public new void Insert(int Index)
            {
                this.Insert(Index, this.Creator.Invoke(Index));
            }

            public override T this[int Index]
            {
                get
                {
                    if (Index == this.List.Count)
                    {
                        var V = this.Creator.Invoke(Index);
                        this.List.Add(V);
                        return V;
                    }
                    return this.List[Index];
                }
                set
                {
                    if (Index == this.List.Count)
                        this.List.Add(value);
                    else
                        this.List[Index] = value;
                }
            }

            public override void RemoveAt(int Index)
            {
                this.List.RemoveAt(Index);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.List.GetEnumerator();
            }

            protected override IEnumerator<T> IEnumerable_1_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private readonly Func<int, T> Creator;
            private readonly IList<T> List;
        }
    }
}

﻿using System.Collections.Generic;
using System.Collections;

namespace Ks
{
    namespace Common
    {

        // Public Class NullExpandingList

        // Public Shared Function Create(Of T)(ByVal List As IList(Of T)) As NullExpandingList(Of T)
        // Return New NullExpandingList(Of T)(List)
        // End Function

        // End Class

        public class NullExpandingList<T> : IList<T>
        {
            public int Count
            {
                get
                {
                    return this.List.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public T this[int Index]
            {
                get
                {
                    if (Index >= this.List.Count)
                        return default(T);
                    return this.List[Index];
                }
                set
                {
                    var loopTo = Index;
                    for (int I = this.List.Count; I <= loopTo; I++)
                        this.List.Add(default(T));
                    this.List[Index] = value;
                }
            }

            public void Add(T item)
            {
                this.List.Add(item);
            }

            public void Clear()
            {
                this.List.Clear();
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                this.List.CopyTo(array, arrayIndex);
            }

            public void Insert(int index, T item)
            {
                this.List.Insert(index, item);
            }

            public void RemoveAt(int index)
            {
                this.List.RemoveAt(index);
            }

            public bool Contains(T item)
            {
                return this.List.Contains(item);
            }

            public List<T>.Enumerator GetEnumerator()
            {
                return this.List.GetEnumerator();
            }

            public IEnumerator<T> IEnumerable_1_GetEnumerator()
            {
                return this.List.GetEnumerator();
            }

            public int IndexOf(T item)
            {
                return this.List.IndexOf(item);
            }

            public bool Remove(T item)
            {
                return this.List.Remove(item);
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.List.GetEnumerator();
            }

            private readonly List<T> List = new List<T>();
        }
    }
}

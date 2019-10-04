using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Ks
{
    namespace Common
    {

        // ToDo Is this replacable by CreateInstance...?

        public class ListCollectionRA<T> : ListCollectionRA<T, List<T>>
        {
            public ListCollectionRA() : base(() =>
            {
                return new List<T>();
            })
            {
            }
        }

        public class ListCollectionRA<T, List> : INotifyingCollection<List>, IList<List>, ISerializable where List : IList<T>
        {
            private readonly List<List> InnerList;
            private readonly Func<List> ListSeeder;

            public ListCollectionRA(Func<List> ListSeeder)
            {
                this.ListSeeder = ListSeeder;
                this.InnerList = new List<List>();
            }

            // Public Sub New()
            // Me.New(Function() As List
            // Return DirectCast(GetType(List).GetConstructor(New Type() {}).Invoke(New Object() {}), List)
            // End Function)
            // End Sub

            // Public Sub New(ByVal Type As Type)
            // Me.New(Function() As List
            // Return DirectCast(Type.GetConstructor(New Type() {}).Invoke(New Object() {}), List)
            // End Function)

            // If Not GetType(List).IsAssignableFrom(Type) Then
            // Throw New ArgumentException("Type has to implement the interface List.")
            // End If
            // End Sub

            // Public Sub New(ByVal Constructor As Reflection.ConstructorInfo, ParamArray ByVal Parameters As Object())
            // Me.New(Function() As List
            // Return DirectCast(Constructor.Invoke(Parameters), List)
            // End Function)

            // If Not GetType(List).IsAssignableFrom(Constructor.DeclaringType) Then
            // Throw New ArgumentException("Type has to implement the interface List.")
            // End If
            // Parameters = DirectCast(Parameters.Clone(), Object())
            // End Sub

            public List this[int Index]
            {
                get
                {
                    this.EnsureFits(Index);
                    return this.InnerList[Index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            protected ListCollectionRA(SerializationInfo info, StreamingContext context)
            {
                if ((info == null))
                    throw new ArgumentNullException(nameof(info));
            }

            protected void GetObjectData(SerializationInfo info, StreamingContext context)
            {
            }

            public event NotifyCollectionChangedEventHandler<List> CollectionChanged;
            private event NotifyCollectionChangedEventHandler Nongeneric_CollectionChanged;

            protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs<List> E)
            {
                CollectionChanged?.Invoke(this, E);
                Nongeneric_CollectionChanged?.Invoke(this, E);
            }

            private List InstantiateList()
            {
                return this.ListSeeder.Invoke();
            }

            private void EnsureFits(int Index)
            {
                List T;
                List<List> NewItems;

                if (this.InnerList.Count <= Index)
                {
                    NewItems = new List<List>();
                    var loopTo = Index;
                    for (int I = this.InnerList.Count; I <= loopTo; I++)
                    {
                        T = this.InstantiateList();
                        this.InnerList.Add(T);
                        NewItems.Add(T);
                    }

                    this.OnCollectionChanged(NotifyCollectionChangedEventArgs<List>.CreateAdd(NewItems));
                }
            }

            public List<List>.Enumerator GetEnumerator()
            {
                return this.InnerList.GetEnumerator();
            }

            private IEnumerator<List> GetEnumerator_Interface()
            {
                return this.InnerList.GetEnumerator();
            }

            private IEnumerator IEnumerable_GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public int Count
            {
                get
                {
                    return this.InnerList.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            public void CopyTo(List[] Array, int ArrayIndex)
            {
                this.InnerList.CopyTo(Array, ArrayIndex);
            }

            public int IndexOf(List item)
            {
                throw new NotSupportedException();
            }

            public void Insert(int index, List item)
            {
                throw new NotSupportedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            public void Add(List item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(List item)
            {
                throw new NotSupportedException();
            }

            public bool Remove(List item)
            {
                throw new NotSupportedException();
            }
        }
    }
}

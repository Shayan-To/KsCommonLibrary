using System.Linq;
using System.Collections.Generic;
using System;

namespace Ks.Common
{
        public class ListWithKeys<T> : BaseList<T>
        {
            public override T this[int index]
            {
                get
                {
                    return this.List[index];
                }
                set
                {
                    var Elem = this.List[index];
                    this.List[index] = value;

                    foreach (var KInd in this.Indexes)
                    {
                        var Key = KInd.Key.KeySelector.Invoke(Elem);
                        KInd.Value[Key].Remove(Elem);
                    }
                    foreach (var KInd in this.Indexes)
                    {
                        var Key = KInd.Key.KeySelector.Invoke(value);
                        KInd.Value[Key].Add(value);
                    }
                }
            }

            public override int Count
            {
                get
                {
                    return this.List.Count;
                }
            }

            public override void Insert(int index, T item)
            {
                this.List.Insert(index, item);
                foreach (var KInd in this.Indexes)
                {
                    var Key = KInd.Key.KeySelector.Invoke(item);
                    KInd.Value[Key].Add(item);
                }
            }

            public override void RemoveAt(int index)
            {
                var Elem = this.List[index];
                this.List.RemoveAt(index);
                foreach (var KInd in this.Indexes)
                {
                    var Key = KInd.Key.KeySelector.Invoke(Elem);
                    KInd.Value[Key].Remove(Elem);
                }
            }

            public override void Clear()
            {
                this.List.Clear();
                foreach (var KInd in this.Indexes)
                    KInd.Value.Clear();
            }

            protected override IEnumerator<T> _GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public List<T>.Enumerator GetEnumerator()
            {
                return this.List.GetEnumerator();
            }

            private List<T> GetIndexList(Condition Condition)
            {
                var Index = this.Indexes[Condition.Key];
                List<T> Res = null;
                Index.TryGetValue(Condition.KeyValue, out Res);
                return Res;
            }

            private List<T> GetIndexList<TK>(Condition<TK> Condition)
            {
                var Index = this.Indexes[Condition.Key];
                List<T> Res = null;
                Index.TryGetValue(Condition.KeyValue, out Res);
                return Res;
            }

            /// <example>
        /// List.GetAllWhere(Key1 = "Value1")
        /// </example>
            public IEnumerable<T> GetAllWhere<TK>(Condition<TK> Condition)
            {
                var Res = this.GetIndexList(Condition);
                if (Res == null)
                    return Enumerable.Empty<T>();
                return Res.ToEnumerable();
            }

            /// <example>
        /// List.GetOneWhere(Key1 = "Value1")
        /// </example>
            public T GetOneWhere<TK>(Condition<TK> Condition)
            {
                var Res = this.GetIndexList(Condition);
                Verify.False(Res == null || Res.Count == 0, "No value with such condition was found.");
                return Res[0];
            }

            /// <example>
        /// List.GetSingleWhere(Key1 = "Value1")
        /// </example>
            public T GetSingleWhere<TK>(Condition<TK> Condition)
            {
                var Res = this.GetIndexList(Condition);
                Verify.False(Res == null || Res.Count == 0, "No value with such condition was found.");
                Verify.True(Res.Count == 1, "More than one value with such condition was found.");

                return Res[0];
            }

            /// <example>
        /// List.TryGetOneWhere(Key1 = "Value1")
        /// </example>
            public (bool Success, T Value) TryGetOneWhere<TK>(Condition<TK> Condition)
            {
                var Res = this.GetIndexList(Condition);
                if (Res == null || Res.Count == 0)
                    return (false, default(T));
                return (true, Res[0]);
            }

            /// <example>
        /// List.TryGetSingleWhere(Key1 = "Value1")
        /// </example>
            public (bool Success, T Value) TryGetSingleWhere<TK>(Condition<TK> Condition)
            {
                var Res = this.GetIndexList(Condition);
                if (Res == null || Res.Count != 1)
                    return (false, default(T));
                return (true, Res[0]);
            }

            /// <example>
        /// List.GetAllWhere(Key1 = "Value1")
        /// </example>
            public IEnumerable<T> GetAllWhere(Condition Condition)
            {
                var Res = this.GetIndexList(Condition);
                if (Res == null)
                    return Enumerable.Empty<T>();
                return Res.ToEnumerable();
            }

            /// <example>
        /// List.GetOneWhere(Key1 = "Value1")
        /// </example>
            public T GetOneWhere(Condition Condition)
            {
                var Res = this.GetIndexList(Condition);
                Verify.False(Res == null || Res.Count == 0, "No value with such condition was found.");
                return Res[0];
            }

            /// <example>
        /// List.GetSingleWhere(Key1 = "Value1")
        /// </example>
            public T GetSingleWhere(Condition Condition)
            {
                var Res = this.GetIndexList(Condition);
                Verify.False(Res == null || Res.Count == 0, "No value with such condition was found.");
                Verify.True(Res.Count == 1, "More than one value with such condition was found.");

                return Res[0];
            }

            /// <example>
        /// List.TryGetOneWhere(Key1 = "Value1")
        /// </example>
            public (bool Success, T Value) TryGetOneWhere(Condition Condition)
            {
                var Res = this.GetIndexList(Condition);
                if (Res == null || Res.Count == 0)
                    return (false, default(T));
                return (true, Res[0]);
            }

            /// <example>
        /// List.TryGetSingleWhere(Key1 = "Value1")
        /// </example>
            public (bool Success, T Value) TryGetSingleWhere(Condition Condition)
            {
                var Res = this.GetIndexList(Condition);
                if (Res == null || Res.Count != 1)
                    return (false, default(T));
                return (true, Res[0]);
            }

            public Key<TK> RegisterNewKey<TK>(Func<T, TK> KeySelector)
            {
                var Key = new Key<TK>(this, KeySelector);
                var Index = CreateInstanceDictionary.Create<object, List<T>>();
                for (var I = 0; I < this.List.Count; I++)
                {
                    var Elem = this.List[I];
                    var K = KeySelector.Invoke(Elem);
                    Index[K].Add(Elem);
                }
                this.Indexes.Add(Key, Index);
                return Key;
            }

            private void DestroyKey(Key Key)
            {
                this.Indexes.Remove(Key);
            }

            private readonly List<T> List = new List<T>();
            private readonly Dictionary<Key, CreateInstanceDictionary<object, List<T>>> Indexes = new Dictionary<Key, CreateInstanceDictionary<object, List<T>>>();

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
            public abstract class Key
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
            {
                public Key(ListWithKeys<T> List, Func<T, object> KeySelector)
                {
                    this.List = List;
                    this._KeySelector = KeySelector;
                }

                public static Condition operator ==(Key Key, object KeyValue)
                {
                    Key.VerifyAlive();
                    return new Condition(Key, KeyValue);
                }

                public static Condition operator !=(Key Key, object KeyValue)
                {
                    Key.VerifyAlive();
                    return new Condition(Key, KeyValue);
                }

                protected void VerifyAlive()
                {
                    Verify.False(this.IsKeyDestroyed, "Cannot use a destroyed key.");
                }

                public void Destroy()
                {
                    this._IsKeyDestroyed = true;
                    this.List.DestroyKey(this);
                }

                private bool _IsKeyDestroyed;

                public bool IsKeyDestroyed
                {
                    get
                    {
                        return this._IsKeyDestroyed;
                    }
                }

                private readonly Func<T, object> _KeySelector;

                public Func<T, object> KeySelector
                {
                    get
                    {
                        return this._KeySelector;
                    }
                }

                protected readonly ListWithKeys<T> List;
            }

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
            public class Key<TK> : Key
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
            {
                public Key(ListWithKeys<T> List, Func<T, TK> KeySelector) : base(List, V => KeySelector.Invoke(V))
                {
                    this._KeySelector = KeySelector;
                }

                public static Condition<TK> operator ==(Key<TK> Key, TK KeyValue)
                {
                    Key.VerifyAlive();
                    return new Condition<TK>(Key, KeyValue);
                }

                public static Condition<TK> operator !=(Key<TK> Key, TK KeyValue)
                {
                    Key.VerifyAlive();
                    return new Condition<TK>(Key, KeyValue);
                }

                private readonly Func<T, TK> _KeySelector;

                public new Func<T, TK> KeySelector
                {
                    get
                    {
                        return this._KeySelector;
                    }
                }
            }

            public struct Condition<TK>
            {
                public Condition(Key<TK> Key, TK KeyValue)
                {
                    this._Key = Key;
                    this._KeyValue = KeyValue;
                }

                private readonly Key<TK> _Key;

                public Key<TK> Key
                {
                    get
                    {
                        return this._Key;
                    }
                }

                private readonly TK _KeyValue;

                public TK KeyValue
                {
                    get
                    {
                        return this._KeyValue;
                    }
                }
            }

            public struct Condition
            {
                public Condition(Key Key, object KeyValue)
                {
                    this._Key = Key;
                    this._KeyValue = KeyValue;
                }

                private readonly Key _Key;

                public Key Key
                {
                    get
                    {
                        return this._Key;
                    }
                }

                private readonly object _KeyValue;

                public object KeyValue
                {
                    get
                    {
                        return this._KeyValue;
                    }
                }
            }
        }
    }

﻿using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Specialized;

namespace Ks
{
    namespace Common
    {
        public delegate void NotifyCollectionChangedEventHandler<T>(object sender, NotifyCollectionChangedEventArgs<T> e);

        public class NotifyCollectionChangedEventArgs<T> : NotifyCollectionChangedEventArgs
        {

            /* TODO ERROR: Skipped WarningDirectiveTrivia */
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action) : base(action)
            {
                InitializeAdd(action, null, -1);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateReset()
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Reset);
            }

            /// <summary>
        /// For Add, Remove, or Reset.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T changedItem) : base(action, changedItem)
            {
                if ((int)action == (int)NotifyCollectionChangedAction.Reset)
                    InitializeAdd(action, null, -1);
                else
                    InitializeAddOrRemove(action, new T[] { changedItem }, -1);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateAdd(T changedItem)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, changedItem);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateRemove(T changedItem)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, changedItem);
            }

            /// <summary>
        /// For Add, Remove, or Reset.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T changedItem, int index) : base(action, changedItem, index)
            {
                if ((int)action == (int)NotifyCollectionChangedAction.Reset)
                    InitializeAdd(action, null, -1);
                else
                    InitializeAddOrRemove(action, new T[] { changedItem }, index);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateAdd(T changedItem, int index)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, changedItem, index);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateRemove(T changedItem, int index)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, changedItem, index);
            }

            /// <summary>
        /// For Add, Remove, or Reset.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList<T> changedItems) : base(action, (IList)changedItems)
            {
                if ((int)action == (int)NotifyCollectionChangedAction.Reset)
                    InitializeAdd(action, null, -1);
                else
                    InitializeAddOrRemove(action, changedItems, -1);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateAdd(IList<T> changedItems)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, changedItems);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateRemove(IList<T> changedItems)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, changedItems);
            }

            /// <summary>
        /// For Add, Remove, or Reset.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList<T> changedItems, int startingIndex) : base(action, (IList)changedItems, startingIndex)
            {
                if ((int)action == (int)NotifyCollectionChangedAction.Reset)
                    InitializeAdd(action, null, -1);
                else
                    InitializeAddOrRemove(action, changedItems, startingIndex);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateAdd(IList<T> changedItems, int startingIndex)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Add, changedItems, startingIndex);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateRemove(IList<T> changedItems, int startingIndex)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Remove, changedItems, startingIndex);
            }

            /// <summary>
        /// For Move or Replace.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T newItem, T oldItem) : base(action, newItem, oldItem)
            {
                InitializeMoveOrReplace(action, new T[] { newItem }, new T[] { oldItem }, -1, -1);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateReplace(T newItem, T oldItem)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Replace, newItem, oldItem);
            }

            /// <summary>
        /// For Move or Replace.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T newItem, T oldItem, int index) : base(action, newItem, oldItem, index)
            {
                InitializeMoveOrReplace(action, new T[] { newItem }, new T[] { oldItem }, index, index);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateReplace(T newItem, T oldItem, int index)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);
            }

            /// <summary>
        /// For Move or Replace.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList<T> newItems, IList<T> oldItems) : base(action, (IList)newItems, (IList)oldItems)
            {
                InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateMove(T changedItem)
            {
                var Items = new T[] { changedItem };
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Move, Items, Items);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateMove(IList<T> changedItems)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Move, changedItems, changedItems);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateReplace(IList<T> newItems, IList<T> oldItems)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Replace, newItems, oldItems);
            }

            /// <summary>
        /// For Move or Replace.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList<T> newItems, IList<T> oldItems, int startingIndex) : base(action, (IList)newItems, (IList)oldItems, startingIndex)
            {
                InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateReplace(IList<T> newItems, IList<T> oldItems, int startingIndex)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Replace, newItems, oldItems, startingIndex);
            }

            /// <summary>
        /// For Move or Replace.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, T changedItem, int index, int oldIndex) : base(action, changedItem, index, oldIndex)
            {
                var changedItems = new T[] { changedItem };
                InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateMove(T changedItem, int index, int oldIndex)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Move, changedItem, index, oldIndex);
            }

            /// <summary>
        /// For Move or Replace.
        /// </summary>
            [Obsolete("Use the Create methods instead.")]
            public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList<T> changedItems, int index, int oldIndex) : base(action, (IList)changedItems, index, oldIndex)
            {
                InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
            }

            public static NotifyCollectionChangedEventArgs<T> CreateMove(IList<T> changedItems, int index, int oldIndex)
            {
                return new NotifyCollectionChangedEventArgs<T>(NotifyCollectionChangedAction.Move, changedItems, index, oldIndex);
            }
            /* TODO ERROR: Skipped WarningDirectiveTrivia */
            public static NotifyCollectionChangedEventArgs<T> FromNotifyCollectionChangedEventArgs(NotifyCollectionChangedEventArgs E)
            {
                switch (E.Action)
                {
                    case NotifyCollectionChangedAction.Move:
                        {
                            return CreateMove(E.NewItems.Cast<T>().ToArray(), E.NewStartingIndex, E.OldStartingIndex);
                        }

                    case NotifyCollectionChangedAction.Replace:
                        {
                            return CreateReplace(E.NewItems.Cast<T>().ToArray(), E.OldItems.Cast<T>().ToArray(), E.NewStartingIndex);
                        }

                    case NotifyCollectionChangedAction.Reset:
                        {
                            return CreateReset();
                        }

                    case NotifyCollectionChangedAction.Add:
                        {
                            return CreateAdd(E.NewItems.Cast<T>().ToArray(), E.NewStartingIndex);
                        }

                    case NotifyCollectionChangedAction.Remove:
                        {
                            return CreateRemove(E.OldItems.Cast<T>().ToArray(), E.OldStartingIndex);
                        }
                }

                Assert.Fail();
                return null;
            }

            public static NotifyCollectionChangedEventArgs<T> FromNotifyCollectionChangedEventArgsNoIndex(NotifyCollectionChangedEventArgs E)
            {
                switch (E.Action)
                {
                    case NotifyCollectionChangedAction.Move:
                        {
                            return CreateMove(E.NewItems.Cast<T>().ToArray());
                        }

                    case NotifyCollectionChangedAction.Replace:
                        {
                            return CreateReplace(E.NewItems.Cast<T>().ToArray(), E.OldItems.Cast<T>().ToArray());
                        }

                    case NotifyCollectionChangedAction.Reset:
                        {
                            return CreateReset();
                        }

                    case NotifyCollectionChangedAction.Add:
                        {
                            return CreateAdd(E.NewItems.Cast<T>().ToArray());
                        }

                    case NotifyCollectionChangedAction.Remove:
                        {
                            return CreateRemove(E.OldItems.Cast<T>().ToArray());
                        }
                }

                Assert.Fail();
                return null;
            }

            private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList<T> changedItems, int startingIndex)
            {
                if ((int)action == (int)NotifyCollectionChangedAction.Add)
                    InitializeAdd(action, changedItems, startingIndex);
                else if ((int)action == (int)NotifyCollectionChangedAction.Remove)
                    InitializeRemove(action, changedItems, startingIndex);
            }

            private void InitializeAdd(NotifyCollectionChangedAction action, IList<T> newItems, int newStartingIndex)
            {
                this._NewItems = (newItems == null) ? null : new List<T>(newItems).AsReadOnly();
            }

            private void InitializeRemove(NotifyCollectionChangedAction action, IList<T> oldItems, int oldStartingIndex)
            {
                this._OldItems = (oldItems == null) ? null : new List<T>(oldItems).AsReadOnly();
            }

            private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList<T> newItems, IList<T> oldItems, int startingIndex, int oldStartingIndex)
            {
                InitializeAdd(action, newItems, startingIndex);
                InitializeRemove(action, oldItems, oldStartingIndex);
            }

            public IList<T> ItemsGotIn
            {
                get
                {
                    if (((int)this.Action == (int)NotifyCollectionChangedAction.Add) | ((int)this.Action == (int)NotifyCollectionChangedAction.Replace))
                        return this.NewItems;
                    return Utilities.Typed<T>.EmptyArray;
                }
            }

            public IList<T> ItemsWentOut
            {
                get
                {
                    if (((int)this.Action == (int)NotifyCollectionChangedAction.Remove) | ((int)this.Action == (int)NotifyCollectionChangedAction.Replace))
                        return this.OldItems;
                    return Utilities.Typed<T>.EmptyArray;
                }
            }

            private IList<T> _NewItems;

            public new IList<T> NewItems
            {
                get
                {
                    return this._NewItems;
                }
            }

            private IList<T> _OldItems;

            public new IList<T> OldItems
            {
                get
                {
                    return this._OldItems;
                }
            }
        }

        public interface INotifyCollectionChanged<T> : INotifyCollectionChanged
        {
            new event NotifyCollectionChangedEventHandler<T> CollectionChanged;
        }

        public interface INotifyingCollection<T> : INotifyCollectionChanged<T>, ICollection<T>
        {
        }

        public class NotifyingList<T> : BaseList<T>, INotifyingCollection<T>
        {
            public NotifyingList(IList<T> List)
            {
                this.BaseList = List;
            }

            public NotifyingList() : this(new List<T>())
            {
            }

            public override void Clear()
            {
                this.BaseList.Clear();
                this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateReset());
            }

            public override void Insert(int index, T item)
            {
                this.BaseList.Insert(index, item);
                this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateAdd(item, index));
            }

            public override void RemoveAt(int index)
            {
                var OldItem = this.BaseList[index];
                this.BaseList.RemoveAt(index);
                this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateRemove(OldItem, index));
            }

            public virtual void Move(int OldIndex, int NewIndex)
            {
                var Item = this.BaseList[OldIndex];
                this.BaseList.Move(OldIndex, NewIndex);
                this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateMove(Item, NewIndex, OldIndex));
            }

            public virtual void SetFrom(IEnumerable<T> Collection)
            {
                this.BaseList.Clear();
                this.BaseList.AddRange(Collection);
                this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateReset());
            }

            protected override IEnumerator<T> IEnumerable_1_GetEnumerator()
            {
                return this.BaseList.GetEnumerator();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return this.BaseList.GetEnumerator();
            }

            public override int Count
            {
                get
                {
                    return this.BaseList.Count;
                }
            }

            public override T this[int index]
            {
                get
                {
                    return this.BaseList[index];
                }
                set
                {
                    var OldItem = this.BaseList[index];
                    this.BaseList[index] = value;
                    this.OnCollectionChanged(NotifyCollectionChangedEventArgs<T>.CreateReplace(value, OldItem, index));
                }
            }

            public new event NotifyCollectionChangedEventHandler<T> CollectionChanged;
            private event NotifyCollectionChangedEventHandler INotifyCollectionChanged_CollectionChanged;

            protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs<T> E)
            {
                INotifyCollectionChanged_CollectionChanged?.Invoke(this, E);
                CollectionChanged?.Invoke(this, E);
            }

            private readonly IList<T> BaseList;
        }
    }
}

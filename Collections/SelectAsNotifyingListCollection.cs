using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Ks.Common
{
    public class SelectAsNotifyingListCollection<TIn, TOut> : SelectAsListCollection<TIn, TOut>, INotifyCollectionChanged<TOut>
    {
        public SelectAsNotifyingListCollection(IReadOnlyList<TIn> List, Func<TIn, TOut> Func) : base(List, Func)
        {
            if (List is INotifyCollectionChanged Notifying)
            {
                Notifying.CollectionChanged += this.List_CollectionChanged;
            }
        }

        public SelectAsNotifyingListCollection(IReadOnlyList<TIn> List, Func<TIn, int, TOut> Func) : base(List, Func)
        {
            if (List is INotifyCollectionChanged Notifying)
            {
                Notifying.CollectionChanged += this.List_CollectionChanged;
            }
        }

        private void List_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs E)
        {
            NotifyCollectionChangedEventArgs<TOut> NE;

            if (this.Func != null)
            {
                NE = E.Action switch
                {
                    NotifyCollectionChangedAction.Move =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateMove(E.NewItems.CastAsList<TIn>().SelectAsList(this.Func), E.NewStartingIndex, E.OldStartingIndex),
                    NotifyCollectionChangedAction.Replace =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateReplace(E.NewItems.CastAsList<TIn>().SelectAsList(this.Func), E.OldItems.CastAsList<TIn>().SelectAsList(this.Func), E.NewStartingIndex),
                    NotifyCollectionChangedAction.Reset =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateReset(),
                    NotifyCollectionChangedAction.Add =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateAdd(E.NewItems.CastAsList<TIn>().SelectAsList(this.Func), E.NewStartingIndex),
                    NotifyCollectionChangedAction.Remove =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateRemove(E.OldItems.CastAsList<TIn>().SelectAsList(this.Func), E.OldStartingIndex),
                    _ => throw new InvalidOperationException("Action not supported."),
                };
            }
            else
            {
                NE = E.Action switch
                {
                    NotifyCollectionChangedAction.Move =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateMove(E.NewItems.CastAsList<TIn>().SelectAsList(this.FuncIndexed), E.NewStartingIndex, E.OldStartingIndex),
                    NotifyCollectionChangedAction.Replace =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateReplace(E.NewItems.CastAsList<TIn>().SelectAsList(this.FuncIndexed), E.OldItems.CastAsList<TIn>().SelectAsList(this.FuncIndexed), E.NewStartingIndex),
                    NotifyCollectionChangedAction.Reset =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateReset(),
                    NotifyCollectionChangedAction.Add =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateAdd(E.NewItems.CastAsList<TIn>().SelectAsList(this.FuncIndexed), E.NewStartingIndex),
                    NotifyCollectionChangedAction.Remove =>
                        NotifyCollectionChangedEventArgs<TOut>.CreateRemove(E.OldItems.CastAsList<TIn>().SelectAsList(this.FuncIndexed), E.OldStartingIndex),
                    _ => throw new InvalidOperationException("Action not supported."),
                };
            }

            this.OnCollectionChanged(NE);
        }

        public event NotifyCollectionChangedEventHandler<TOut> CollectionChanged;
        private event NotifyCollectionChangedEventHandler INotifyCollectionChanged_CollectionChanged;
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add => this.INotifyCollectionChanged_CollectionChanged += value;
            remove => this.INotifyCollectionChanged_CollectionChanged -= value;
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs<TOut> E)
        {
            CollectionChanged?.Invoke(this, E);
            INotifyCollectionChanged_CollectionChanged?.Invoke(this, E);
        }
    }
}

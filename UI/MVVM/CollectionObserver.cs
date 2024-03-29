using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Ks.Common.MVVM
{
    public class CollectionObserver<T>
    {
        private void Collection_CollectionChanged(object? Sender, NotifyCollectionChangedEventArgs E)
        {
            switch (E.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (T I in E.NewItems)
                    {
                        this.OnElementGotIn(new ElementEventArgs<T>(I!));
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (T I in E.OldItems)
                    {
                        this.OnElementGotOut(new ElementEventArgs<T>(I!));
                    }

                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Replace:
                    foreach (T I in E.OldItems)
                    {
                        this.OnElementGotOut(new ElementEventArgs<T>(I!));
                    }

                    foreach (T I in E.NewItems)
                    {
                        this.OnElementGotIn(new ElementEventArgs<T>(I!));
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (T I in this.Clone)
                    {
                        this.OnElementGotOut(new ElementEventArgs<T>(I!));
                    }

                    if (this.Collection != null)
                    {
                        foreach (T I in this.Collection)
                        {
                            this.OnElementGotIn(new ElementEventArgs<T>(I!));
                        }
                    }

                    break;
            }

            this.OnCollectionChanged();

            switch (E.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    for (var I = 0; I < E.NewItems.Count; I++)
                    {
                        this.Clone.Insert(E.NewStartingIndex + I, E.NewItems[I]!);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    for (var I = E.OldItems.Count - 1; I >= 0; I--)
                    {
                        this.Clone.RemoveAt(E.OldStartingIndex + I);
                    }

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (E.NewStartingIndex < E.OldStartingIndex)
                    {
                        for (var I = 0; I < E.NewItems.Count; I++)
                        {
                            this.Clone.Move(E.OldStartingIndex + I, E.NewStartingIndex + I);
                        }
                    }
                    else
                    {
                        for (var I = E.NewItems.Count - 1; I >= 0; I--)
                        {
                            this.Clone.Move(E.OldStartingIndex + I, E.NewStartingIndex + I);
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Replace:
                    for (var I = 0; I < E.NewItems.Count; I++)
                    {
                        this.Clone[E.NewStartingIndex + I] = E.NewItems[I]!;
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.Clone.Clear();
                    if (this.Collection != null)
                    {
                        this.Clone.AddRange(this.Collection.Cast<object>());
                    }

                    break;
            }
        }

        public event EventHandler<ElementEventArgs<T>>? ElementGotIn;

        protected virtual void OnElementGotIn(ElementEventArgs<T> E)
        {
            ElementGotIn?.Invoke(this, E);
        }

        public event EventHandler<ElementEventArgs<T>>? ElementGotOut;

        protected virtual void OnElementGotOut(ElementEventArgs<T> E)
        {
            ElementGotOut?.Invoke(this, E);
        }

        public event EventHandler? CollectionChanged;

        protected virtual void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerable? _Collection;

        public IEnumerable? Collection
        {
            get => this._Collection;
            set
            {
                if (this._Collection is INotifyCollectionChanged Obs)
                {
                    Obs.CollectionChanged -= this.Collection_CollectionChanged;
                }

                this._Collection = value;

                if (this._Collection is INotifyCollectionChanged nObs)
                {
                    nObs.CollectionChanged += this.Collection_CollectionChanged;
                }

                if (this.AssumeSettingOfCollectionAsReset)
                {
                    this.Collection_CollectionChanged(this.Collection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                else
                {
                    this.Clone.Clear();
                    if (this.Collection != null)
                    {
                        this.Clone.AddRange(this.Collection.Cast<object>());
                    }
                }
            }
        }

        public bool AssumeSettingOfCollectionAsReset { get; set; } = true;

        private readonly List<object> Clone = new List<object>();
    }

    public class ElementEventArgs<T> : EventArgs
    {
        public ElementEventArgs(T Element)
        {
            this.Element = Element;
        }

        public T Element { get; }
    }
}

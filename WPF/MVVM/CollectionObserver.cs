using System.Collections.Generic;
using System.Collections;
using System;
using System.Collections.Specialized;

namespace Ks
{
    namespace Ks.Common.MVVM
    {
        public class CollectionObserver<T>
        {
            public CollectionObserver()
            {
                this._Collection_CollectionChanged = new NotifyCollectionChangedEventHandler(this.Collection_CollectionChanged);
            }

            private void Collection_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs E)
            {
                switch (E.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            foreach (T I in E.NewItems)
                                this.OnElementGotIn(new ElementEventArgs<T>(I));
                            break;
                        }

                    case NotifyCollectionChangedAction.Remove:
                        {
                            foreach (T I in E.OldItems)
                                this.OnElementGotOut(new ElementEventArgs<T>(I));
                            break;
                        }

                    case NotifyCollectionChangedAction.Move:
                        {
                            break;
                        }

                    case NotifyCollectionChangedAction.Replace:
                        {
                            foreach (T I in E.OldItems)
                                this.OnElementGotOut(new ElementEventArgs<T>(I));
                            foreach (T I in E.NewItems)
                                this.OnElementGotIn(new ElementEventArgs<T>(I));
                            break;
                        }

                    case NotifyCollectionChangedAction.Reset:
                        {
                            foreach (T I in this.Clone)
                                this.OnElementGotOut(new ElementEventArgs<T>(I));
                            foreach (T I in this.Collection)
                                this.OnElementGotIn(new ElementEventArgs<T>(I));
                            break;
                        }
                }

                this.OnCollectionChanged();

                switch (E.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var loopTo = E.NewItems.Count - 1;
                            for (int I = 0; I <= loopTo; I++)
                                this.Clone.Insert(E.NewStartingIndex + I, E.NewItems[I]);
                            break;
                        }

                    case NotifyCollectionChangedAction.Remove:
                        {
                            for (int I = E.OldItems.Count - 1; I >= 0; I += -1)
                                this.Clone.RemoveAt(E.OldStartingIndex + I);
                            break;
                        }

                    case NotifyCollectionChangedAction.Move:
                        {
                            if (E.NewStartingIndex < E.OldStartingIndex)
                            {
                                var loopTo1 = E.NewItems.Count - 1;
                                for (int I = 0; I <= loopTo1; I++)
                                    this.Clone.Move(E.OldStartingIndex + I, E.NewStartingIndex + I);
                            }
                            else
                                for (int I = E.NewItems.Count - 1; I >= 0; I += -1)
                                    this.Clone.Move(E.OldStartingIndex + I, E.NewStartingIndex + I);
                            break;
                        }

                    case NotifyCollectionChangedAction.Replace:
                        {
                            var loopTo2 = E.NewItems.Count - 1;
                            for (int I = 0; I <= loopTo2; I++)
                                this.Clone[E.NewStartingIndex + I] = E.NewItems[I];
                            break;
                        }

                    case NotifyCollectionChangedAction.Reset:
                        {
                            this.Clone.Clear();
                            if (this.Collection != null)
                                this.Clone.AddRange(this.Collection);
                            break;
                        }
                }
            }

            public event EventHandler<ElementEventArgs<T>> ElementGotIn;

            protected virtual void OnElementGotIn(ElementEventArgs<T> E)
            {
                ElementGotIn?.Invoke(this, E);
            }

            public event EventHandler<ElementEventArgs<T>> ElementGotOut;

            protected virtual void OnElementGotOut(ElementEventArgs<T> E)
            {
                ElementGotOut?.Invoke(this, E);
            }

            public event EventHandler CollectionChanged;

            protected virtual void OnCollectionChanged()
            {
                CollectionChanged?.Invoke(this, EventArgs.Empty);
            }

            private IEnumerable _Collection;

            public IEnumerable Collection
            {
                get
                {
                    return this._Collection;
                }
                set
                {
                    var Obs = this._Collection as INotifyCollectionChanged;
                    if (Obs != null)
                        Obs.CollectionChanged -= this._Collection_CollectionChanged;

                    this._Collection = value;

                    Obs = this._Collection as INotifyCollectionChanged;
                    if (Obs != null)
                        Obs.CollectionChanged += this._Collection_CollectionChanged;

                    if (this.AssumeSettingOfCollectionAsReset)
                        this.Collection_CollectionChanged(this.Collection, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    else
                    {
                        this.Clone.Clear();
                        if (this.Collection != null)
                            this.Clone.AddRange(this.Collection);
                    }
                }
            }

            private bool _AssumeSettingOfCollectionAsReset = true;

            public bool AssumeSettingOfCollectionAsReset
            {
                get
                {
                    return this._AssumeSettingOfCollectionAsReset;
                }
                set
                {
                    this._AssumeSettingOfCollectionAsReset = value;
                }
            }

            private readonly List<object> Clone = new List<object>();
            private readonly NotifyCollectionChangedEventHandler _Collection_CollectionChanged;
        }

        public class ElementEventArgs<T> : EventArgs
        {
            public ElementEventArgs(T Element)
            {
                this._Element = Element;
            }

            private readonly T _Element;

            public T Element
            {
                get
                {
                    return this._Element;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace Ks.Common
{
    public struct ObservableEvents<T> where T : class
    {
        public ObservableEvents(T @object)
        {
            this.Object = @object;
        }

        public T Object { get; }
    }

    public static partial class EventsExtensions
    {
        public static ObservableEvents<T> ObservableEvents<T>(this T self) where T : class
        {
            return new ObservableEvents<T>(self);
        }

        public static IObservable<EventData<object, NotifyCollectionChangedEventArgs>> CollectionChanged<T>(this ObservableEvents<ObservableCollection<T>> self)
        {
            return Observable.Create<EventData<object, NotifyCollectionChangedEventArgs>>(obs =>
            {
                void Handler(object s, NotifyCollectionChangedEventArgs e)
                {
                    obs.OnNext(new(s, e));
                }

                self.Object.CollectionChanged += Handler;
                return () => self.Object.CollectionChanged -= Handler;
            });
        }

        public struct EventData<TSender, TEventArgs>
        {
            public EventData(TSender sender, TEventArgs eventArgs)
            {
                this.Sender = sender;
                this.EventArgs = eventArgs;
            }

            public TSender Sender { get; }
            public TEventArgs EventArgs { get; }
        }
    }
}

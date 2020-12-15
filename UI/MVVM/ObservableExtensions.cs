using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;

using DynamicData;
using DynamicData.Binding;

using ReactiveUI;

namespace Ks.Common.MVVM
{
    public static class ObservableExtensions
    {

        public static ObservableAsPropertyHelper<TRet> ToSafeProperty<TObj, TRet>(this IObservable<TRet> target, TObj source, string property, TRet initialValue = default, IScheduler? scheduler = null) where TObj : class, IReactiveObject
        {
            target = target.Publish().RefCount();
            var initialValueSubscription = target.Subscribe(v => initialValue = v);

            source.RaisePropertyChanging(property);
            var res = target.ToProperty(source, property, initialValue, false, scheduler);
            source.RaisePropertyChanged(property);

            initialValueSubscription.Dispose();

            return res!;
        }

        public static ObservableCollectionExtended<T> ToObservableCollection<T>(this IObservable<IChangeSet<T>> source, int resetThreshold = 25)
        {
            var observableCollectionExtended = new ObservableCollectionExtended<T>();
            var adaptor = new ObservableCollectionAdaptor<T>(observableCollectionExtended, resetThreshold);
            source.Adapt(adaptor).Subscribe();
            return observableCollectionExtended;
        }

    }
}

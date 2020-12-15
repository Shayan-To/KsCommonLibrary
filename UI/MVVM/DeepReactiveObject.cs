using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using ReactiveUI;

using IPropertyChangedEventArgs = ReactiveUI.IReactivePropertyChangedEventArgs<ReactiveUI.IReactiveObject>;

namespace Ks.Common.MVVM
{
    public class DeepReactiveObject : ReactiveObject
    {

        private readonly Subject<DeepChangedEventArgs> _DeepChangedSubject = new();

        [IgnoreDataMember]
        public IObservable<DeepChangedEventArgs> DeepChanged { get; }

        protected DeepChangeSource DeepChangeSource { get; set; }

        public DeepReactiveObject()
        {
            this.DeepChanged = this._DeepChangedSubject.Select(e => e.WithSource(this.DeepChangeSource));
            this.Changed.Subscribe(e => this._DeepChangedSubject.OnNext(new(e)));
        }

        protected LambdaCallDisposable WithDeepChangeSource(DeepChangeSource deepChangeSource)
        {
            var orig = this.DeepChangeSource;
            this.DeepChangeSource = deepChangeSource;
            return new(() => this.DeepChangeSource = orig);
        }

        protected static IDisposable PropagateCollectionChange<TModel>(TModel self, Expression<Func<TModel, IEnumerable<DeepReactiveObject>?>> collection, PropagateDepth depth, bool @const = true) where TModel : DeepReactiveObject
        {
            var subscriptions = new Dictionary<DeepReactiveObject, IDisposable>(ReferenceEqualityComparer<DeepReactiveObject>.Instance);

            var propertyName = collection.GetAccessedMemberInfo().Name;
            var propertyPath = new[] { propertyName, "[]" };

            var obs = new CollectionObserver<DeepReactiveObject>();
            if (depth != PropagateDepth.Shallow)
            {
                obs.ElementGotIn += (_, e) =>
                {
                    if (e.Element != null)
                    {
                        var obs = depth == PropagateDepth.Deep ? e.Element.DeepChanged : e.Element.Changed.Select(e => new DeepChangedEventArgs(e));
                        var subs = obs.Subscribe(e => self._DeepChangedSubject.OnNext(new(e, propertyPath)));
                        subscriptions.Add(e.Element, subs);
                    }
                };
                obs.ElementGotOut += (_, e) =>
                {
                    if (e.Element == null)
                    {
                        return;
                    }
                    if (!subscriptions.Remove(e.Element, out var subscription))
                    {
                        throw Assert.Fail();
                    }
                    subscription.Dispose();
                };
            }

            IDisposable? propSubscription = null;
            if (@const)
            {
                obs.Collection = collection.Compile().Invoke(self);
            }
            else
            {
                propSubscription = self.WhenAnyValue(collection)
                    .Concat(Observable.Return<IEnumerable<DeepReactiveObject>?>(null))
                    .Subscribe(c => obs.Collection = c);
            }

            obs.CollectionChanged += (_, _) => self._DeepChangedSubject.OnNext(new(propertyPath));

            var subscription = new LambdaCallDisposable(() => obs.Collection = null);
            return propSubscription == null ? subscription : new CompositeDisposable(propSubscription, subscription);
        }

        protected static IDisposable PropagateConstPropertyChange<TModel>(TModel self, Expression<Func<TModel, DeepReactiveObject?>> prop, PropagateDepth depth) where TModel : DeepReactiveObject
        {
            var propertyName = prop.GetAccessedMemberInfo().Name;
            var value = prop.Compile().Invoke(self);
            if (value != null)
            {
                var obs = depth == PropagateDepth.Deep ? value.DeepChanged : value.Changed.Select(e => new DeepChangedEventArgs(e));
                var subscription = obs.Subscribe(e => self._DeepChangedSubject.OnNext(new(e, propertyName)));
                return subscription;
            }
            return Disposable.Empty;
        }

        protected static IDisposable PropagatePropertyChange<TModel>(TModel self, Expression<Func<TModel, DeepReactiveObject?>> prop, PropagateDepth depth) where TModel : DeepReactiveObject
        {
            var propertyName = prop.GetAccessedMemberInfo().Name;

            IDisposable? innerSubscription = null;

            var subscription = self.WhenAnyValue(prop)
                .Concat(Observable.Return<DeepReactiveObject?>(null))
                .Subscribe(v =>
                {
                    if (innerSubscription != null)
                    {
                        innerSubscription.Dispose();
                        innerSubscription = null;
                    }
                    if (v != null)
                    {
                        var obs = depth == PropagateDepth.Deep ? v.DeepChanged : v.Changed.Select(e => new DeepChangedEventArgs(e));
                        innerSubscription = obs.Subscribe(e => self._DeepChangedSubject.OnNext(new(e, propertyName)));
                    }
                });

            return new CompositeDisposable(subscription, new LambdaCallDisposable(() =>
            {
                if (innerSubscription != null)
                {
                    innerSubscription.Dispose();
                    innerSubscription = null;
                }
            }));
        }

        protected enum PropagateDepth
        {
            Shallow,
            Normal,
            Deep
        }
    }

    public struct DeepChangedEventArgs
    {
        public DeepChangedEventArgs(string propertyName, bool isDirect = true)
        {
            this.PropertyPath = ImmutableArray.Create(propertyName);
            this.IsDirect = isDirect;
            this.OriginalEvent = null;
            this.Source = DeepChangeSource.Normal;
        }

        public DeepChangedEventArgs(params string[] propertyPath)
        {
            this.PropertyPath = ImmutableArray.Create(propertyPath);
            this.IsDirect = propertyPath.Length <= 1;
            this.OriginalEvent = null;
            this.Source = DeepChangeSource.Normal;
        }

        public DeepChangedEventArgs(IPropertyChangedEventArgs originalEvent)
        {
            this.PropertyPath = ImmutableArray.Create(originalEvent.PropertyName);
            this.IsDirect = true;
            this.OriginalEvent = originalEvent;
            this.Source = DeepChangeSource.Normal;
        }

        public DeepChangedEventArgs(DeepChangedEventArgs originalEvent, string? propertyName = null)
        {
            this.PropertyPath = propertyName == null ? originalEvent.PropertyPath : originalEvent.PropertyPath.Insert(0, propertyName);
            this.IsDirect = propertyName == null && originalEvent.IsDirect;
            this.OriginalEvent = originalEvent.OriginalEvent;
            this.Source = DeepChangeSource.Normal;
        }

        public DeepChangedEventArgs(DeepChangedEventArgs originalEvent, params string[] propertyPath)
        {
            this.PropertyPath = originalEvent.PropertyPath.InsertRange(0, propertyPath);
            this.IsDirect = propertyPath.Length == 0 && originalEvent.IsDirect;
            this.OriginalEvent = originalEvent.OriginalEvent;
            this.Source = DeepChangeSource.Normal;
        }

        public DeepChangedEventArgs WithSource(DeepChangeSource source)
        {
            var res = this;
            res.Source = source;
            return res;
        }

        public bool IsDeep => !this.IsDirect;
        public string PropertyName => this.PropertyPath[0];
        public bool IsDirect { get; }
        public DeepChangeSource Source { get; private set; }
        public ImmutableArray<string> PropertyPath { get; }
        public IPropertyChangedEventArgs? OriginalEvent { get; }
    }

    public enum DeepChangeSource
    {
        Normal,
        RepeatableComputation,
    }
}

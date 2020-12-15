using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Ks.Common
{
    public static class ObservableExtensions
    {
        public static IObservable<(T? Prev, T? Cur)> WithPrevious<T>(this IObservable<T> self)
        {
            return self.Scan(((T? Prev, T? Cur)) (default, default), (a, cur) => (a.Cur, cur));
        }

        public static IObservable<Unit> SelectUnit<T>(this IObservable<T> self)
        {
            return self.Select(_ => Unit.Default);
        }

        public static IDisposable LifeTimeSubscribe<T, TTarget>(this IObservable<T> self, TTarget target, Action<TTarget, T>? onNext = null, Action<TTarget, Exception>? onError = null, Action<TTarget>? onCompleted = null) where TTarget : class
        {
            Verify.TrueArg(onNext == null || onNext.GetInvocationList().All(i => i.Target == null), nameof(onNext), "Must be a static method.");
            Verify.TrueArg(onError == null || onError.GetInvocationList().All(i => i.Target == null), nameof(onError), "Must be a static method.");
            Verify.TrueArg(onCompleted == null || onCompleted.GetInvocationList().All(i => i.Target == null), nameof(onCompleted), "Must be a static method.");

            var weakReference = new WeakReference<TTarget>(target);
            IDisposable subscription = null!;

            void onNext2(T v)
            {
                if (weakReference.TryGetTarget(out var target))
                {
                    onNext?.Invoke(target, v);
                }
                else
                {
                    subscription.Dispose();
                }
            }

            void onError2(Exception ex)
            {
                if (weakReference.TryGetTarget(out var target))
                {
                    onError?.Invoke(target, ex);
                }
                else
                {
                    subscription.Dispose();
                }
            }

            void onCompleted2()
            {
                if (weakReference.TryGetTarget(out var target))
                {
                    onCompleted?.Invoke(target);
                }
                else
                {
                    subscription.Dispose();
                }
            }

            subscription = self.Subscribe(onNext2, onError2, onCompleted2);
            return subscription;
        }
    }
}

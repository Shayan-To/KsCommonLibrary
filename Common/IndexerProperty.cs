using System;

namespace Ks.Common
{
    public class IndexerProperty<T, TProp, TIn>
    {
        // ToDo Revise the API to be more nice. (See BitArray)

        public IndexerProperty(T self, Func<T, TIn, TProp> getter, Action<T, TIn, TProp> setter)
        {
            this.Self = self;
            this.Getter = getter;
            this.Setter = setter;
        }

        public TProp this[TIn i]
        {
            get => this.Getter.Invoke(this.Self, i);
            set => this.Setter.Invoke(this.Self, i, value);
        }

        private readonly T Self;
        public readonly Func<T, TIn, TProp> Getter;
        public readonly Action<T, TIn, TProp> Setter;

    }

    public class IndexerPropertyReadOnly<T, TProp, TIn>
    {

        public IndexerPropertyReadOnly(T self, Func<T, TIn, TProp> getter)
        {
            this.Self = self;
            this.Getter = getter;
        }

        public TProp this[TIn i] => this.Getter.Invoke(this.Self, i);

        private readonly T Self;
        public readonly Func<T, TIn, TProp> Getter;

    }
}

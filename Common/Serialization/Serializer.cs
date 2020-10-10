using System;

namespace Ks.Common
{
    public abstract class Serializer
    {
        public Serializer(string Id)
        {
            this.Id = Id;
        }

        public abstract bool CanSerializeType(Type Type);
        public abstract void Set(FormatterSetProxy Formatter, object Obj);

        public virtual object Get(FormatterGetProxy Formatter)
        {
            throw new NotSupportedException();
        }

        public virtual void Get(FormatterGetProxy Formatter, object Obj)
        {
            throw new NotSupportedException();
        }

        public string Id { get; }

        public static Serializer Create(string Id, Func<Type, bool> CanSerialize, Func<FormatterGetProxy, object> Get, Action<FormatterGetProxy, object> Get2, Action<FormatterSetProxy, object> Set)
        {
            return new DelegateSerializer(Id, CanSerialize, Get, Get2, Set);
        }

        private class DelegateSerializer : Serializer
        {
            public DelegateSerializer(string Id, Func<Type, bool> CanSerialize, Func<FormatterGetProxy, object> Get, Action<FormatterGetProxy, object> Get2, Action<FormatterSetProxy, object> Set) : base(Id)
            {
                this.CanSerializeDelegate = CanSerialize;
                this.GetDelegate = Get;
                this.GetDelegate2 = Get2;
                this.SetDelegate = Set;
            }

            public override bool CanSerializeType(Type Type)
            {
                return this.CanSerializeDelegate.Invoke(Type);
            }

            public override void Set(FormatterSetProxy Formatter, object Obj)
            {
                this.SetDelegate.Invoke(Formatter, Obj);
            }

            public override object Get(FormatterGetProxy Formatter)
            {
                if (this.GetDelegate == null)
                    throw new NotSupportedException();
                return this.GetDelegate.Invoke(Formatter);
            }

            public override void Get(FormatterGetProxy Formatter, object Obj)
            {
                if (this.GetDelegate2 == null)
                    throw new NotSupportedException();
                this.GetDelegate2.Invoke(Formatter, Obj);
            }

            private readonly Func<Type, bool> CanSerializeDelegate;
            private readonly Func<FormatterGetProxy, object> GetDelegate;
            private readonly Action<FormatterGetProxy, object> GetDelegate2;
            private readonly Action<FormatterSetProxy, object> SetDelegate;
        }
    }

    public abstract class Serializer<T> : Serializer
    {
        public Serializer(string Id) : base(Id)
        {
        }

        public abstract void SetT(FormatterSetProxy Formatter, T Obj);

        public virtual T GetT(FormatterGetProxy Formatter)
        {
            throw new NotSupportedException();
        }

        public virtual void GetT(FormatterGetProxy Formatter, T Obj)
        {
            throw new NotSupportedException();
        }

        // ToDo This does not work... Fix it.
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public override bool CanSerializeType(Type Type)
        {
            return Type == typeof(T);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public sealed override object Get(FormatterGetProxy Formatter)
        {
            return this.GetT(Formatter);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public sealed override void Get(FormatterGetProxy Formatter, object Obj)
        {
            this.GetT(Formatter, (T) Obj);
        }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public sealed override void Set(FormatterSetProxy Formatter, object Obj)
        {
            this.SetT(Formatter, (T) Obj);
        }

        public static Serializer<T> Create(string Id, Func<FormatterGetProxy, T> Get, Action<FormatterGetProxy, T> Get2, Action<FormatterSetProxy, T> Set)
        {
            return new DelegateSerializer(Id, Get, Get2, Set);
        }

        private class DelegateSerializer : Serializer<T>
        {
            public DelegateSerializer(string Id, Func<FormatterGetProxy, T> Get, Action<FormatterGetProxy, T> Get2, Action<FormatterSetProxy, T> Set) : base(Id)
            {
                this.GetDelegate = Get;
                this.SetDelegate = Set;
                this.GetDelegate2 = Get2;
            }

            public override void SetT(FormatterSetProxy Formatter, T Obj)
            {
                this.SetDelegate.Invoke(Formatter, Obj);
            }

            public override T GetT(FormatterGetProxy Formatter)
            {
                if (this.GetDelegate == null)
                    throw new NotSupportedException();
                return this.GetDelegate.Invoke(Formatter);
            }

            public override void GetT(FormatterGetProxy Formatter, T Obj)
            {
                if (this.GetDelegate2 == null)
                    throw new NotSupportedException();
                this.GetDelegate2.Invoke(Formatter, Obj);
            }

            private readonly Func<FormatterGetProxy, T> GetDelegate;
            private readonly Action<FormatterGetProxy, T> GetDelegate2;
            private readonly Action<FormatterSetProxy, T> SetDelegate;
        }
    }
}

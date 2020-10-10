using System;

namespace Ks.Common
{
    public abstract class GenericSerializer : Serializer
    {
        public GenericSerializer(string Id, Type SerializerType) : base(Id)
        {
            Verify.True(SerializerType.IsGenericTypeDefinition);
            this.SerializerType = SerializerType;
        }

        protected abstract Type[] GetTypeArguments(Type Type);

        private Serializer GetSerializer(Type[] TypeArguments)
        {
            if (this.Cache.TryGetValue(TypeArguments, out var S))
                return S;

            S = (Serializer) this.SerializerType.MakeGenericType(TypeArguments).CreateInstance();

            while (this.Cache.Count >= CacheLimit)
                this.Cache.RemoveAt(0);

            this.Cache.Add(TypeArguments, S);
            return S;
        }

        public sealed override bool CanSerializeType(Type Type)
        {
            return this.GetTypeArguments(Type) != null;
        }

        public sealed override void Set(FormatterSetProxy Formatter, object Obj)
        {
            var TypeArguments = this.GetTypeArguments(Obj.GetType());
            var S = this.GetSerializer(TypeArguments);

            var IsSingle = TypeArguments.Length == 1;
            Formatter.Set(nameof(IsSingle), IsSingle);

            if (IsSingle)
                Formatter.Set(nameof(TypeArguments), TypeArguments[0]);
            else
                Formatter.Set(nameof(TypeArguments), TypeArguments);
            S.Set(Formatter, Obj);
        }

        public sealed override object Get(FormatterGetProxy Formatter)
        {
            Type[] TypeArguments;
            var IsSingle = default(bool);
            IsSingle = Formatter.Get<bool>(nameof(IsSingle));
            if (IsSingle)
                TypeArguments = new[] { Formatter.Get<Type>(nameof(TypeArguments)) };
            else
                TypeArguments = Formatter.Get<Type[]>(nameof(TypeArguments));

            var S = this.GetSerializer(TypeArguments);
            return S.Get(Formatter);
        }

        public sealed override void Get(FormatterGetProxy Formatter, object Obj)
        {
            throw new NotSupportedException();
        }

        public Type SerializerType { get; }

        private const int CacheLimit = 10;

        private readonly OrderedDictionary<Type[], Serializer> Cache = new OrderedDictionary<Type[], Serializer>(new ArrayComparer<Type>());

        public static GenericSerializer Create(string Id, Type SerializerType, Func<Type, Type[]> GetTypeArgument)
        {
            return new DelegateGenericSerializer(Id, SerializerType, GetTypeArgument);
        }

        public class DelegateGenericSerializer : GenericSerializer
        {
            public DelegateGenericSerializer(string Id, Type SerializerType, Func<Type, Type[]> GetTypeArgument) : base(Id, SerializerType)
            {
                this.GetTypeArgumentDelegate = GetTypeArgument;
            }

            protected override Type[] GetTypeArguments(Type Type)
            {
                return this.GetTypeArgumentDelegate.Invoke(Type);
            }

            private readonly Func<Type, Type[]> GetTypeArgumentDelegate;
        }
    }
}

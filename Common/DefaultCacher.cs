using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Ks.Common
{
    public static class DefaultCacher<T>
    {

        public static void SetValue(T Value)
        {
            _Value.Value = Value;
        }

        private static readonly System.Threading.ThreadLocal<T> _Value = new System.Threading.ThreadLocal<T>(Initialize);

        private static T Initialize()
        {
            return DefaultCacher.CreateInstance<T>();
        }

        public static T Value => _Value.Value;
    }

    public static class DefaultCacher
    {
        private static readonly Dictionary<Type, object> Initializers = new Dictionary<Type, object>();

        public static void AddTypeInitializer<T>(Func<T> initializer)
        {
            Initializers[typeof(T)] = initializer;
        }

        public static T CreateInstance<T>()
        {
            if (Initializers.TryGetValue(typeof(T), out var initializer))
            {
                return ((Func<T>) initializer).Invoke();
            }

            return (T) typeof(T).CreateInstance();
        }

        static DefaultCacher()
        {
            AddTypeInitializer(() => RandomNumberGenerator.Create());
        }

    }
}

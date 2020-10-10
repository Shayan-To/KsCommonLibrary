using System.Collections.Generic;
using System;

namespace Ks.Common
{
    public class ObjectTraverser
    {
    }

    public class ObjectProxy
    {
        public void Reset()
        {
            this.Container = null;
            this.Type = null;
        }

        public void Set<T>(T Value)
        {
            Verify.True(this.Type == null & this.Container == null, "Reset before setting.");
            this.Type = typeof(T);

            ObjectContainer<T> C = null;
            if (this.Containers.TryGetValue(this.Type, out this.Container))
            {
                C = (ObjectContainer<T>) this.Container;
            }
            else
            {
                C = new ObjectContainer<T>();
                this.Container = C;
                this.Containers.Add(this.Type, this.Container);
            }

            C.Value = Value;
        }

        public T Get<T>()
        {
            Verify.True(this.Type != null & this.Container != null, "Set before getting.");
            Verify.True(this.Type == typeof(T), "The type set is different.");

            this.Type = typeof(T);
            var C = (ObjectContainer<T>) this.Container;

            return C.Value;
        }

        private Type Type;
        private object Container;
        private readonly Dictionary<Type, object> Containers = new Dictionary<Type, object>();

        private class ObjectContainer<T>
        {
            public T Value { get; set; }
        }
    }
}

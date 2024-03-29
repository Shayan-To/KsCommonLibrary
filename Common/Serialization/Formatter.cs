using System.Collections.Generic;

namespace Ks.Common
{
    public abstract partial class Formatter
    {

        // ToDo Should there be a machanism for fallback? Two suggestions:
        // 1. Put a way Get and Set of Serializer can tell that they cannot serialize the object.
        // 2. Put back CanSerializeObject so that we can check it before calling Get or Set.
        // Current design was chosen as it is simpler, and adds more restrictions (removes the unneccessary freedom (~Vocab)).

        /// <summary>
        /// This method has to be called at the end of every constructor of every derived class.
        /// </summary>
        protected void Initialize()
        {
            this.Serializers.LockCurrentElements();
        }

        protected abstract void OnGetStarted();
        protected abstract void OnGetFinished();
        protected abstract void OnGetEnterContext(string Name);
        protected abstract void OnGetExitContext(string Name);

        protected abstract void OnSetStarted();
        protected abstract void OnSetFinished();
        protected abstract void OnSetEnterContext(string Name);
        protected abstract void OnSetExitContext(string Name);

        private Serializer GetSerializer<T>(CNullable<T> Obj, bool IsGeneric)
        {
            var Type = typeof(T);
            if (!IsGeneric & Obj.HasValue)
            {
                Type = Obj.Value.GetType();
            }

            for (var I = this.Serializers.Count - 1; I >= 0; I--)
            {
                var S = this.Serializers[I];

                if (!S.CanSerializeType(Type))
                {
                    continue;
                }

                // We will also return the non-generic serializers for generic serialization. The users have to check whether the returned serializer is generic or not.

                return S;
            }
            return null;
        }

        private void SetImpl<T>(string Name, T Obj, bool IsGeneric)
        {
            var WasRunning = this.IsRunning;
            var IsRefType = !typeof(T).IsValueType & (typeof(T) != typeof(string));

            if (!WasRunning)
            {
                this.OnSetStarted();

                this.IsRunning = true;
                Assert.True(this.GetCache.Count == 0);
                Assert.True(this.SetCache.Count == 0);
                Assert.True(this.ObjectsCount == 0);
            }

            this.OnSetEnterContext(Name);

            try
            {
                var Id = this.ObjectsCount;
                if (IsRefType)
                {
                    var WasInCache = this.SetCache.TryGetValue(Obj, out var Tmp);

                    var IsDone = Tmp.Item2;
                    if (WasInCache)
                    {
                        Id = Tmp.Item1;
                    }

                    // We allow more than one serialization iterations on the same object. See the serializer for Object for a use case.

                    this.Set(nameof(Id), Id);

                    if (IsDone)
                    {
                        return;
                    }
                    else if (!WasInCache)
                    {
                        this.SetCache.Add(Obj, (Id, false));
                        this.ObjectsCount += 1;
                    }
                }

                var S = this.GetSerializer<T>(Obj, IsGeneric);
                if (IsGeneric)
                {
                    if (S is Serializer<T> ST)
                    {
                        ST.SetT(this.SetProxy, Obj);
                    }
                    else
                    {
                        S.Set(this.SetProxy, Obj);
                    }
                }
                else
                {
                    this.Set(nameof(Serializer), S);
                    S.Set(this.SetProxy, Obj);
                }

                if (IsRefType)
                {
                    Assert.True(this.SetCache.ContainsKey(Obj));
                    this.SetCache[Obj] = (Id, true);
                }
            }
            finally
            {
                this.OnSetExitContext(Name);

                if (!WasRunning)
                {
                    this.OnSetFinished();

                    this.IsRunning = false;
                    this.SetCache.Clear();
                    this.ObjectsCount = 0;
                }
            }
        }

        private T GetImpl<T>(string Name, CNullable<T> GObj, bool IsGeneric)
        {
            var WasRunning = this.IsRunning;
            var IsRefType = !typeof(T).IsValueType;

            // We allow T to be a value type. See the serializer for SerializationArrayChunk for a use case.

            if (!WasRunning)
            {
                this.OnGetStarted();

                this.IsRunning = true;
                Assert.True(this.GetCache.Count == 0);
                Assert.True(this.SetCache.Count == 0);
                Assert.True(this.ObjectsCount == 0);
            }

            this.OnGetEnterContext(Name);

            try
            {
                var Id = 0;
                if (IsRefType)
                {
                    Id = this.Get<int>(nameof(Id));

                    if (this.GetCache.TryGetValue(Id, out var Obj) && Obj != null)
                    {
                        return (T) Obj;
                    }

                    this.GetCache.Add(Id, null);
                }

                var S = default(Serializer);
                if (IsGeneric)
                {
                    S = this.GetSerializer<T>(default, true);
                }
                else
                {
                    S = this.Get<Serializer>(nameof(Serializer));
                }

                var ST = S as Serializer<T>;
                var R = default(T);

                if (IsGeneric & ST != null)
                {
                    if (GObj.HasValue)
                    {
                        ST.GetT(this.GetProxy, GObj.Value);
                        R = GObj.Value;
                    }
                    else
                    {
                        R = ST.GetT(this.GetProxy);
                    }
                }
                else if (GObj.HasValue)
                {
                    S.Get(this.GetProxy, GObj.Value);
                    R = GObj.Value;
                }
                else
                {
                    R = (T) S.Get(this.GetProxy);
                }

                if (IsRefType)
                {
                    var Obj = this.GetCache[Id];
                    if (Obj != null)
                    {
                        Verify.True(Obj == (object) R, "Two different objects returned for the same id.");
                    }
                    else
                    {
                        this.GetCache[Id] = R;
                    }
                }

                return R;
            }
            finally
            {
                this.OnGetExitContext(Name);

                if (!WasRunning)
                {
                    this.OnGetFinished();

                    this.IsRunning = false;
                    this.GetCache.Clear();
                    this.ObjectsCount = 0;
                }
            }
        }

        protected internal void Set<T>(string Name, T Obj)
        {
            this.SetImpl(Name, Obj, true);
        }

        protected internal void Set(string Name, object Obj)
        {
            this.SetImpl(Name, Obj, false);
        }

        protected internal T Get<T>(string Name)
        {
            return this.GetImpl<T>(Name, default, true);
        }

        protected internal object Get(string Name)
        {
            return this.GetImpl<object>(Name, default, false);
        }

        protected internal void Get<T>(string Name, T Obj)
        {
            this.GetImpl<T>(Name, Obj, true);
        }

        protected internal void Get(string Name, object Obj)
        {
            this.GetImpl(Name, new CNullable<object>(Obj), false);
        }

        public SerializerCollection Serializers { get; } = new SerializerCollection();

        private readonly FormatterSetProxy SetProxy;
        private readonly FormatterGetProxy GetProxy;

        private readonly Dictionary<object, (int, bool)> SetCache = new Dictionary<object, (int, bool)>(ReferenceEqualityComparer<object>.Instance);
        private readonly Dictionary<int, object> GetCache = new Dictionary<int, object>();
        private int ObjectsCount;
        private bool IsRunning;
    }
}

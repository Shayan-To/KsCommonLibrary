using System;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    partial class Formatter
    {
        public Formatter()
        {
            this.SetProxy = new FormatterSetProxy(this);
            this.GetProxy = new FormatterGetProxy(this);
            this.Serializers.Add(GenericSerializer.Create(nameof(Array), typeof(ArraySerializer<>), T =>
            {
                if (!T.IsArray || T.GetArrayRank() != 1)
                {
                    return null;
                }

                return new[] { T.GetElementType() };
            }));
            this.Serializers.Add(GenericSerializer.Create(nameof(List<object>), typeof(ListSerializer<>), T =>
            {
                Type IT = null;
                foreach (var I in T.GetInterfaces())
                {
                    if (I.IsGenericType && I.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        IT = I;
                        break;
                    }
                }
                return IT?.GetGenericArguments();
            }));
            this.Serializers.Add(GenericSerializer.Create(nameof(Dictionary<object, object>), typeof(DictionarySerializer<,>), T =>
           {
               Type IT = null;
               foreach (var I in T.GetInterfaces())
               {
                   if (I.IsGenericType && I.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                   {
                       IT = I;
                       break;
                   }
               }
               if (IT == null)
               {
                   return null;
               }

               T = IT.GetGenericArguments().Single();
               if (!T.IsGenericType || T.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
               {
                   return null;
               }

               return T.GetGenericArguments();
           }));
            this.Serializers.Add(GenericSerializer.Create(nameof(KeyValuePair<object, object>), typeof(KeyValuePairSerializer<,>), T =>
           {
               if (!T.IsGenericType || T.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
               {
                   return null;
               }

               return T.GetGenericArguments();
           }));

            this.Serializers.Add(Serializer<Type>.Create(nameof(Type), F =>
            {
                var FullName = default(string);
                FullName = F.Get<string>(nameof(FullName));
                return Type.GetType(FullName); // ToDo This method wants the assembly-qualified name. See how it's done using the full name.
            }, null, (F, O) =>
            {
                F.Set(nameof(O.FullName), O.FullName);
            }));

            this.Serializers.Add(Serializer<Serializer>.Create(nameof(Serializer), F =>
            {
                var Id = default(string);
                Id = F.Get<string>(nameof(Id));
                return F.Formatter.Serializers[Id];
            }, null, (F, O) =>
            {
                F.Set(nameof(O.Id), O.Id);
            }));

            this.Serializers.Add(Serializer<object>.Create(nameof(Object), F =>
            {
                var IsObject = default(bool);
                IsObject = F.Get<bool>(nameof(IsObject));
                if (IsObject)
                {
                    return new object();
                }

                return F.Get(null);
            }, null, (F, O) =>
            {
                var IsObject = O.GetType() == typeof(object);
                F.Set(nameof(IsObject), IsObject);

                if (!IsObject)
                {
                    F.Set(null, O);
                }
            }));

            this.Initialize();
        }
    }
}

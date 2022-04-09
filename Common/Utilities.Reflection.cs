using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;

using Reflect = System.Reflection;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class Reflection
        {

            public static IEnumerable<Reflect.Assembly> GetAllAccessibleAssemblies()
            {
                return Enumerable.Concat(Reflect.Assembly.GetEntryAssembly().GetRecursiveReferencedAssemblies(), AssemblyLoadContext.All.SelectMany(alc => alc.Assemblies)).Distinct();
            }

            public static IEnumerable<Reflect.MethodInfo> GetAllMethods(IEnumerable<Reflect.Assembly> Assemblies = null)
            {
                if (Assemblies == null)
                {
                    Assemblies = GetAllAccessibleAssemblies();
                }

                foreach (var Ass in Assemblies)
                {
                    foreach (var Type in Ass.GetTypes())
                    {
                        foreach (var Method in Type.GetMethods(Reflect.BindingFlags.Static | Reflect.BindingFlags.Instance | Reflect.BindingFlags.Public | Reflect.BindingFlags.NonPublic))
                        {
                            yield return Method;
                        }
                    }
                }
            }

            public static IEnumerable<Type> GetAllTypes(IEnumerable<Reflect.Assembly> Assemblies = null)
            {
                if (Assemblies == null)
                {
                    Assemblies = GetAllAccessibleAssemblies();
                }

                foreach (var Ass in Assemblies)
                {
                    foreach (var Type in Ass.GetTypes())
                    {
                        yield return Type;
                    }
                }
            }

            public static IEnumerable<Type> GetAllTypesDerivedFrom(Type Base, IEnumerable<Reflect.Assembly> Assemblies = null)
            {
                if (Assemblies == null)
                {
                    Assemblies = GetAllAccessibleAssemblies();
                }

                Base = Base.GetGenericTypeDef();

                foreach (var Type in GetAllTypes(Assemblies))
                {
                    if (Type.GetBaseTypes().Concat(Type.GetInterfaces()).Select(t => t.GetGenericTypeDef()).Contains(Base))
                    {
                        yield return Type;
                    }
                }
            }
        }
    }
}

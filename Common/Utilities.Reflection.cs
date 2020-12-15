using System;
using System.Collections.Generic;

using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class Reflection
        {

            public static IEnumerable<Reflect.Assembly> GetAllAccessibleAssemblies()
            {
                return Reflect.Assembly.GetEntryAssembly().GetRecursiveReferencedAssemblies();
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

                foreach (var Type in GetAllTypes(Assemblies))
                {
                    if (Base.IsAssignableFrom(Type))
                    {
                        yield return Type;
                    }
                }
            }
        }
    }
}

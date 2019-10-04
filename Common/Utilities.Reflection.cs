using Mono;
using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common
    {
        partial class Utilities
        {
            public class Reflection
            {
                private Reflection()
                {
                    throw new NotSupportedException();
                }

                public static IEnumerable<System.Reflect.Assembly> GetAllAccessibleAssemblies()
                {
                    return System.Reflect.Assembly.GetEntryAssembly().GetRecursiveReferencedAssemblies();
                }

                public static IEnumerable<System.Reflect.MethodInfo> GetAllMethods(IEnumerable<System.Reflect.Assembly> Assemblies = null)
                {
                    if (Assemblies == null)
                        Assemblies = GetAllAccessibleAssemblies();
                    foreach (var Ass in Assemblies)
                    {
                        foreach (Type Type in Ass.GetTypes())
                        {
                            foreach (System.Reflect.MethodInfo Method in Type.GetMethods(System.Reflect.BindingFlags.Static | System.Reflect.BindingFlags.Instance | System.Reflect.BindingFlags.Public | System.Reflect.BindingFlags.NonPublic))
                                yield return Method;
                        }
                    }
                }

                public static IEnumerable<Type> GetAllTypes(IEnumerable<System.Reflect.Assembly> Assemblies = null)
                {
                    if (Assemblies == null)
                        Assemblies = GetAllAccessibleAssemblies();
                    foreach (var Ass in Assemblies)
                    {
                        foreach (Type Type in Ass.GetTypes())
                            yield return Type;
                    }
                }

                public static IEnumerable<Type> GetAllTypesDerivedFrom(Type Base, IEnumerable<System.Reflect.Assembly> Assemblies = null)
                {
                    if (Assemblies == null)
                        Assemblies = GetAllAccessibleAssemblies();
                    foreach (Type Type in GetAllTypes(Assemblies))
                    {
                        if (Base.IsAssignableFrom(Type))
                            yield return Type;
                    }
                }

                [Sample()]
                public static void GetAllTypesDerivedFrom(Mono.Cecil.TypeDefinition Base, System.Reflect.Assembly Assembly = null)
                {
                    var Helper = CecilHelper.Instance;

                    if (Assembly == null)
                        Assembly = System.Reflect.Assembly.GetEntryAssembly();

                    foreach (var A in Helper.GetReferencedAssemblies(Helper.Convert(Assembly)))
                    {
                        foreach (var M in A.Modules)
                        {
                            foreach (var T in M.Types)
                            {
                                if (Helper.IsBaseTypeOf(Base, T))
                                    Console.WriteLine(T);
                            }
                        }
                    }
                }
            }
        }
    }
}

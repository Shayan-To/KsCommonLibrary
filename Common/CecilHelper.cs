using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ks.Common
{
    public class CecilHelper
    {
        public bool IsBaseTypeOf(Mono.Cecil.TypeDefinition Base, Mono.Cecil.TypeDefinition Derived)
        {
            if (Base.IsInterface)
            {
                foreach (var I in Derived.Interfaces)
                {
                    var ID = I.InterfaceType.Resolve();
                    if (this.Equals(Base, ID))
                    {
                        return true;
                    }
                }
            }
            else
            {
                while (Derived != null)
                {
                    if (this.Equals(Base, Derived))
                    {
                        return true;
                    }

                    Base = Derived.BaseType?.Resolve();
                }
            }

            return false;
        }

        public bool Equals(Mono.Cecil.TypeDefinition Type1, Mono.Cecil.TypeDefinition Type2)
        {
            return (Type1.FullName == Type2.FullName) & (Type1.Module.FileName == Type2.Module.FileName);
        }

        public System.Reflection.MethodInfo Convert(Mono.Cecil.MethodDefinition Method)
        {
            var Type = this.Convert(Method.DeclaringType);
            return Type.GetMethod(Method.Name, Method.Parameters.Select(P => this.Convert(P.ParameterType.Resolve())).ToArray());
        }

        public Type Convert(Mono.Cecil.TypeDefinition Type)
        {
            var Assembly = this.Convert(Type.Module.Assembly);
            return Assembly.GetType(Type.FullName);
        }

        public System.Reflection.Assembly Convert(Mono.Cecil.AssemblyDefinition Assembly)
        {
            return System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(Assembly.FullName));
        }

        public System.Reflection.Assembly Convert(Mono.Cecil.AssemblyNameReference AssemblyName)
        {
            return System.Reflection.Assembly.Load(new System.Reflection.AssemblyName(AssemblyName.FullName));
        }

        public Mono.Cecil.TypeDefinition Convert(Type Type)
        {
            var FName = Type.FullName;
            foreach (var M in this.Convert(Type.Assembly).Modules)
            {
                var T = M.GetType(FName);
                if (T != null)
                {
                    return T;
                }
            }
            throw new Exception();
        }

        public Mono.Cecil.AssemblyDefinition Convert(System.Reflection.Assembly Assembly)
        {
            return Mono.Cecil.AssemblyDefinition.ReadAssembly(Assembly.Location);
        }

        public Mono.Cecil.AssemblyNameReference[] GetReferencedAssemblyNames(Mono.Cecil.AssemblyDefinition Assembly)
        {
            return this.GetRawReferencedAssemblyNames(Assembly).ToArray();
        }

        internal IEnumerable<Mono.Cecil.AssemblyNameReference> GetRawReferencedAssemblyNames(Mono.Cecil.AssemblyDefinition Assembly)
        {
            this.AssemblyNames.Clear();

            foreach (var M in Assembly.Modules)
            {
                foreach (var A in M.AssemblyReferences)
                {
                    this.AssemblyNames.Add(A);
                }
            }

            return this.AssemblyNames;
        }

        public Mono.Cecil.AssemblyDefinition[] GetReferencedAssemblies(Mono.Cecil.AssemblyDefinition Assembly)
        {
            return this.GetRawReferencedAssemblies(Assembly).ToArray();
        }

        internal IEnumerable<Mono.Cecil.AssemblyDefinition> GetRawReferencedAssemblies(Mono.Cecil.AssemblyDefinition Assembly)
        {
            this.Assemblies.Clear();

            foreach (var M in Assembly.Modules)
            {
                foreach (var A in M.AssemblyReferences)
                {
                    this.Assemblies.Add(this.Resolver.Resolve(A));
                }
            }

            return this.Assemblies;
        }

        public Mono.Cecil.AssemblyDefinition[] GetRecursiveReferencedAssemblies(Mono.Cecil.AssemblyDefinition Assembly)
        {
            return this.GetRawRecursiveReferencedAssemblies(Assembly).ToArray();
        }

        internal IEnumerable<Mono.Cecil.AssemblyDefinition> GetRawRecursiveReferencedAssemblies(Mono.Cecil.AssemblyDefinition Assembly)
        {
            this.Assemblies.Clear();
            this.CollectReferences(Assembly);
            return this.Assemblies;
        }

        private void CollectReferences(Mono.Cecil.AssemblyDefinition Assembly)
        {
            if (!this.Assemblies.Add(Assembly))
            {
                return;
            }

            foreach (var M in Assembly.Modules)
            {
                foreach (var A in M.AssemblyReferences)
                {
                    this.CollectReferences(this.Resolver.Resolve(A));
                }
            }
        }

        public static CecilHelper Instance { get; } = new CecilHelper();

        private readonly Mono.Cecil.DefaultAssemblyResolver Resolver = new Mono.Cecil.DefaultAssemblyResolver();
        private readonly HashSet<Mono.Cecil.AssemblyDefinition> Assemblies = new HashSet<Mono.Cecil.AssemblyDefinition>();
        private readonly HashSet<Mono.Cecil.AssemblyNameReference> AssemblyNames = new HashSet<Mono.Cecil.AssemblyNameReference>();
    }
}

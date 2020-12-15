using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Ks
{
    namespace Common
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class InteractiveRunnableAttribute : Attribute
        {
            public InteractiveRunnableAttribute(bool ShouldBeRun = false)
            {
                this._ShouldBeRun = ShouldBeRun;
            }

            [DebuggerHidden()]
            public static void RunTestMethods(IEnumerable<MethodInfo> Methods, bool JustTrue = true)
            {
                var FullNameSelector = new Func<MethodInfo, string>(M => $"{M.DeclaringType.FullName}.{M.Name}");
                var List = Methods.WithCustomAttribute<InteractiveRunnableAttribute>()
                        .Where(MA =>
                        {
                            var M = MA.Method;

                            if (!M.IsStatic)
                            {
                                ConsoleUtilities.WriteColored($"Skipping {FullNameSelector.Invoke(M)}... Instance method.", ConsoleColor.Red);
                                Console.WriteLine();
                                return false;
                            }

                            if (M.GetParameters().Length != 0)
                            {
                                ConsoleUtilities.WriteColored($"Skipping {FullNameSelector.Invoke(M)}... Accepts arguments.", ConsoleColor.Red);
                                Console.WriteLine();
                                return false;
                            }

                            return JustTrue.Implies(MA.Attribute.ShouldBeRun);
                        })
                        .Select(MA => MA.Method);
                var ChoiceReader = new ConsoleListChoiceReader<MethodInfo>(List, FullNameSelector);

                while (true)
                {
                    var M = ChoiceReader.ReadChoice();
                    if (M == null)
                        break;
                    M.Invoke(null, Utilities.Typed<object>.EmptyArray);
                }

                Console.WriteLine();
                ConsoleUtilities.WriteColored("Done.", ConsoleColor.Green);
                Console.WriteLine();
            }

            [Sample()]
            private static void CecilRunTestMethods(bool JustTrue = true)
            {
                var Helper = CecilHelper.Instance;
                var AttributeType = Helper.Convert(typeof(InteractiveRunnableAttribute));
                foreach (var A in Helper.GetReferencedAssemblies(Helper.Convert(System.Reflection.Assembly.GetEntryAssembly())))
                {
                    foreach (var M in A.Modules)
                    {
                        foreach (var T in M.Types)
                        {
                            foreach (var Mth in T.Methods)
                            {
                                foreach (var CA in Mth.CustomAttributes.Where(Att => Helper.Equals(Att.AttributeType.Resolve(), AttributeType)))
                                {
                                    if (Mth.Parameters.Count != 0)
                                    {
                                        ConsoleUtilities.WriteColored($"Skipping {T.FullName}.{M.Name}... Accepts arguments.", ConsoleColor.Red);
                                        Console.ReadKey(true);
                                        Console.WriteLine();

                                        continue;
                                    }

                                    var Att = new InteractiveRunnableAttribute((bool)CA.ConstructorArguments[0].Value);
                                    if (!JustTrue | Att.ShouldBeRun)
                                    {
                                        if (ConsoleUtilities.ReadYesNo($"Run {T.FullName}.{M.Name}? (Y/N)"))
                                            Helper.Convert(Mth).Invoke(null, Utilities.Typed<object>.EmptyArray);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            [DebuggerHidden()]
            public static void RunTestMethods(bool JustTrue = true)
            {
                RunTestMethods(new[] { Assembly.GetEntryAssembly() }, JustTrue);
            }

            [DebuggerHidden()]
            public static void RunTestMethods(IEnumerable<Assembly> Assemblies, bool JustTrue = true)
            {
                RunTestMethods(Utilities.Reflection.GetAllMethods(Assemblies), JustTrue);
            }

            private readonly bool _ShouldBeRun;

            public bool ShouldBeRun
            {
                get
                {
                    return this._ShouldBeRun;
                }
            }
        }
    }
}

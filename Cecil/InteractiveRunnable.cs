using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Ks.Cecil
{
    public class InteractiveRunnable
    {
        [Sample()]
        public static void RunTestMethods(bool JustTrue = true)
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

                                var Att = new InteractiveRunnableAttribute((bool) CA.ConstructorArguments[0].Value);
                                if (!JustTrue | Att.ShouldBeRun)
                                {
                                    if (ConsoleUtilities.ReadYesNo($"Run {T.FullName}.{M.Name}? (Y/N)"))
                                    {
                                        Helper.Convert(Mth).Invoke(null, Utilities.Typed<object>.EmptyArray);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

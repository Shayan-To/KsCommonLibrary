using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Ks.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InteractiveRunnableAttribute : Attribute
    {
        public InteractiveRunnableAttribute(bool ShouldBeRun = false)
        {
            this.ShouldBeRun = ShouldBeRun;
        }

        [DebuggerHidden()]
        public static void RunTestMethods(IEnumerable<MethodInfo> Methods, bool JustTrue = true)
        {
            var FullNameSelector = new Func<MethodInfo, string>(M => $"{M.DeclaringType.FullName}.{M.Name}");
            var List = Methods.WithCustomAttribute<InteractiveRunnableAttribute>(true)
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
                {
                    break;
                }

                M.Invoke(null, Utilities.Typed<object>.EmptyArray);
            }

            Console.WriteLine();
            ConsoleUtilities.WriteColored("Done.", ConsoleColor.Green);
            Console.WriteLine();
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

        public bool ShouldBeRun { get; }
    }
}

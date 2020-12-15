using System;
using System.Linq;
using System.Reflection;

using Ks.Common;

namespace Ks.Tests
{
    public static class InteractiveTests
    {
        public static void Run()
        {
            ConsoleUtilities.Initialize();
            var assemblies = Utilities.Reflection.GetAllAccessibleAssemblies()
                .CatchExceptions()
#pragma warning disable SYSLIB0005 // Type or member is obsolete
                .Where(a => !a.GlobalAssemblyCache);
#pragma warning restore SYSLIB0005 // Type or member is obsolete
            var abc = assemblies
                .Where(a => !a.GetCustomAttributes<AssemblyMetadataAttribute>(true).Any(a => a.Key == ".NETFrameworkAssembly") &&
                            !a.GetCustomAttributes<AssemblyCompanyAttribute>(true).Any(a => a.Company == "Microsoft Corporation"));
            InteractiveRunnableAttribute.RunTestMethods(assemblies);
            ConsoleUtilities.Pause();
        }
    }
}

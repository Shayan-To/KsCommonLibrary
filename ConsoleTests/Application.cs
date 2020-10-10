using System;

using Ks.Common;

namespace Ks.ConsoleTests
{
    public static class Application
    {
        public static void Main()
        {
            ConsoleUtilities.Initialize();
            InteractiveRunnableAttribute.RunTestMethods();
            ConsoleUtilities.Pause();
        }
    }
}

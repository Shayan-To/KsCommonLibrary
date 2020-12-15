using System;

using Ks.Common;

namespace Ks.Tests
{
    public static class Program
    {
        public static void Main()
        {
            ConsoleUtilities.Initialize();
            InteractiveRunnableAttribute.RunTestMethods();
            ConsoleUtilities.Pause();
        }
    }
}

using System;
using Ks.Common;

namespace Ks.ConsoleTests
{
    public abstract class Application
    {
        private Application()
        {
            throw new NotSupportedException();
        }

        public static void Main()
        {
            ConsoleUtilities.Initialize();
            InteractiveRunnableAttribute.RunTestMethods();
            ConsoleUtilities.Pause();
        }
    }
}

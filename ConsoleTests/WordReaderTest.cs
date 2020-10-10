using System;

using Ks.Common;

namespace Ks.ConsoleTests
{
    public static class WordReaderTest
    {
        [InteractiveRunnable(true)]
        public static void Start()
        {
            var Reader = new WordReader(Console.In);

            while (true)
            {
                var W = Reader.ReadWord();
                Console.WriteLine(W);
            }
        }
    }
}

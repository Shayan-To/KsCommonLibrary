using System;
using Ks.Common;

namespace Ks.ConsoleTests
{
    public class WordReaderTest
    {
        [InteractiveRunnable(true)]
        public static void Start()
        {
            var Reader = new WordReader(Console.In);

            do
            {
                var W = Reader.ReadWord();
                Console.WriteLine(W);
            }
            while (true);
        }
    }
}

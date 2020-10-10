using System;
using System.Linq;

using Ks.Common;

namespace Ks.ConsoleTests
{
    public static class ListChoiceReaderTest
    {
        [InteractiveRunnable(true)]
        public static void Start()
        {
            var Rand = new Random();
            var L = Utilities.Collections.Range(45).Select(I => (int?) Rand.Next());
            var ChoiceReader = new ConsoleListChoiceReader<int?>(L);

            while (true)
            {
                var I = ChoiceReader.ReadChoice();
                if (!I.HasValue)
                {
                    break;
                }

                Console.WriteLine($"{I} was chosen!!");
            }
        }
    }
}

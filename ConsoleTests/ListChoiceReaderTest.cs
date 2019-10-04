using System;
using Ks.Common;

namespace Ks.ConsoleTests
{
    public class ListChoiceReaderTest
    {
        [InteractiveRunnable(true)]
        public static void Start()
        {
            var Rand = new Random();
            var L = Utilities.Collections.Range(45).Select<int?>(I => Rand.Next());
            var ChoiceReader = new ConsoleListChoiceReader<int?>(L);

            do
            {
                var I = ChoiceReader.ReadChoice();
                if (!I.HasValue)
                    break;
                Console.WriteLine($"{I} was chosen!!");
            }
            while (true);
        }
    }
}

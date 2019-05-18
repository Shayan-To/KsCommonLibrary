using System.Collections.Generic;
using System;

namespace Ks
{
    namespace Common
    {
        public class ConsoleUtilities
        {
            private ConsoleUtilities()
            {
                throw new NotSupportedException();
            }

            public const ConsoleColor DefaultBackColor = ConsoleColor.White;
            public const ConsoleColor DefaultForeColor = ConsoleColor.Black;

            public static void Initialize(bool SetInvariantCulture = true)
            {
                if (SetInvariantCulture)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
                }

                Console.ForegroundColor = DefaultForeColor;
                Console.BackgroundColor = DefaultBackColor;
                Console.Clear();
            }

            public static void WriteColored(string Value, ConsoleColor BackColor = DefaultBackColor, ConsoleColor ForeColor = DefaultForeColor)
            {
                var PBackColor = Console.BackgroundColor;
                var PForeColor = Console.ForegroundColor;
                Console.BackgroundColor = BackColor;
                Console.ForegroundColor = ForeColor;
                Console.Write(Value);
                Console.BackgroundColor = PBackColor;
                Console.ForegroundColor = PForeColor;
            }

            public static bool ReadYesNo(string Prompt)
            {
                WriteColored(Prompt, ConsoleColor.Green);

                ConsoleKey K = default(ConsoleKey);
                do
                    K = Console.ReadKey(true).Key;
                while (!((K == ConsoleKey.Y) | (K == ConsoleKey.N)));

                var Res = K == ConsoleKey.Y;

                WriteColored(Res ? " Y" : " N");
                Console.WriteLine();

                return Res;
            }

            public static void WriteExceptionData(Exception Ex)
            {
                var Bl = true;
                do
                {
                    WriteColored($"{(Bl ? "Exception" : "Inner exception")}: ".PadRight(0, '-'), ConsoleColor.Yellow);
                    Bl = false;
                    Console.WriteLine();
                    WriteColored($"Type: {Ex.GetType().FullName}");
                    WriteColored($"Message: {Ex.Message}");
                    WriteColored($"StackTrace: {Ex.StackTrace}");
                    Console.WriteLine();
                    Ex = Ex.InnerException;
                }
                while (Ex != null);
            }

            public static void WriteTypes(IEnumerable<Type> Types)
            {
                foreach (var T in Types)
                    Console.WriteLine(T.FullName + "\t" + T.Attributes.ToString());
            }

            public static void Pause()
            {
                if (Environment.UserInteractive)
                {
                    WriteColored("Press any key to continue...", ConsoleColor.Green);
                    Console.ReadKey(true);
                    Console.WriteLine();
                }
            }
        }
    }
}

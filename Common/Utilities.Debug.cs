using System;
using System.Diagnostics;
using System.Text;

namespace Ks.Common
{
    partial class Utilities
    {
        public static class Debug
        {

            public static string CompactStackTrace(int Count)
            {
                var ST = new StackTrace(true);
                var F = ST.GetFrames();

                if (Count >= F.Length)
                {
                    Count = F.Length - 1;
                }

                var R = new StringBuilder();
                for (var I = 0; I < Count; I++)
                {
                    if (I > 1)
                    {
                        R.Append('>');
                    }

                    var M = F[I].GetMethod();
                    R.Append(M.DeclaringType.Name)
                     .Append('.')
                     .Append(M.Name)
                     .Append(M.GetParameters().Length)
                     .Append(':')
                     .Append(F[I].GetFileLineNumber());
                }

                return R.ToString();
            }

            public static void PrintCallInformation(params object[] Args)
            {
                var R = new StringBuilder();

                var F = new StackFrame(1);
                var M = F.GetMethod();
                var P = M.GetParameters();
                F = new StackFrame(2, true);

                if ((Args.Length != 0) & (P.Length != Args.Length))
                {
                    throw new ArgumentException();
                }

                R.Append(F.GetFileLineNumber())
                 .Append(":")
                 .Append(M.DeclaringType.Name)
                 .Append('.')
                 .Append(M.Name)
                 .Append('_')
                 .Append(P.Length)
                 .Append('(');

                if (Args.Length == 0)
                {
                    if (P.Length != 0)
                    {
                        R.Append("...");
                    }
                }
                else
                {
                    var Bl = true;
                    for (var I = 0; I < Args.Length; I++)
                    {
                        if (Bl)
                        {
                            Bl = false;
                        }
                        else
                        {
                            R.Append(", ");
                        }

                        R.Append(P[I].Name).Append('=').Append(Args[I]);
                    }
                }

                R.Append(')').AppendLine();

                Console.Write(R.ToString());
            }

            public static string GetCallInformation(params object[] Args)
            {
                var R = new StringBuilder();

                var F = new StackFrame(1);
                var M = F.GetMethod();
                var P = M.GetParameters();
                F = new StackFrame(2, true);

                if ((Args.Length != 0) & (P.Length != Args.Length))
                {
                    throw new ArgumentException();
                }

                R.Append(F.GetFileLineNumber())
                 .Append(":")
                 .Append(M.DeclaringType.Name)
                 .Append('.')
                 .Append(M.Name)
                 .Append(P.Length)
                 .Append('(');

                if (Args.Length == 0)
                {
                    if (P.Length != 0)
                    {
                        R.Append("...");
                    }
                }
                else
                {
                    var Bl = true;
                    foreach (var A in Args)
                    {
                        if (Bl)
                        {
                            Bl = false;
                        }
                        else
                        {
                            R.Append(", ");
                        }

                        R.Append(A);
                    }
                }

                R.Append(')').AppendLine();

                return R.ToString();
            }
        }
    }
}

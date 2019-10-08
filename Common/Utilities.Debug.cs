using System.Diagnostics;
using System;
using System.Text;
using Media = System.Windows.Media;
using Reflect = System.Reflection;
using SIO = System.IO;

namespace Ks
{
    namespace Common
    {
        partial class Utilities
        {
            public class Debug
            {
                private Debug()
                {
                    throw new NotSupportedException();
                }

                public static void ShowMessageBox(string text, string caption = "")
                {
                    System.Windows.Forms.MessageBox.Show(text, caption);
                }

                public static string CompactStackTrace(int Count)
                {
                    var ST = new StackTrace(true);
                    var F = ST.GetFrames();

                    if (Count >= F.Length)
                        Count = F.Length - 1;

                    var R = new StringBuilder();
                    for (int I = 0; I < Count; I++)
                    {
                        if (I > 1)
                            R.Append('>');
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
                        throw new ArgumentException();

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
                            R.Append("...");
                    }
                    else
                    {
                        var Bl = true;
                        for (int I = 0; I < Args.Length; I++)
                        {
                            if (Bl)
                                Bl = false;
                            else
                                R.Append(", ");
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
                        throw new ArgumentException();

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
                            R.Append("...");
                    }
                    else
                    {
                        var Bl = true;
                        foreach (var A in Args)
                        {
                            if (Bl)
                                Bl = false;
                            else
                                R.Append(", ");
                            R.Append(A);
                        }
                    }

                    R.Append(')').AppendLine();

                    return R.ToString();
                }
            }
        }
    }
}

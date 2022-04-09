using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Ks.Common
{
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
    public class Verify
    {
        [DebuggerHidden()]
        public static void NonNullArg<T>([NotNull] T O, string Name = null, string Message = null) where T : class
        {
            if (O == null)
            {
                if (Name == null)
                {
                    throw new ArgumentNullException();
                }
                else if (Message == null)
                {
                    throw new ArgumentNullException(Name);
                }
                else
                {
                    throw new ArgumentNullException(Name, Message);
                }
            }
        }

        [DebuggerHidden()]
        public static void TrueArg([DoesNotReturnIf(false)] bool T, string Name = null, string Message = null)
        {
            if (!T)
            {
                if (Name == null & Message == null)
                {
                    throw new ArgumentException();
                }
                else
                {
                    throw new ArgumentException(Message, Name);
                }
            }
        }

        [DebuggerHidden()]
        public static void RangeArg<T>(T Start, T V, T End, string Name = null, string Message = null) where T : IComparable<T>
        {
            if (Start.CompareTo(V) > 0 || V.CompareTo(End) > 0)
            {
                if (Name == null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (Message == null)
                {
                    throw new ArgumentOutOfRangeException(Name, V, string.Format("Argument must be between '{0}' and '{1}'.", Start, End));
                }
                else
                {
                    throw new ArgumentOutOfRangeException(Name, V, Message);
                }
            }
        }

        [DebuggerHidden()]
        public static void FalseArg([DoesNotReturnIf(true)] bool T, string Name = null, string Message = null)
        {
            if (T)
            {
                if (Name == null & Message == null)
                {
                    throw new ArgumentException();
                }
                else
                {
                    throw new ArgumentException(Message, Name);
                }
            }
        }

        [DebuggerHidden()]
        public static void NonNull<T>([NotNull] T O, string Name = null) where T : class
        {
            if (O == null)
            {
                if (Name == null)
                {
                    throw new NullReferenceException(string.Format("Object reference '{0}' not set to an instance of an object.", Name));
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
        }

        [DebuggerHidden()]
        public static void True([DoesNotReturnIf(false)] bool T, string Message = null)
        {
            if (!T)
            {
                throw new InvalidOperationException(Message);
            }
        }

        [DebuggerHidden()]
        public static void False([DoesNotReturnIf(true)] bool T, string Message = null)
        {
            if (T)
            {
                throw new InvalidOperationException(Message);
            }
        }

        [DebuggerHidden()]
        [DoesNotReturn]
        public static InvalidOperationException Fail(string Message = null)
        {
            throw new InvalidOperationException(Message);
        }

        [DebuggerHidden()]
        [DoesNotReturn]
        public static ArgumentException FailArg(string Name = null, string Message = null)
        {
            if (Name == null & Message == null)
            {
                throw new ArgumentException();
            }
            else
            {
                throw new ArgumentException(Message, Name);
            }
        }
    }
}

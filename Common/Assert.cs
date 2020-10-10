using System.Diagnostics;

namespace Ks.Common
{
    public class Assert
    {
        [DebuggerHidden()]
        public static void NonNull<T>(T O, string Name = null) where T : class
        {
            if (O == null)
            {
                if (Name != null)
                {
                    Fail(string.Format("Object reference '{0}' not set to an instance of an object.", Name));
                }
                else
                {
                    Fail("Object reference not set to an instance of an object.");
                }
            }
        }

        [DebuggerHidden()]
        public static void True(bool T, string Message = null)
        {
            if (!T)
            {
                Fail(Message);
            }
        }

        [DebuggerHidden()]
        public static void False(bool T, string Message = null)
        {
            if (T)
            {
                Fail(Message);
            }
        }

        [DebuggerHidden()]
        public static void Fail(string Message = null)
        {
            throw new AssertionException(Message);
        }
    }
}

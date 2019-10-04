using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Ks
{
    namespace Ks.Common.Win32
    {
        public abstract class Common
        {
            private Common()
            {
                throw new NotSupportedException();
            }

            // Windows Data Types: https://msdn.microsoft.com/en-us/library/windows/desktop/aa383751(v=vs.85).aspx

            internal static void VerifyError()
            {
                var ErrorCode = Marshal.GetLastWin32Error();
                if (ErrorCode != 0)
                    throw new Win32Exception(ErrorCode);
            }

            internal static void ThrowError()
            {
                throw new Win32Exception();
            }
        }
    }
}

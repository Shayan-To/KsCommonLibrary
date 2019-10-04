using System.Threading.Tasks;
using Xunit;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualBasic;
using Xunit.Assert;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Xml.Linq;
using Ks.Common;
using FsCheck.Xunit;

namespace Ks.Tests
{
    namespace Common
    {
        partial class Utilities_Tests
        {
            public class Math
            {
                [Property()]
                public void SquareRootConsistencyCheck(int N)
                {
                    if (N < 0)
                    {
                        Assert.Throws<ArgumentException>(() => Utilities.Math.SquareRoot(N));
                        return;
                    }

                    var T = Utilities.Math.SquareRoot(N);
                    Assert.Equal(N, (T.Root * T.Root) + T.Reminder);
                    Assert.False(T.Root < 0, "Root must be non-negative.");
                    Assert.False(T.Reminder < 0, "Root must be non-negative.");
                    Assert.True(T.Reminder < (((T.Root + 1) * (T.Root + 1)) - (T.Root * T.Root)), "Reminder is too large.");
                }

                [Property()]
                public void SquareRootLConsistencyCheck(long N)
                {
                    if (N < (long)0)
                    {
                        Assert.Throws<ArgumentException>(() => Utilities.Math.SquareRootL(N));
                        return;
                    }

                    var T = Utilities.Math.SquareRootL(N);
                    Assert.Equal(N, (T.Root * T.Root) + T.Reminder);
                    Assert.False(T.Root < (long)0, "Root must be non-negative.");
                    Assert.False(T.Reminder < (long)0, "Root must be non-negative.");
                    Assert.True(T.Reminder < (((T.Root + (long)1) * (T.Root + (long)1)) - (T.Root * T.Root)), "Reminder is too large.");
                }

                [Property()]
                public void MultLongTo128USmallCheck(uint A, uint B)
                {
                    var T = Utilities.Math.MultLongTo128U((ulong)A, (ulong)B);
                    Assert.Equal(System.Convert.ToUInt64(A) * System.Convert.ToUInt64(B), T.Low);
                }

                [Property()]
                public void MultLongTo128UBigCheck(uint A, uint B)
                {
                    var T = Utilities.Math.MultLongTo128U((ulong)A, (ulong)B);
                    var T2 = new System.Numerics.BigInteger(A) * new System.Numerics.BigInteger(B);
                    Assert.Equal(T2.ToByteArray().PadEnd(16), BitConverter.GetBytes(T.Low).Concat(BitConverter.GetBytes(T.High)));
                }
            }
        }
    }
}

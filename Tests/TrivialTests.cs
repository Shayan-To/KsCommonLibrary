using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using FsCheck.Xunit;

using Ks.Common;

using Xunit;

using Assert = Xunit.Assert;

namespace Ks.Tests
{
    public class TrivialTests
    {
        [Fact()]
        public void IsTrue()
        {
            Assert.True(true);
        }

        [Fact()]
        public void IsNotFalse()
        {
            Assert.False(false);
        }

        [Fact()]
        public void DoNothing()
        {
            Utilities.DoNothing();
        }
    }
}

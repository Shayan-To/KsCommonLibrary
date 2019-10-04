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

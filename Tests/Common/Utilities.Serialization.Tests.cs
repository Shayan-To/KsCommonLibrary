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

namespace Ks.Tests.Common
{
    partial class Utilities_Tests
    {
        public class Serialization
        {
            [Property()]
            public void ListToString_ListFromString_Consistency(string[] List)
            {
                for (var I = 0; I < List.Length; I++)
                {
                    if (List[I] == null)
                    {
                        List[I] = "";
                    }
                }
                var Serialized = Utilities.Serialization.ListToString(List);
                var Deserialized = Utilities.Serialization.ListFromString(Serialized);
                Assert.Equal(List, Deserialized);
            }

            [Fact()]
            public void ListFromString()
            {
                Assert.Equal(new string[] { }, Utilities.Serialization.ListFromString("{}"));
                Assert.Equal(new string[] { "" }, Utilities.Serialization.ListFromString("{,}"));
                Assert.Equal(new string[] { "AA", "BB" }, Utilities.Serialization.ListFromString("{AA,BB,}"));
                Assert.Equal(new string[] { "{,}", "{\r}\n" }, Utilities.Serialization.ListFromString(@"{\{\,\},\{\r\}\n,}"));
            }

            [Fact()]
            public void ListToString()
            {
                Assert.Equal("{}", Utilities.Serialization.ListToString(new string[] { }));
                Assert.Equal("{,}", Utilities.Serialization.ListToString(new[] { "" }));
                Assert.Equal("{AA,BB,}", Utilities.Serialization.ListToString(new[] { "AA", "BB" }));
                Assert.Equal(@"{\{\,\},\{\r\}\n,}", Utilities.Serialization.ListToString(new[] { "{,}", "{\r}\n" }));
            }

            [Property()]
            public void ListToStringMultiline_ListFromStringMultiline_Consistency(string[] List)
            {
                for (var I = 0; I < List.Length; I++)
                {
                    if (List[I] == null)
                    {
                        List[I] = "";
                    }
                }
                var Serialized = Utilities.Serialization.ListToStringMultiline(List);
                var Deserialized = Utilities.Serialization.ListFromStringMultiline(Serialized);
                Assert.Equal(List, Deserialized);
            }
        }
    }
}

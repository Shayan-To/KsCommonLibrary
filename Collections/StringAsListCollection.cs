using System.Collections.Generic;

namespace Ks
{
    namespace Common
    {
        public class StringAsListCollection : BaseReadOnlyList<char>
        {
            public StringAsListCollection(string Str)
            {
                this.Base = Str;
            }

            public override int Count
            {
                get
                {
                    return this.Base.Length;
                }
            }

            public override char this[int Index]
            {
                get
                {
                    return this.Base[Index];
                }
            }

            public override IEnumerator<char> GetEnumerator()
            {
                return this.Base.GetEnumerator();
            }

            private readonly string Base;
        }
    }
}

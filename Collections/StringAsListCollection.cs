using System.Collections.Generic;

namespace Ks.Common
{
    public class StringAsListCollection : BaseReadOnlyList<char>
    {
        public StringAsListCollection(string Str)
        {
            this.Base = Str;
        }

        public override int Count => this.Base.Length;

        public override char this[int Index] => this.Base[Index];

        public override IEnumerator<char> GetEnumerator()
        {
            return this.Base.GetEnumerator();
        }

        private readonly string Base;
    }
}

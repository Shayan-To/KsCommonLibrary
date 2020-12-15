using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class JsonListObject : JsonObject, IReadOnlyList<JsonObject>
    {
        public JsonListObject(IEnumerable<JsonObject> Items)
        {
            this.List = Items.ToArray();
        }

        public int Count => this.List.Length;

        public JsonObject this[int Index] => this.List[Index];

        public IEnumerator<JsonObject> GetEnumerator()
        {
            return ((IReadOnlyList<JsonObject>) this.List).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private readonly JsonObject[] List;
    }
}

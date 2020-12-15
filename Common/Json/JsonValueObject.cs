namespace Ks.Common
{
    public class JsonValueObject : JsonObject
    {
        public JsonValueObject(string Value, bool IsString)
        {
            this.Value = Value;
            this.IsString = IsString;
        }

        public bool IsString { get; }

        public string Value { get; }
    }
}

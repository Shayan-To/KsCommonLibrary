namespace Ks.Common
{
        public class JsonValueObject : JsonObject
        {
            public JsonValueObject(string Value, bool IsString)
            {
                this._Value = Value;
                this._IsString = IsString;
            }

            private readonly bool _IsString;

            public bool IsString
            {
                get
                {
                    return this._IsString;
                }
            }

            private readonly string _Value;

            public string Value
            {
                get
                {
                    return this._Value;
                }
            }
        }
    }

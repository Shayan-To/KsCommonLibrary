namespace Ks.Common
{
        public class JsonDynamicValue : JsonDynamicBase
        {
            public JsonDynamicValue(string Value, bool IsString)
            {
                this._Value = Value;
                this._IsString = IsString;
            }

            public static implicit operator JsonDynamicValue(string Value)
            {
                return new JsonDynamicValue(Value, true);
            }

            public static implicit operator string(JsonDynamicValue Value)
            {
                Verify.True(Value.IsString, "Value is not a string.");
                return Value.Value;
            }

            public static implicit operator JsonDynamicValue(int Value)
            {
                return new JsonDynamicValue(Value.ToStringInv(), false);
            }

            public static implicit operator int(JsonDynamicValue Value)
            {
                Verify.False(Value.IsString, "Value is a string.");
                return Value.GetInteger();
            }

            public static implicit operator JsonDynamicValue(double Value)
            {
                return new JsonDynamicValue(Value.ToStringInv(), false);
            }

            public static implicit operator double(JsonDynamicValue Value)
            {
                Verify.False(Value.IsString, "Value is a string.");
                return Value.GetDouble();
            }

            public static implicit operator JsonDynamicValue(bool Value)
            {
                return new JsonDynamicValue(Value ? "true" : "false", false);
            }

            public static implicit operator bool(JsonDynamicValue Value)
            {
                Verify.False(Value.IsString, "Value is a string.");
                return Value.GetBoolean();
            }

            public static bool operator ==(JsonDynamicValue L, JsonDynamicValue R)
            {
                return (L.Value == R.Value) & (L.IsString == R.IsString);
            }

            public static bool operator !=(JsonDynamicValue L, JsonDynamicValue R)
            {
                return !(L == R);
            }

            public override bool Equals(object obj)
            {
                var T = obj as JsonDynamicValue;
                if (T == null)
                    return false;
                return this == T;
            }

            public override int GetHashCode()
            {
                return Utilities.CombineHashCodes(this.Value.GetHashCode(), this.IsString.GetHashCode());
            }

            public override string ToString()
            {
                return (this.IsString ? "\"" : "") + this.Value + (this.IsString ? "\"" : "");
            }

            private readonly string _Value;

            public string Value
            {
                get
                {
                    return this._Value;
                }
            }

            private readonly bool _IsString;

            public bool IsString
            {
                get
                {
                    return this._IsString;
                }
            }

            internal const string True = "true";
            internal const string False = "false";
        }
    }

using System;

namespace Ks
{
    namespace Common
    {
        public abstract class StringFormatterBase : Formatter
        {
            public StringFormatterBase()
            {
                this.Serializers.Add(Serializer<bool>.Create($"Binary_{nameof(Boolean)}", F => ParseInv.Boolean(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));

                this.Serializers.Add(Serializer<sbyte>.Create($"Binary_{nameof(SByte)}", F => ParseInv.SByte(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));
                this.Serializers.Add(Serializer<Int16>.Create($"Binary_{nameof(Int16)}", F => ParseInv.Short(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));
                this.Serializers.Add(Serializer<Int32>.Create($"Binary_{nameof(Int32)}", F => ParseInv.Integer(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));
                this.Serializers.Add(Serializer<Int64>.Create($"Binary_{nameof(Int64)}", F => ParseInv.Long(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));
                this.Serializers.Add(Serializer<byte>.Create($"Binary_{nameof(Byte)}", F => ParseInv.Byte(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));
                this.Serializers.Add(Serializer<UInt16>.Create($"Binary_{nameof(UInt16)}", F => ParseInv.UShort(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));
                this.Serializers.Add(Serializer<UInt32>.Create($"Binary_{nameof(UInt32)}", F => ParseInv.UInteger(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));
                this.Serializers.Add(Serializer<UInt64>.Create($"Binary_{nameof(UInt64)}", F => ParseInv.ULong(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));

                this.Serializers.Add(Serializer<float>.Create($"Binary_{nameof(Single)}", F => ParseInv.Single(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));
                this.Serializers.Add(Serializer<double>.Create($"Binary_{nameof(Double)}", F => ParseInv.Double(F.Get<string>(null)), null, (F, O) => F.Set(null, O.ToStringInv())));

                this.Serializers.Add(Serializer<char>.Create($"Binary_{nameof(Char)}", F => F.Get<string>(null)[0], null, (F, O) => F.Set<string>(null, O.ToString())));

                this.Initialize();
            }
        }
    }
}

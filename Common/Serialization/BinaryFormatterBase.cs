using System;

namespace Ks.Common
{
    public abstract class BinaryFormatterBase : Formatter
    {
        public BinaryFormatterBase()
        {
            this.Serializers.Add(Serializer<bool>.Create($"Binary_{nameof(Boolean)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 1));
                return BitConverter.ToBoolean(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));

            this.Serializers.Add(Serializer<Int16>.Create($"Binary_{nameof(Int16)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 2));
                return BitConverter.ToInt16(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));
            this.Serializers.Add(Serializer<Int32>.Create($"Binary_{nameof(Int32)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 4));
                return BitConverter.ToInt32(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));
            this.Serializers.Add(Serializer<Int64>.Create($"Binary_{nameof(Int64)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 8));
                return BitConverter.ToInt64(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));
            this.Serializers.Add(Serializer<UInt16>.Create($"Binary_{nameof(UInt16)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 2));
                return BitConverter.ToUInt16(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));
            this.Serializers.Add(Serializer<UInt32>.Create($"Binary_{nameof(UInt32)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 4));
                return BitConverter.ToUInt32(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));
            this.Serializers.Add(Serializer<UInt64>.Create($"Binary_{nameof(UInt64)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 8));
                return BitConverter.ToUInt64(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));

            this.Serializers.Add(Serializer<float>.Create($"Binary_{nameof(Single)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 4));
                return BitConverter.ToSingle(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));
            this.Serializers.Add(Serializer<double>.Create($"Binary_{nameof(Double)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 8));
                return BitConverter.ToDouble(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));

            this.Serializers.Add(Serializer<char>.Create($"Binary_{nameof(Char)}", F =>
            {
                F.Get(null, new SerializationArrayChunk<byte>(this.TempArray, 0, 2));
                return BitConverter.ToChar(this.TempArray, 0);
            }, null, (F, O) =>
            {
                F.Set(null, new SerializationArrayChunk<byte>(BitConverter.GetBytes(O)));
            }));
            this.Serializers.Add(Serializer<string>.Create($"Binary_{nameof(String)}", F => System.Text.Encoding.UTF8.GetString(F.Get<byte[]>(null)), null, (F, O) => F.Set(null, System.Text.Encoding.UTF8.GetBytes(O))));

            this.Initialize();
        }

        private readonly byte[] TempArray = new byte[8];
    }
}

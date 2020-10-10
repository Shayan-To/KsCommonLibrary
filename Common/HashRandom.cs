using System;
using System.Linq;

namespace Ks.Common
{
    public class HashRandom
    {
        private static byte[] GetRandomSeed(System.Security.Cryptography.HashAlgorithm Hasher)
        {
            var Seed = new byte[Hasher.OutputBlockSize];
            DefaultCacher<Random>.Value.NextBytes(Seed);
            return Seed;
        }

        public HashRandom(System.Security.Cryptography.HashAlgorithm Hasher) : this(Hasher, GetRandomSeed(Hasher))
        {
        }

        public HashRandom(System.Security.Cryptography.HashAlgorithm Hasher, long Seed) : this(Hasher, BitConverter.GetBytes(Seed))
        {
        }

        public HashRandom(System.Security.Cryptography.HashAlgorithm Hasher, byte[] Seed)
        {
            this.Buffer = new byte[Hasher.OutputBlockSize];
            Hasher.ComputeHash(Seed, this.Buffer);
            this.Hasher = Hasher;
            this.Index = 0;
        }

        protected HashRandom(System.Security.Cryptography.HashAlgorithm Hasher, byte[] Buffer, int Index)
        {
            this.Hasher = Hasher;
            this.Buffer = Buffer.ToArray();
            this.Index = Index;
        }

        private void RenewBuffer()
        {
            var Tmp = this.Buffer.ToArray();
            this.Hasher.ComputeHash(Tmp, this.Buffer);
            this.Index = 0;
        }

        public int GetRandomInteger()
        {
            this.GetRandomBytes(this.Tmp, 0, 4);
            return BitConverter.ToInt32(this.Tmp, 0);
        }

        public uint GetRandomUInteger()
        {
            this.GetRandomBytes(this.Tmp, 0, 4);
            return BitConverter.ToUInt32(this.Tmp, 0);
        }

        public long GetRandomLong()
        {
            this.GetRandomBytes(this.Tmp, 0, 8);
            return BitConverter.ToInt64(this.Tmp, 0);
        }

        public ulong GetRandomULong()
        {
            this.GetRandomBytes(this.Tmp, 0, 8);
            return BitConverter.ToUInt64(this.Tmp, 0);
        }

        public short GetRandomShort()
        {
            this.GetRandomBytes(this.Tmp, 0, 2);
            return BitConverter.ToInt16(this.Tmp, 0);
        }

        public ushort GetRandomUShort()
        {
            this.GetRandomBytes(this.Tmp, 0, 2);
            return BitConverter.ToUInt16(this.Tmp, 0);
        }

        public byte GetRandomByte()
        {
            this.GetRandomBytes(this.Tmp, 0, 1);
            return this.Tmp[0];
        }

        public sbyte GetRandomSByte()
        {
            this.GetRandomBytes(this.Tmp, 0, 1);
            this.Tmp[1] = 0;
            return (sbyte) BitConverter.ToUInt16(this.Tmp, 0);
        }

        public byte[] GetRandomBytes(int Count)
        {
            var Res = new byte[Count];
            this.GetRandomBytes(Res, 0, Count);
            return Res;
        }

        public virtual void GetRandomBytes(byte[] Array, int Index, int Length)
        {
            Verify.TrueArg(Length >= 0, nameof(Length), "Length cannot be negative.");
            while (Length != 0)
            {
                if (this.Index == this.Buffer.Length)
                {
                    this.RenewBuffer();
                }

                var Len = Math.Min(this.Buffer.Length - this.Index, Length);
                System.Array.Copy(this.Buffer, this.Index, Array, Index, Len);
                Length -= Len;
                this.Index += Len;
            }
        }

        public virtual (byte[] Buffer, int Index) Export()
        {
            return (this.Buffer.ToArray(), this.Index);
        }

        public virtual void Import((byte[] Buffer, int Index) Data)
        {
            Verify.True(Data.Buffer.Length == this.Buffer.Length);
            Array.Copy(Data.Buffer, this.Buffer, this.Buffer.Length);
            this.Index = Data.Index;
        }

        private readonly byte[] Tmp = new byte[8];
        private readonly System.Security.Cryptography.HashAlgorithm Hasher;
        private readonly byte[] Buffer;
        private int Index;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ks.Common
{
    public class BitArray : BaseList<bool>
    {
        public BitArray(int Count)
        {
            this._Count = Count;
            this._Bytes = new byte[(Count >> 3) + (((Count & 7) == 0) ? 0 : 1)];
            this.Byte = new IndexerProperty<BitArray, byte, int>(this, this.Byte.Getter, this.Byte.Setter);
        }

        public override void Insert(int index, bool item)
        {
            throw new NotSupportedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public override void Clear()
        {
            Array.Clear(this._Bytes, 0, this._Bytes.Length);
        }

        private int LastByteBitCount()
        {
            var R = this.Count & 7;
            if (R == 0)
            {
                R = 8;
            }

            return R;
        }

        public void Not()
        {
            for (var I = 0; I < this._Bytes.Length; I++)
            {
                this._Bytes[I] = (byte) ~this._Bytes[I];
            }
        }

        /// <param name="Amount">Shift left if positive, right if negative.</param>
        public void Shift(int Amount)
        {
            for (var I = 0; I < this._Bytes.Length; I++)
            {
                this._Bytes[I] = (byte) ~this._Bytes[I];
            }

            this._Bytes[this._Bytes.Length - 1] = (byte) (this._Bytes[this._Bytes.Length - 1] & ((1 << this.LastByteBitCount()) - 1));
        }

        protected override IEnumerator<bool> _GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<bool> GetEnumerator()
        {
            for (var I = 0; I < this._Bytes.Length - 1; I++)
            {
                var B = this._Bytes[I];
                for (var J = 0; J < 8; J++)
                {
                    yield return (B >> J & 1) == 1;
                }
            }

            if (true)
            {
                var B = this._Bytes[this._Bytes.Length - 1];
                var bits = this.LastByteBitCount();
                for (var J = 0; J < bits; J++)
                {
                    yield return (B >> J & 1) == 1;
                }
            }
        }

        public void SetOne(int Index)
        {
            Verify.True((0 <= Index) & (Index < this.Count), "Index out of range.");

            var B = (int) this._Bytes[Index >> 3];
            var I = Index & 7;
            B = B | (1 << I);
            this._Bytes[Index >> 3] = (byte) B;
        }

        public void SetZero(int Index)
        {
            Verify.True((0 <= Index) & (Index < this.Count), "Index out of range.");

            var B = (int) this._Bytes[Index >> 3];
            var I = Index & 7;
            B = B & ~(1 << I);
            this._Bytes[Index >> 3] = (byte) B;
        }

        public override bool this[int index]
        {
            get
            {
                Verify.True((0 <= index) & (index < this.Count), "Index out of range.");

                var B = this._Bytes[index >> 3];
                var I = index & 7;
                return (B >> I & 1) == 1;
            }
            set
            {
                Verify.True((0 <= index) & (index < this.Count), "Index out of range.");

                var B = (int) this._Bytes[index >> 3];
                var I = index & 7;
                B = (B & ~(1 << I)) | ((value ? 1 : 0) << I);
                this._Bytes[index >> 3] = (byte) B;
            }
        }

        public IndexerProperty<BitArray, byte, int> Byte { get; } = new IndexerProperty<BitArray, byte, int>(
            null,
            (self, Index) =>
            {
                return self._Bytes[Index];
            },
            (self, Index, value) =>
            {
                if (Index == (self._Bytes.Length - 1))
                {
                    value = (byte) (value & ((1 << self.LastByteBitCount()) - 1));
                }

                self._Bytes[Index] = value;
            }
        );

        public int BytesCount => this._Bytes.Count();

        private readonly int _Count;

        public override int Count => this._Count;

        private byte[] _Bytes;
    }
}

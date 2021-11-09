using System.Collections;

namespace DarkSoulsMemoryReader
{
    public class BinaryMemoryValue : MemoryValue
    {
        private int bitStart;
        private int bitCount;

        public BinaryMemoryValue(MemoryAddress address, int bitStart, int bitCount) : base(address) {
            this.bitStart = bitStart;
            this.bitCount = bitCount;
        }

        public override bool TryReadValue(ProcessMemoryReader reader, out dynamic value) {
            int numBytes = ((bitStart + bitCount) / 8) + 1;
            if (reader.TryReadRawBytes(Address, 0, numBytes, out byte[] buffer)) {
                BitArray referenceBitArray = new BitArray(buffer);
                value = new BitArray(bitCount);
                for (int i = 0; i < bitCount; i++) {
                    value.Set(i, referenceBitArray.Get(bitStart + i));
                }
                return true;
            } else {
                value = default(BitArray);
                return false;
            }
        }
    }
}

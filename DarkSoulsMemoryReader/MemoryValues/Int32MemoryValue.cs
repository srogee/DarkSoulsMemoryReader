using System;

namespace DarkSoulsMemoryReader
{
    public class Int32MemoryValue : MemoryValue
    {
        public Int32MemoryValue(MemoryAddress address) : base(address) {

        }

        public override bool TryReadValue(ProcessMemoryReader reader, out dynamic value) {
            if (reader.TryReadRawBytes(Address, 0, 4, out byte[] buffer)) {
                value = BitConverter.ToInt32(buffer);
                return true;
            } else {
                value = default(int);
                return false;
            }
        }
    }
}

using System;

namespace DarkSoulsMemoryReader
{
    public class FloatMemoryValue : MemoryValue
    {
        public FloatMemoryValue(MemoryAddress address) : base(address) {

        }

        public override bool TryReadValue(ProcessMemoryReader reader, out dynamic value) {
            if (reader.TryReadRawBytes(Address, 0, 4, out byte[] buffer)) {
                value = BitConverter.ToSingle(buffer);
                return true;
            } else {
                value = default(float);
                return false;
            }
        }
    }
}

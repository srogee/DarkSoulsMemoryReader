using System.Collections;

namespace DarkSoulsMemoryReader
{
    public class BoolMemoryValue : BinaryMemoryValue
    {
        public BoolMemoryValue(MemoryAddress address, int bitStart) : base(address, bitStart, 1) {

        }

        public override bool TryReadValue(ProcessMemoryReader reader, out dynamic value) {
            if (base.TryReadValue(reader, out dynamic tempValue)) {
                value = (tempValue as BitArray).Get(0);
                return true;
            } else {
                value = default(bool);
                return false;
            }
        }
    }
}

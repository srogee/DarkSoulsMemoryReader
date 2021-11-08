using System.Linq;

namespace DS3MemoryReader
{
    public struct DS3MemoryAddress
    {
        public int BaseAddress;
        public int[] Offsets;

        public DS3MemoryAddress(int baseAddress, params int[] offsets) {
            BaseAddress = baseAddress;
            Offsets = offsets;
        }

        public DS3MemoryAddress AddOffsets(int[] offsets) {
            var newOffsets = Offsets.Concat(offsets).ToArray();
            return new DS3MemoryAddress(BaseAddress, newOffsets);
        }

        public DS3MemoryAddress AddOffset(int offset) {
            return AddOffsets(new int[] { offset });
        }
    }
}

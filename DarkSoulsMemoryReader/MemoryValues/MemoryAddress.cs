namespace DarkSoulsMemoryReader
{
    public struct MemoryAddress
    {
        public int BaseAddress;
        public int[] Offsets;

        public MemoryAddress(int baseAddress, params int[] offsets) {
            BaseAddress = baseAddress;
            Offsets = offsets;
        }
    }
}

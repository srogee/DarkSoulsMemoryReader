namespace DS3MemoryReader
{
    public class DS3MemoryValueBoolFlag : DS3MemoryValueBinary
    {
        public DS3MemoryValueBoolFlag(DS3ProcessInfo processInfo, DS3MemoryAddress memoryAddress, int bitStart, DS3AddressUpdateType updateType = DS3AddressUpdateType.Manual) : base(processInfo, memoryAddress, updateType, bitStart, 1) { }

        public new bool Value
        {
            get
            {
                return GetBitArray().Get(0);
            }
        }
    }
}

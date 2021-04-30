using System;

namespace DS3MemoryReader
{
    class DS3MemoryValueInt32 : DS3MemoryValue
    {
        public DS3MemoryValueInt32(DS3ProcessInfo processInfo, DS3MemoryAddress memoryAddress, DS3AddressUpdateType updateType) : base(processInfo, memoryAddress, updateType) { }

        public int Value
        {
            get
            {
                if (VerifyRealAddressIsValid()) {
                    return BitConverter.ToInt32(GetRawBytes(4));
                } else {
                    return default(int);
                }
            }
        }
    }
}

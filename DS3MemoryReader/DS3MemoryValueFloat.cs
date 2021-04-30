using System;

namespace DS3MemoryReader
{
    class DS3MemoryValueFloat : DS3MemoryValue
    {
        public DS3MemoryValueFloat(DS3ProcessInfo processInfo, DS3MemoryAddress memoryAddress, DS3AddressUpdateType updateType) : base(processInfo, memoryAddress, updateType) { }

        public float Value
        {
            get
            {
                if (VerifyRealAddressIsValid()) {
                    return BitConverter.ToSingle(GetRawBytes(4));
                } else {
                    return default(float);
                }
            }
        }
    }
}

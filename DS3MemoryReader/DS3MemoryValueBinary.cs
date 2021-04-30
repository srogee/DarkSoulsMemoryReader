using System;
using System.Collections;

namespace DS3MemoryReader
{
    class DS3MemoryValueBoolFlag : DS3MemoryValueBinary
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

    class DS3MemoryValueBinary : DS3MemoryValue
    {
        private int bitStart;
        private int bitLength;

        public DS3MemoryValueBinary(DS3ProcessInfo processInfo, DS3MemoryAddress memoryAddress, DS3AddressUpdateType updateType, int bitStart, int bitLength) : base(processInfo, memoryAddress, updateType) {
            this.bitStart = bitStart;
            this.bitLength = bitLength;
        }

        protected BitArray GetBitArray() {
            if (VerifyRealAddressIsValid()) {
                // Get bit values for all bytes necessary
                int numBytes = ((bitStart + bitLength) / 8) + 1;
                BitArray referenceBitArray = new BitArray(GetRawBytes(numBytes));

                // Copy values into smaller return value array
                BitArray returnValue = new BitArray(bitLength);
                for (int i = 0; i < bitLength; i++) {
                    returnValue.Set(i, referenceBitArray.Get(bitStart + i));
                }

                return returnValue;
            } else {
                return default(BitArray);
            }
        }

        public BitArray Value
        {
            get
            {
                return GetBitArray();
            }
        }
    }
}

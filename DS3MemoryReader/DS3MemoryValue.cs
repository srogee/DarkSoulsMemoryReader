using System;

namespace DS3MemoryReader
{
    public enum DS3AddressUpdateType
    {
        Automatic, // Addresses are recalculated every time the value is read, to ensure they are always accurate
        Manual
    }

    class DS3MemoryValue {
        private DS3ProcessInfo processInfo;
        private DS3MemoryAddress memoryAddress;
        private IntPtr realAddress;
        private DS3AddressUpdateType updateType;
        private bool hasGeneratedRealAddress;

        public DS3MemoryValue(DS3ProcessInfo processInfo, DS3MemoryAddress memoryAddress, DS3AddressUpdateType updateType) {
            this.processInfo = processInfo;
            this.memoryAddress = memoryAddress;
            this.updateType = updateType;
        }

        // Regenerate the real address of one or more values
        public static void RegenerateAddresses(params DS3MemoryValue[] values) {
            foreach (DS3MemoryValue value in values) {
                value.RegenerateAddress();
            }
        }

        // Regenerate the real address of the value, since the pointers could have changed
        public void RegenerateAddress() {
            hasGeneratedRealAddress = false;

            if (processInfo.IsValid) {
                try {
                    realAddress = ReadIntPtrAtLocation(processInfo.Handle, processInfo.BaseAddress + memoryAddress.BaseAddress);

                    if (memoryAddress.Offsets.Length > 0) {
                        for (int i = 0; i < memoryAddress.Offsets.Length - 1; i++) {
                            realAddress = ReadIntPtrAtLocation(processInfo.Handle, realAddress + memoryAddress.Offsets[i]);
                        }
                        realAddress += memoryAddress.Offsets[memoryAddress.Offsets.Length - 1];
                    }

                    hasGeneratedRealAddress = true;
                } catch (Exception) {
                    // Assume the process has just exited
                    processInfo.Detach();
                }
            }
        }

        // Helper function for reading the memory at the specified location and converting it to an IntPtr
        private static IntPtr ReadIntPtrAtLocation(IntPtr processHandle, IntPtr location) {
            int bytesRead = 0;
            byte[] buffer = new byte[8];
            ProcessInterop.ReadProcessMemory(processHandle, location, buffer, buffer.Length, ref bytesRead);
            return (IntPtr)BitConverter.ToInt64(buffer);
        }

        // Read the specified number of bytes at the specified offset from this value's real address
        public byte[] GetRawBytes(int length, int offset = 0) {
            if (processInfo.IsValid) {
                try {
                    int bytesRead = 0;
                    byte[] buffer = new byte[length];
                    ProcessInterop.ReadProcessMemory(processInfo.Handle, realAddress + offset, buffer, buffer.Length, ref bytesRead);
                    return buffer;
                } catch (Exception) {
                    // Assume the process has just exited
                    processInfo.Detach();
                }
            }

            return new byte[0];
        }

        // Common function for value getters to use to set up the address and verify it's okay to read from
        protected bool VerifyRealAddressIsValid() {
            if (updateType == DS3AddressUpdateType.Automatic || !hasGeneratedRealAddress) {
                RegenerateAddress();
            }

            return processInfo.IsValid && hasGeneratedRealAddress;
        }
    }

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

        public override string ToString() {
            return Value.ToString();
        }
    }

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

        public override string ToString() {
            return Value.ToString();
        }
    }

    class DS3MemoryValueString : DS3MemoryValue
    {
        private int maxLength;
        public DS3MemoryValueString(DS3ProcessInfo processInfo, DS3MemoryAddress memoryAddress, DS3AddressUpdateType updateType, int maxLength) : base(processInfo, memoryAddress, updateType) {
            this.maxLength = maxLength;
        }

        public string Value
        {
            get
            {
                if (VerifyRealAddressIsValid()) {
                    byte[] bytes = GetRawBytes(maxLength * 2);
                    string str = System.Text.Encoding.Unicode.GetString(bytes);
                    int nullTerminatorIndex = str.IndexOf("\0");
                    if (nullTerminatorIndex >= 0) {
                        return str.Substring(0, nullTerminatorIndex);
                    }
                    return str;
                } else {
                    return default(string);
                }
            }
        }

        public override string ToString() {
            return Value.ToString();
        }
    }
}

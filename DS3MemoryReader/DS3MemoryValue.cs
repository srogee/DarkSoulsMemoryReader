using System;

namespace DS3MemoryReader
{
    public enum DS3AddressUpdateType
    {
        Automatic, // Addresses are recalculated every time the value is read, to ensure they are always accurate
        Manual
    }

    public abstract class DS3MemoryValue {
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

        public dynamic GetValueGeneric() {
            return (this as dynamic).Value;
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

        public override string ToString() {
            return GetValueGeneric().ToString();
        }
    }
}

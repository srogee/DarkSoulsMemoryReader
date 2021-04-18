using System;

namespace DS3MemoryReader
{
    public enum DS3AddressUpdateType
    {
        Automatic
    }

    class DS3MemoryValue<T> {
        private DS3ProcessInfo processInfo;
        private DS3MemoryAddress memoryAddress;
        private IntPtr realAddress;
        private DS3AddressUpdateType updateType;

        public DS3MemoryValue(DS3ProcessInfo processInfo, DS3MemoryAddress memoryAddress) {
            this.processInfo = processInfo;
            this.memoryAddress = memoryAddress;
            this.updateType = DS3AddressUpdateType.Automatic;
        }

        public T Value
        {
            // Get value at memory referenced by this class
            get
            {
                if (processInfo.IsValid) {
                    if (updateType == DS3AddressUpdateType.Automatic) {
                        RegenerateAddress();
                    }

                    object value;
                    Type type = typeof(T);

                    if (type == typeof(float)) {
                        value = BitConverter.ToSingle(GetRawBytes(4));
                    } else if (type == typeof(int)) {
                        value = BitConverter.ToInt32(GetRawBytes(4));
                    } else if (type == typeof(string)) {
                        value = ReadString();
                    } else {
                        throw new Exception($"DS3MemoryValue does not support generic type {type}");
                    }

                    return (T)value;
                } else {
                    return default(T);
                }
            }
        }

        private string ReadString() {
            byte[] bytes = GetRawBytes(16 * 2);
            return System.Text.Encoding.Unicode.GetString(bytes);
        }

        public void RegenerateAddress() {
            // Regenerate the real address of the value, since the pointers could have changed
            if (processInfo.IsValid) {
                try {
                    realAddress = ReadIntPtrAtLocation(processInfo.Handle, processInfo.BaseAddress + memoryAddress.BaseAddress);

                    if (memoryAddress.Offsets.Length > 0) {
                        for (int i = 0; i < memoryAddress.Offsets.Length - 1; i++) {
                            realAddress = ReadIntPtrAtLocation(processInfo.Handle, realAddress + memoryAddress.Offsets[i]);
                        }
                        realAddress += memoryAddress.Offsets[memoryAddress.Offsets.Length - 1];
                    }
                } catch (Exception) {
                    // Assume the process has just exited
                    processInfo.Detach();
                }
            }
        }

        public byte[] GetRawBytes(int length) {
            if (processInfo.IsValid) {
                try {
                    int bytesRead = 0;
                    byte[] buffer = new byte[length];
                    ProcessInterop.ReadProcessMemory(processInfo.Handle, realAddress, buffer, buffer.Length, ref bytesRead);
                    return buffer;
                } catch (Exception) {
                    // Assume the process has just exited
                    processInfo.Detach();
                }
            }

            return new byte[0];
        }

        public override string ToString() {
            return Value.ToString();
        }

        private static IntPtr ReadIntPtrAtLocation(IntPtr processHandle, IntPtr location) {
            int bytesRead = 0;
            byte[] buffer = new byte[8];
            ProcessInterop.ReadProcessMemory(processHandle, location, buffer, buffer.Length, ref bytesRead);
            return (IntPtr)BitConverter.ToInt64(buffer);
        }
    }
}

namespace DS3MemoryReader
{
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
    }
}

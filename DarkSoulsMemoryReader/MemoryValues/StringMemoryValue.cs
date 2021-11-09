using System.Text;

namespace DarkSoulsMemoryReader
{
    public class StringMemoryValue : MemoryValue
    {
        private int maxLength;

        public StringMemoryValue(MemoryAddress address, int maxLength) : base(address) {
            this.maxLength = maxLength;
        }

        public override bool TryReadValue(ProcessMemoryReader reader, out dynamic value) {
            if (reader.TryReadRawBytes(Address, 0, maxLength * 2, out byte[] buffer)) {
                value = Encoding.Unicode.GetString(buffer);
                int nullTerminatorIndex = value.IndexOf("\0");
                if (nullTerminatorIndex >= 0) {
                    value = value.Substring(0, nullTerminatorIndex);
                }
                return true;
            } else {
                value = default(string);
                return false;
            }
        }
    }
}

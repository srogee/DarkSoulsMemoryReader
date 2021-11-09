using System;

namespace MemoryReader
{
    public class MemoryValue
    {
        public MemoryAddress Address;

        public MemoryValue(MemoryAddress address) {
            Address = address;
        }

        public dynamic ReadValue(ProcessMemoryReader reader) {
            TryReadValue(reader, out dynamic value);
            return value;
        }

        public virtual bool TryReadValue(ProcessMemoryReader reader, out dynamic value) {
            throw new NotImplementedException();
        }
    }
}

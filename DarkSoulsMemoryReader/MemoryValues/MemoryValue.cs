using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkSoulsMemoryReader
{
    public class MemoryValue
    {
        public MemoryAddress Address;

        public MemoryValue() { }

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

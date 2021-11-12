using System;
using System.Linq;

namespace DarkSoulsMemoryReader
{
    class VirtualMemoryValue : MemoryValue
    {
        protected MemoryValue[] Components;

        public VirtualMemoryValue(params MemoryValue[] components) {
            Components = components;
        }

        public override bool TryReadValue(ProcessMemoryReader reader, out dynamic value) {
            if (reader.IsAttached) {
                value = CalculateValue(Components.Select(component => component.ReadValue(reader)).ToArray());
                return true;
            } else {
                value = CalculateValue(new dynamic[0]);
                return false;
            }
        }

        protected virtual dynamic CalculateValue(dynamic[] values) {
            throw new NotImplementedException();
        }
    }
}

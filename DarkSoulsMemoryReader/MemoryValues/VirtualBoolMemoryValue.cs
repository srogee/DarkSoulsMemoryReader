using System.Linq;

namespace DarkSoulsMemoryReader
{
    class VirtualBoolMemoryValue : VirtualMemoryValue
    {
        public VirtualBoolMemoryValue(params MemoryValue[] components) : base(components) {}

        protected override dynamic CalculateValue(dynamic[] values) {
            return values.Length > 0 ? values.All(value => value) : false;
        }
    }
}

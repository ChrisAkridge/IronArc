using IronArc.HardwareDefinitionGenerator.Models;

namespace IronArc.HardwareDefinitionGenerator
{
    internal static class DefaultDataTypes
    {
        public static HardwareMethodDataType Pointer => new HardwareMethodDataType("void", 1);
        public static HardwareMethodDataType UInt8 => new HardwareMethodDataType("byte", 0);
        public static HardwareMethodDataType UInt16 => new HardwareMethodDataType("ushort", 0);
        public static HardwareMethodDataType UInt32 => new HardwareMethodDataType("uint", 0);
        public static HardwareMethodDataType LpString => new HardwareMethodDataType("byte", 0);
        public static HardwareMethodDataType LpStringPointer => new HardwareMethodDataType("byte", 1);
    }
}

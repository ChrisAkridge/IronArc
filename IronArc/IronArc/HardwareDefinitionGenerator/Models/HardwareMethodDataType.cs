namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareMethodDataType
    {
        public string TypeName { get; }
        public int PointerLevel { get; }

        public HardwareMethodDataType(string typeName, int pointerLevel)
        {
            TypeName = typeName;
            PointerLevel = pointerLevel;
        }
    }
}

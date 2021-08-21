namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareCallParameter
    {
        public string ParameterName { get; }
        public HardwareCallDataType Type { get; }

        public HardwareCallParameter(string parameterName, HardwareCallDataType type)
        {
            ParameterName = parameterName;
            Type = type;
        }
    }
}

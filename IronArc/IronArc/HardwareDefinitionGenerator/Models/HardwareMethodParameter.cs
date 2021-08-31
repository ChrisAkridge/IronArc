namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareMethodParameter
    {
        public string ParameterName { get; }
        public HardwareMethodDataType Type { get; }

        public HardwareMethodParameter(string parameterName, HardwareMethodDataType type)
        {
            ParameterName = parameterName;
            Type = type;
        }
    }
}

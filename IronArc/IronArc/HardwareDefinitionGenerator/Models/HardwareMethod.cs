using System.Collections.Generic;

namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareMethod
    {
        private readonly List<HardwareMethodParameter> parameters;

        public HardwareMethodType MethodType { get; }
        public HardwareMethodDataType ReturnType { get; }
        public string CallName { get; }
        public IReadOnlyList<HardwareMethodParameter> Parameters => parameters.AsReadOnly();

        public HardwareMethod(HardwareMethodType methodType, HardwareMethodDataType returnType, string callName,
            IList<HardwareMethodParameter> parameters)
        {
            MethodType = methodType;
            ReturnType = returnType;
            CallName = callName;
            this.parameters = (List<HardwareMethodParameter>)parameters;
        }
    }
}

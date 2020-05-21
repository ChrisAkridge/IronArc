using System;
using System.Collections.Generic;
using System.Text;

namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareCall
    {
        private readonly List<HardwareCallParameter> parameters;

        public HardwareCallDataType ReturnType { get; }
        public string CallName { get; }
        public IReadOnlyList<HardwareCallParameter> Parameters => parameters.AsReadOnly();

        public HardwareCall(HardwareCallDataType returnType, string callName, IList<HardwareCallParameter> parameters)
        {
            ReturnType = returnType;
            CallName = callName;
            this.parameters = (List<HardwareCallParameter>)parameters;
        }
    }
}

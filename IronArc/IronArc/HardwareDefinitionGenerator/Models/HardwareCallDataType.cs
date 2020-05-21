using System;
using System.Collections.Generic;
using System.Text;

namespace IronArc.HardwareDefinitionGenerator.Models
{
    public sealed class HardwareCallDataType
    {
        public string TypeName { get; }
        public int PointerLevel { get; }

        public HardwareCallDataType(string typeName, int pointerLevel)
        {
            TypeName = typeName;
            PointerLevel = pointerLevel;
        }
    }
}

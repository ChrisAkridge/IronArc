using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class HardwareConfigurationTypeAttribute : Attribute
    {
        public Type ConfigurationType { get; set; }

        public HardwareConfigurationTypeAttribute(Type configurationType) => ConfigurationType = configurationType;
    }
}

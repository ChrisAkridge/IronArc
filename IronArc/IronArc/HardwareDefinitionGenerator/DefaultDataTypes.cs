﻿using IronArc.HardwareDefinitionGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.HardwareDefinitionGenerator
{
    internal static class DefaultDataTypes
    {
        public static HardwareCallDataType UInt16 => new HardwareCallDataType("ushort", 0);
        public static HardwareCallDataType LpString => new HardwareCallDataType("lpstring", 0);
        public static HardwareCallDataType LpStringPointer => new HardwareCallDataType("lpstring", 1);
    }
}

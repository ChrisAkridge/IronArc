using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using IronArc.HardwareDefinitionGenerator.Models;
using Newtonsoft.Json;

namespace IronArc.HardwareDefinitionGenerator
{
    public static class Generator
    {
        public static string GenerateHardwareDefinition(string version)
        {
            var hardwareTypes = GetHardwareTypes();
            IEnumerable<HardwareDevice> devices =
                hardwareTypes.Select(t => (HardwareDevice)Activator.CreateInstance(t, new Guid(), 0u)).ToList();
            var deviceDefinitions =
                devices.Select(d => d.Definition);
            var definition = new HardwareDefinition(version, deviceDefinitions.ToList());

            foreach (HardwareDevice device in devices)
            {
                device.Dispose();
            }

            return JsonConvert.SerializeObject(definition);
        }

        internal static HardwareCall ParseHardwareCall(string callDescription)
        {
            int argumentsStart = callDescription.IndexOf('(');

            var parameterListText = callDescription
                .Substring(argumentsStart);

            var parameters = (parameterListText != "()")
                ? parameterListText
                    .TrimStart('(')
                    .TrimEnd(')')
                    .Split(',')
                    .Select(param => param.Trim().Split(' '))
                    .Select(param => new HardwareCallParameter(
                        param[1],
                        MapHardwareCallTypeToCixType(param[0].TrimEnd('*'), param[0].Count(c => c == '*'))
                    ))
                    .ToList()
                : new List<HardwareCallParameter>();
            
            var typeAndName = callDescription
                .Substring(0, argumentsStart)
                .Split(' ');

            var returnType = typeAndName[0];
            var deviceAndCallName = typeAndName[2]
                .Split(new[]
                {
                    "::"
                }, StringSplitOptions.None);
            var callName = deviceAndCallName[1];
            
            return new HardwareCall(MapHardwareCallTypeToCixType(returnType, 0),
                callName,
                parameters);
        }

        private static HardwareCallDataType MapHardwareCallTypeToCixType(string typeName, int pointerLevel) =>
            typeName == "ptr"
                ? DefaultDataTypes.Pointer
                : new HardwareCallDataType(MapHardwareCallTypeNameToCixTypeName(typeName), pointerLevel);

        private static string MapHardwareCallTypeNameToCixTypeName(string typeName)
        {
            switch (typeName)
            {
                case "uint8": return "byte";
                case "uint16": return "ushort";
                case "uint32": return "uint";
                case "uint64": return "ulong";
                case "int8": return "sbyte";
                case "int16": return "short";
                case "int32": return "int";
                case "int64": return "long";
                case "ptr": return "void";
                default: return typeName;
            }
        }

        private static IEnumerable<Type> GetHardwareTypes()
        {
            // https://stackoverflow.com/a/949285/2709212
            var ironArc = Assembly.GetExecutingAssembly();
            var ironArcTypes = ironArc.GetTypes();
            const string namespaceName = "IronArc.Hardware";

            // https://stackoverflow.com/a/16349607/2709212
            return ironArcTypes.Where(t => t.Namespace == namespaceName)
                .Where(t => 
                    !t.IsInterface
                    && t.GetCustomAttribute(typeof(CompilerGeneratedAttribute), true) == null);
        }
    }
}

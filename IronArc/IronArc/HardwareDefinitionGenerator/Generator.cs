using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
                hardwareTypes.Select(t => (HardwareDevice)Activator.CreateInstance(t, new Guid())).ToList();
            IEnumerable<Models.HardwareDevice> deviceDefinitions =
                devices.Select(d => d.Definition);
            var definition = new Models.HardwareDefinition(version, deviceDefinitions.ToList());

            foreach (HardwareDevice device in devices)
            {
                device.Dispose();
            }

            return JsonConvert.SerializeObject(definition);
        }

        internal static HardwareCall ParseHardwareCall(string callDescription)
        {
            int argumentsStart = callDescription.IndexOf('(');
            var parameters = callDescription
                .Substring(argumentsStart)
                .TrimStart('(')
                .TrimEnd(')')
                .Split(',')
                .Select(param => param.Trim().Split(' '))
                .Select(param => new HardwareCallParameter(
                    param[1],
                    MapHardwareCallTypeToCixType(param[0].TrimEnd('*'), param[0].Count(c => c == '*'))
                ))
                .ToList();

            var typeAndName = callDescription
                .Substring(0, argumentsStart)
                .Split(' ');

            var returnType = typeAndName[0];
            var deviceAndCallName = typeAndName[2].Split(new[] {"::"}, StringSplitOptions.None);
            var deviceName = deviceAndCallName[0];
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

            return ironArcTypes.Where(t => t.Namespace == namespaceName)
                .Where(t => !t.IsInterface);
        }
    }
}

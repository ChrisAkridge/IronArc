﻿using System;
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

            foreach (var device in devices)
            {
                device.Dispose();
            }

            return JsonConvert.SerializeObject(definition);
        }

        internal static HardwareMethod ParseHardwareMethod(string methodDescription)
        {
            int argumentsStart = methodDescription.IndexOf('(');

            var parameterListText = methodDescription
                .Substring(argumentsStart);

            var parameters = (parameterListText != "()")
                ? parameterListText
                    .TrimStart('(')
                    .TrimEnd(')')
                    .Split(',')
                    .Select(param => param.Trim().Split(' '))
                    .Select(param => new HardwareMethodParameter(
                        param[1],
                        MapHardwareCallTypeToCixType(param[0].TrimEnd('*'), param[0].Count(c => c == '*'))
                    ))
                    .ToList()
                : new List<HardwareMethodParameter>();
            
            var typeAndName = methodDescription
                .Substring(0, argumentsStart)
                .Split(' ');

            var returnType = typeAndName[0];
            var methodType = typeAndName[1] == "hwcall"
                ? HardwareMethodType.HardwareCall
                : HardwareMethodType.Interrupt;
            var deviceAndCallName = typeAndName[2]
                .Split(new[]
                {
                    "::"
                }, StringSplitOptions.None);
            var callName = deviceAndCallName[1];

            if (methodType == HardwareMethodType.Interrupt && returnType != "void")
            {
                throw new ArgumentException($"Interrupts cannot return values. Description: \"{methodDescription}\"");
            }
            
            return new HardwareMethod(methodType, MapHardwareCallTypeToCixType(returnType, 0),
                callName,
                parameters);
        }

        private static HardwareMethodDataType MapHardwareCallTypeToCixType(string typeName, int pointerLevel) =>
            typeName == "ptr"
                ? DefaultDataTypes.Pointer
                : new HardwareMethodDataType(MapHardwareCallTypeNameToCixTypeName(typeName), pointerLevel);

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

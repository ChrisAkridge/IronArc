using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IronArc;

namespace IronArcHost
{
    internal static class HardwareSearcher
    {
        private static readonly List<string> hardwareDeviceNames = new List<string>();

        public static List<Type> HardwareDeviceTypes = new List<Type>();
        public static IReadOnlyList<string> HardwareDeviceNames => hardwareDeviceNames.AsReadOnly();

        public static void FindHardwareInAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(HardwareDevice))))
            {
                HardwareDeviceTypes.Add(type);
                hardwareDeviceNames.Add(type.FullName);
            }
        }

        public static void FindHardwareInIronArc()
        {
            var ironArc = typeof(Processor).Assembly;
            FindHardwareInAssembly(ironArc);
        }

        public static Type LookupDeviceByName(string deviceFullTypeName)
        {
            var deviceType = HardwareDeviceTypes.FirstOrDefault(h => h.FullName == deviceFullTypeName);
            if (deviceType == null)
            {
                throw new ArgumentException($"No hardware device named {deviceFullTypeName} exists.");
            }

            return deviceType;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IronArc.HardwareDefinitionGenerator
{
    public static class Generator
    {
        public static string GenerateHardwareDefinition(string version)
        {
            var hardwareTypes = GetHardwareTypes();
            IEnumerable<HardwareDevice> devices =
                hardwareTypes.Select(t => (HardwareDevice)Activator.CreateInstance(t, new Guid()));
            IEnumerable<Models.HardwareDevice> deviceDefinitions =
                devices.Select(d => d.Definition);
            var definition = new Models.HardwareDefinition(version, deviceDefinitions.ToList());
            
            foreach (HardwareDevice device in devices)
            {
                device.Dispose();
            }

            return JsonConvert.SerializeObject(definition);
        }

        private static IEnumerable<Type> GetHardwareTypes()
        {
            // https://stackoverflow.com/a/949285/2709212
            var ironArc = Assembly.GetExecutingAssembly();
            var ironArcTypes = ironArc.GetTypes();
            string namespaceName = "IronArc.Hardware";

            return ironArcTypes.Where(t => t.Namespace == namespaceName)
                .Where(t => !t.IsInterface);
        }
    }
}

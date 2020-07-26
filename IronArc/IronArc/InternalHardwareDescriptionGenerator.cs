using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc.Memory;

namespace IronArc
{
    internal static class InternalHardwareDescriptionGenerator
    {
        public static byte[] GetDescriptions(IEnumerable<HardwareDevice> devices, ulong destination)
        {
            var descriptions = new List<byte[]>();

            foreach (var description in devices.OrderBy(d => d.DeviceId).Select(device => GetDescription(device, destination)))
            {
                descriptions.Add(description);
                destination += (ulong)description.Length;
            }

            return descriptions.SelectMany(desc => desc).ToArray();
        }
        
        public static byte[] GetDescription(HardwareDevice device, ulong destination)
        {
            byte[] nameUtf8Bytes = Encoding.UTF8.GetBytes(device.DeviceName);
            byte[] deviceIndexBytes = BitConverter.GetBytes(device.DeviceId);
            byte[] memoryStartBytes = BitConverter.GetBytes(device.MemoryMapping.StartAddress);
            byte[] memoryEndBytes = BitConverter.GetBytes(device.MemoryMapping.EndAddress);
            byte[] nameLengthBytes = BitConverter.GetBytes(nameUtf8Bytes.Length);

            byte[] namePointerBytes =
                BitConverter.GetBytes(destination
                    + 8UL
                    + (ulong)deviceIndexBytes.Length
                    + (ulong)memoryStartBytes.Length
                    + (ulong)memoryEndBytes.Length);

            return nameUtf8Bytes.Concat(deviceIndexBytes)
                .Concat(memoryStartBytes)
                .Concat(memoryEndBytes)
                .Concat(nameLengthBytes)
                .ToArray();
        }
    }
}

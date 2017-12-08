using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IronArc;

namespace IronArcHost
{
	internal static class HardwareSearcher
	{
		public static List<Type> HardwareDeviceTypes = new List<Type>();

		public static void FindHardwareInAssembly(Assembly assembly)
		{
			foreach (var type in assembly.GetTypes())
			{
				// Only types that derive from IronArc.HardwareDevice are hardware devices
				// https://stackoverflow.com/a/8699063/2709212
				if (type.IsSubclassOf(typeof(HardwareDevice)))
				{
					HardwareDeviceTypes.Add(type);
				}
			}
		}

		public static void FindHardwareInIronArc()
		{
			var ironArc = typeof(Processor).Assembly;
			FindHardwareInAssembly(ironArc);
		}
	}
}

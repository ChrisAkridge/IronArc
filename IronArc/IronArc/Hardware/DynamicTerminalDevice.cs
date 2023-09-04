using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks
using IronArc.HardwareDefinitionGenerator;
using IronArc.HardwareDefinitionGenerator.Models;
using DefinitionDevice = IronArc.HardwareDefinitionGenerator.Models.HardwareDevice;

namespace IronArc.Hardware
{
	public sealed class DynamicTerminalDevice : HardwareDevice
	{
		private readonly IDynamicTerminal terminal;

		public override string DeviceName => "DynamicTerminalDevice";

		public override HardwareDeviceStatus Status
		{
			get => HardwareDeviceStatus.Active;
			protected set => throw new InvalidOperationException();
		}

		internal override DefinitionDevice Definition =>
			new DefinitionDevice(nameof(DynamicTerminalDevice),
				new List<HardwareMethod>
				{
					new HardwareMethod(HardwareMethodType.HardwareCall, null, "SetCursorPosition", new List<HardwareMethodParameter>
					{
						new HardwareMethodParameter("x", DefaultDataTypes.UInt16),
						new HardwareMethodParameter("y", DefaultDataTypes.UInt16)
					}),
					new HardwareMethod(HardwareMethodType.HardwareCall, null, "SetWindowSize", new List<HardwareMethodParameter>
					{
						new HardwareMethodParameter("width", DefaultDataTypes.UInt16),
						new HardwareMethodParameter("height", DefaultDataTypes.UInt16)
					}),
					new HardwareMethod(HardwareMethodType.HardwareCall, null, "Write",
					new List<HardwareMethodParameter>
					{
						new HardwareMethodParameter("text", DefaultDataTypes.LpStringPointer)
					}),
					new HardwareMethod(HardwareMethodType.HardwareCall, null, "WriteLine",
					new List<HardwareMethodParameter>
					{
						new HardwareMethodParameter("text", DefaultDataTypes.LpStringPointer)
					}),
					new HardwareMethod(HardwareMethodType.HardwareCall, DefaultDataTypes.UInt16, "ReadKey", new List<HardwareMethodParameter> { }),
					new HardwareMethod(HardwareMethodType.HardwareCall, DefaultDataTypes.UInt16, "ReadChar", new List<HardwareMethodParameter> { }),
					new HardwareMethod(HardwareMethodType.HardwareCall, null, "ReadLine", new List<HardwareMethodParameter>
					{
						new HardwareMethodParameter("destination", DefaultDataTypes.Pointer)
					})
				});

		public DynamicTerminalDevice(Guid machineId, uint deviceId)
		{
			terminal = HardwareProvider.Provider.CreateDynamicTerminal();
			terminal.MachineId = machineId;

			MachineId = machineId;
			DeviceId = deviceId;
		}

		public override void HardwareCall(string functionName, VirtualMachine vm)
		{
			if (functionName.Equals("SetCursorPosition", StringComparison.Ordinal))
			{
				ushort y = (ushort)vm.Processor.PopExternal(OperandSize.Word);
				ushort x = (ushort)vm.Processor.PopExternal(OperandSize.Word);

				SetCursorPosition(x, y);
			}
			else if (functionName.Equals("SetWindowSize", StringComparison.Ordinal))
			{
				ushort height = (ushort)vm.Processor.PopExternal(OperandSize.Word);
				ushort width = (ushort)vm.Processor.PopExternal(OperandSize.Word);

				SetWindowSize(width, height);
			}
			else if (functionName.Equals("Write", StringComparison.Ordinal))
			{
				ulong textAddress = vm.Processor.PopExternal(OperandSize.QWord);
				string text = vm.Processor.ReadStringFromMemory(textAddress);

				Write(text);
			}
			else if (functionName.Equals("WriteLine", StringComparison.Ordinal))
			{
				ulong textAddress = vm.Processor.PopExternal(OperandSize.QWord);
				string text = vm.Processor.ReadStringFromMemory(textAddress);

				WriteLine(text);
			}
			else if (functionName.Equals("ReadKey", StringComparison.Ordinal))
			{
				var key = ReadKey();
				vm.Processor.PushExternal((ulong)key, OperandSize.DWord);
			}
			else if (functionName.Equals("ReadChar", StringComparison.Ordinal))
			{
				ushort key = ReadChar();
				vm.Processor.PushExternal(key, OperandSize.Word);
			}
			else if (functionName.Equals("ReadLine", StringComparison.Ordinal))
			{
				string line = ReadLine();
				ulong address = vm.Processor.PopExternal(OperandSize.QWord);
				vm.Processor.WriteStringToMemory(address, line);
				vm.Processor.PushExternal(address, OperandSize.QWord);
			}
			else
			{
				throw new InvalidOperationException($"The hardware call '{functionName}' is not supported by the DynamicTerminalDevice.");
			}
		}

		private void SetCursorPosition(ushort x, ushort y)
		{
			terminal.CursorX = x;
			terminal.CursorY = y;
		}

		private void SetWindowSize(ushort width, ushort height)
		{
			terminal.Width = width;
			terminal.Height = height;
		}

		private void Write(string text) => terminal.Write(text);
		private void WriteLine(string text) => terminal.WriteLine(text);

		private int ReadKey() =>
			terminal.CanPerformWaitingRead ? terminal.ReadKey() : terminal.NonWaitingReadKey();

		private char ReadChar() =>
			terminal.CanPerformWaitingRead ? terminal.ReadChar() : terminal.NonWaitingReadChar();

		private string ReadLine() =>
			terminal.CanPerformWaitingRead ? terminal.ReadLine() : terminal.NonWaitingReadLine();
		public override void Dispose() => HardwareProvider.Provider.DestroyDynamicTerminal(terminal);
	}
}

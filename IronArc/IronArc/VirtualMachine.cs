using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IronArc.Hardware;

namespace IronArc
{
    public sealed class VirtualMachine
    {
        public const ushort SpecificationMajorVersion = 0x0001;
        public const ushort SpecificationMinorVersion = 0x0001;
        public const uint MagicNumber = 0x45584549; // "IEXE"
        private const ulong HeaderSize = 28UL;

        private ulong firstInstructionAddress;
        private ulong stringsTableAddress;

        public Guid MachineId { get; }
        public Processor Processor { get; }
        public ByteBlock Memory { get; }
        public ConcurrentQueue<Message> MessageQueue { get; }
        public List<HardwareDevice> Hardware { get; }
        public VMState State { get; private set; }
        public ulong InstructionExecutedCount { get; private set; }

        public VirtualMachine(ulong memorySize, string programPath, ulong loadAddress,
            IEnumerable<string> hardwareDeviceNames)
        {
            byte[] program = File.ReadAllBytes(programPath);
            ParseHeader(program);

            MachineId = Guid.NewGuid();
            MessageQueue = new ConcurrentQueue<Message>();

            Memory = ByteBlock.FromLength(memorySize);
            Memory.WriteAt(program, loadAddress);

            Processor = new Processor(Memory, loadAddress, (ulong)program.Length + HeaderSize,
                stringsTableAddress, this);
            Processor.EIP += firstInstructionAddress;
            Hardware = new List<HardwareDevice>();

            foreach (var type in hardwareDeviceNames.Select(name => Type.GetType(name)))
            {
                Hardware.Add((HardwareDevice)Activator.CreateInstance(type ?? throw new InvalidOperationException(), MachineId));
            }

            State = VMState.Paused;
        }

        private void ParseHeader(byte[] program)
        {
            uint magicNumber = BitConverter.ToUInt32(program, 0);
            if (magicNumber != MagicNumber)
            {
                Error((uint)IronArc.Error.HeaderInvalid);
            }

            uint specificationVersion = BitConverter.ToUInt32(program, 4);
            ushort majorVersion = (ushort)(specificationVersion >> 16);
            ushort minorVersion = (ushort)(specificationVersion & 0xFFFF);

            if (majorVersion > SpecificationMajorVersion
                || (majorVersion == SpecificationMajorVersion && minorVersion > SpecificationMinorVersion))
            {
                Error((uint)IronArc.Error.HeaderInvalid);
            }

            firstInstructionAddress = BitConverter.ToUInt64(program, 12);
            stringsTableAddress = BitConverter.ToUInt64(program, 20);
        }

        public void AddHardwareDevice(Type hardwareDeviceType)
        {
            if (Hardware.Select(h => h.GetType()).Contains(hardwareDeviceType))
            {
                throw new ArgumentException($"A hardware device named {hardwareDeviceType.FullName} is already running on this VM.");
            }

            Hardware.Add((HardwareDevice)Activator.CreateInstance(hardwareDeviceType, MachineId));
        }

        public void AddHardwareDevice(HardwareDevice device)
        {
            if (Hardware.Any(h => h.GetType() == device.GetType()))
            {
                throw new ArgumentException($"A hardware device named {device.GetType().FullName} is already running on this VM.");
            }

            Hardware.Add(device);
        }

        public bool RemoveHardwareDevice(Type hardwareDeviceType)
        {
            var device = Hardware.FirstOrDefault(h => h.GetType() == hardwareDeviceType);
            if (device == null) { return false; }

            device.Dispose();
            Hardware.Remove(device);
            return true;
        }

        public void Resume()
        {
            State = VMState.Running;

            var message = new Message(VMMessage.None, UIMessage.VMStateChanged, MachineId,
                (int)State, 0L, null);
            HardwareProvider.Provider.UIMessageQueue.Enqueue(message);
        }

        public void Pause()
        {
            State = VMState.Paused;

            var message = new Message(VMMessage.None, UIMessage.VMStateChanged, MachineId,
                (int)State, 0L, null);
            HardwareProvider.Provider.UIMessageQueue.Enqueue(message);
        }

        public void Halt()
        {
            State = VMState.Halted;

            var message = new Message(VMMessage.None, UIMessage.VMStateChanged, MachineId,
                (int)State, 0L, null);
            HardwareProvider.Provider.UIMessageQueue.Enqueue(message);
        }

        public void Error(uint errorCode)
        {
            State = VMState.Error;

            var message = new Message(VMMessage.None, UIMessage.VMStateChanged, MachineId, (int)State,
                errorCode, null);
            HardwareProvider.Provider.UIMessageQueue.Enqueue(message);
        }

        public void MainLoop()
        {
            while (State == VMState.Running)
            {
                Processor.ExecuteNextInstruction();
                InstructionExecutedCount++;

                // Here we check if the message queue has any messages in it. Ordinarily, calling
                // IsEmpty isn't very good since another thread can add a message after the call to
                // IsEmpty but before the dequeue happens. This would typically be a problem, but
                // depending on how often we check for messages, it may not be that bad - we may not
                // respond to messages on this loop, but we will soon.
                //
                // TryDequeue is usually used as the solution to this two-stage check, but it isn't
                // as performant - it calls IsEmpty internally, which makes it less performant than
                // just using IsEmpty. So we'll use IsEmpty until we don't have do.
                if (!MessageQueue.IsEmpty)
                {
                    // For now, we'll just respond to SetVMState.
                    if (!MessageQueue.TryDequeue(out Message message))
                    {
                        throw new InvalidOperationException("Tried to dequeue a message from an empty queue.");
                        // yay race conditions
                        // although the only things doing dequeues SHOULD be VMs, hopefully
                    }

                    if (message.VMMessage == VMMessage.SetVMState)
                    {
                        // WParam stores the new state to switch to
                        if ((VMState)message.WParam != VMState.Paused) { continue; }

                        Pause();
                        break;
                    }
                    else
                    {
                        switch (message.VMMessage)
                        {
                            case VMMessage.AddHardwareDevice:
                                AddHardwareDevice((HardwareDevice)message.Data);
                                break;
                            case VMMessage.RemoveHardwareDevice:
                                RemoveHardwareDevice((Type)message.Data);
                                break;
                        }
                    }
                }
            }
        }

        public void ExecuteOneInstruction()
        {
            Processor.ExecuteNextInstruction();
            InstructionExecutedCount++;
        }

        internal void HardwareCall(string hwcall)
        {
            string[] hwcallParts = hwcall.Split(new[] { "::" }, StringSplitOptions.None);

            foreach (var hwDevice in Hardware.Where(d => d.DeviceName == hwcallParts[0]))
            {
                hwDevice.HardwareCall(hwcallParts[1], this);
                return;
            }

            // If we reach here, this hardware device doesn't exist, or the device name is wrong.
            Processor.RaiseError(IronArc.Error.HardwareDeviceNotPresent);
        }
    }
}

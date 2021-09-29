using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronArc.Hardware.Configuration;
using IronArc.Hardware.Storage;
using IronArc.HardwareDefinitionGenerator;
using IronArc.HardwareDefinitionGenerator.Models;
using IronArc.Memory;

namespace IronArc.Hardware
{
    [HardwareConfigurationType(typeof(StorageDeviceConfiguration))]
    public sealed class StorageDevice : HardwareDevice
    {
        private const int LargeTransferBufferSize = 1048576;

        private FileStream fileStream;
        private readonly Dictionary<uint, StorageStream> streams = new Dictionary<uint, StorageStream>();
        
        public override string DeviceName => "Storage";

        public override HardwareDeviceStatus Status
        {
            get => HardwareDeviceStatus.Active;
            protected set => throw new InvalidOperationException();
        }
        
        internal override HardwareDefinitionGenerator.Models.HardwareDevice Definition =>
            new HardwareDefinitionGenerator.Models.HardwareDevice("Storage",
                new List<HardwareMethod>
                {
                    Generator.ParseHardwareMethod("uint32 hwcall Storage::OpenStream(int64 startPosition, int64 length)"),
                    Generator.ParseHardwareMethod("void hwcall Storage::CloseStream(uint32 streamID)"),
                    Generator.ParseHardwareMethod("int64 hwcall Storage::GetStreamLength(uint32 streamID)"),
                    Generator.ParseHardwareMethod("void hwcall Storage::Seek(uint32 streamID, int64 newPosition)"),
                    Generator.ParseHardwareMethod("void hwcall Storage::Read(uint32 streamID, ptr destination, int64 length)"),
                    Generator.ParseHardwareMethod("void hwcall Storage::Write(uint32 streamID, ptr source, int64 length)")
                });

        public StorageDevice(Guid machineId, uint deviceId)
        {
            MachineId = machineId;
            DeviceId = deviceId;
        }

        public override void Configure(HardwareConfiguration configuration)
        {
            var storageConfiguration = (StorageDeviceConfiguration)configuration;

            fileStream = File.Open(storageConfiguration.BaseFileAbsolutePath, FileMode.Open);
        }

        public override void HardwareCall(string functionName, VirtualMachine vm)
        {
            string lowerCased = functionName.ToLowerInvariant();

            switch (lowerCased)
            {
                case "openstream":
                    OpenStream(vm);
                    break;
                case "closestream":
                    CloseStream(vm);
                    break;
                case "getstreamlength":
                    GetStreamLength(vm);
                    break;
                case "seek":
                    Seek(vm);
                    break;
                case "read":
                    Read(vm);
                    break;
                case "write":
                    Write(vm);
                    break;
            }
        }

        private void OpenStream(VirtualMachine vm)
        {
            var length = (long)vm.Processor.PopExternal(OperandSize.QWord);
            var startPosition = (long)vm.Processor.PopExternal(OperandSize.QWord);

            if (startPosition < 0)
            {
                vm.Processor.RaiseError(Error.HardwareError, $"Cannot open stream at a negative position (-0x{(-startPosition):X16})");
                return;
            }
            
            if (length < 0)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot open stream with a negative length (-0x{(-length):X16})");
                return;
            }

            if (startPosition + length >= fileStream.Length)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot open stream from 0x{startPosition:X16} to 0x{(startPosition + length):X16} (device is 0x{fileStream.Length:X16} bytes)");
                return;
            }

            var newStream = new StorageStream(fileStream, startPosition, length);
            streams.Add(newStream.StreamID, newStream);
            vm.Processor.PushExternal(newStream.StreamID, OperandSize.DWord);
        }

        private void CloseStream(VirtualMachine vm)
        {
            var streamID = (uint)vm.Processor.PopExternal(OperandSize.DWord);

            if (!streams.ContainsKey(streamID))
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot close stream with ID 0x{streamID:X8}; no stream has that ID");
                return;
            }

            streams.Remove(streamID);
        }

        private void GetStreamLength(VirtualMachine vm)
        {
            var streamID = (uint)vm.Processor.PopExternal(OperandSize.DWord);

            if (!streams.ContainsKey(streamID))
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot read length of stream with ID 0x{streamID:X8}; no stream has that ID");
                return;
            }
            
            vm.Processor.PushExternal((ulong)streams[streamID].Length, OperandSize.QWord);
        }

        private void Seek(VirtualMachine vm)
        {
            var newPosition = (long)vm.Processor.PopExternal(OperandSize.QWord);
            var streamID = (uint)vm.Processor.PopExternal(OperandSize.DWord);

            if (newPosition < 0)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    "Position to seek to was negative");
                return;
            }
            
            if (!streams.ContainsKey(streamID))
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot read length of stream with ID 0x{streamID:X8}; no stream has that ID");
                return;
            }

            var stream = streams[streamID];
            if (newPosition >= stream.Length)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot seek to 0x{newPosition:X16} - stream is only 0x{stream.Length:X16} bytes long");
                return;
            }
            
            stream.Seek(newPosition);
        }

        private void Read(VirtualMachine vm)
        {
            var length = (long)vm.Processor.PopExternal(OperandSize.QWord);
            var destination = vm.Processor.PopExternal(OperandSize.QWord);
            var streamID = (uint)vm.Processor.PopExternal(OperandSize.DWord);

            if (length <= 0)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    "Stream read was zero-length or negative length");
            }

            if (!streams.ContainsKey(streamID))
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot read length 0x{length:X16} of stream with ID 0x{streamID:X8}; no stream has that ID");

                return;
            }

            var stream = streams[streamID];
            var memoryManager = vm.MemoryManager;

            if (stream.Position + length > stream.Length)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot read from 0x{stream.Position:X16} to 0x{(stream.Position + length):X16} - stream is only 0x{stream.Length:X16} bytes long");

                return;
            }

            // ughhhhhhh why doesn't .NET like using unsigned types for lengths
            if ((long)(destination + (ulong)length) >= memoryManager.CurrentContextLength)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot read into 0x{destination:X16} to 0x{(destination + (ulong)length):X16} - current context is only 0x{memoryManager.CurrentContextLength:X16} bytes long");
                return;
            }

            fileStream.Seek(stream.StartAddress + stream.Position, SeekOrigin.Begin);

            if (length < int.MaxValue)
            {
                ReadUnderTwoGigabytes(memoryManager, destination, (int)length);
            }
            else
            {
                ReadOverTwoGigabytes(memoryManager, destination, length);
            }

            stream.Advance(length);
        }

        private void Write(VirtualMachine vm)
        {
            var length = (long)vm.Processor.PopExternal(OperandSize.QWord);
            var source = vm.Processor.PopExternal(OperandSize.QWord);
            var streamID = (uint)vm.Processor.PopExternal(OperandSize.DWord);

            if (length <= 0)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    "Stream read was zero-length or negative length");
            }

            if (!streams.ContainsKey(streamID))
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot write length 0x{length:X16} of stream with ID 0x{streamID:X8}; no stream has that ID");

                return;
            }

            var stream = streams[streamID];
            var memoryManager = vm.MemoryManager;

            if (stream.Position + length >= stream.Length)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot write to 0x{stream.Position:X16} to 0x{(stream.Position + length):X16} - stream is only 0x{stream.Length:X16} bytes long");

                return;
            }

            // ughhhhhhh why doesn't .NET like using unsigned types for lengths
            if ((long)(source + (ulong)length) >= memoryManager.CurrentContextLength)
            {
                vm.Processor.RaiseError(Error.HardwareError,
                    $"Cannot write from 0x{source:X16} to 0x{(source + (ulong)length):X16} - current context is only 0x{memoryManager.CurrentContextLength:X16} bytes long");
                return;
            }

            fileStream.Seek(stream.StartAddress + stream.Position, SeekOrigin.Begin);

            if (length < int.MaxValue)
            {
                WriteUnderTwoGigabytes(memoryManager, source, (int)length);
            }
            else
            {
                WriteOverTwoGigabytes(memoryManager, source, length);
            }
            
            stream.Advance(length);
        }

        private void ReadUnderTwoGigabytes(MemoryManager memoryManager, ulong destination, int length)
        {
            var bytes = new byte[length];
            fileStream.Read(bytes, 0, length);
            memoryManager.Write(bytes, destination);
        }

        private void ReadOverTwoGigabytes(MemoryManager memoryManager, ulong destination, long length)
        {
            var buffer = new byte[LargeTransferBufferSize];

            while (length > 0)
            {
                var bytesRead = fileStream.Read(buffer, 0, LargeTransferBufferSize);
                
                memoryManager.Write(buffer, destination);
                
                length -= bytesRead;
                destination += (ulong)bytesRead;

                if (bytesRead < LargeTransferBufferSize)
                {
                    return;
                }
            }
        }

        private void WriteUnderTwoGigabytes(MemoryManager memoryManager, ulong source, int length)
        {
            var bytes = new byte[length];
            memoryManager.Write(bytes, source);
            fileStream.Write(bytes, 0, length);
        }

        private void WriteOverTwoGigabytes(MemoryManager memoryManager, ulong source, long length)
        {
            var buffer = new byte[LargeTransferBufferSize];

            while (length > 0)
            {
                // what, did you break a bone or something?
                var bytesToWrite = (ulong)Math.Min(length, LargeTransferBufferSize);
                memoryManager.Read(source, bytesToWrite);
                fileStream.Write(buffer, 0, (int)bytesToWrite);

                length -= (long)bytesToWrite;
                source += bytesToWrite;
                // you know, because of all those casts?
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public override void Dispose()
        {
            fileStream?.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using IronArc.Memory;

// ReSharper disable InconsistentNaming

namespace IronArc
{
    public sealed class Processor
    {
        private readonly VirtualMachine vm;
        private readonly MemoryManager memory;

        public ulong EAX;
        public ulong EBX;
        public ulong ECX;
        public ulong EDX;
        public ulong EEX;
        public ulong EFX;
        public ulong EGX;
        public ulong EHX;

        public ulong EBP;
        public ulong ESP;

        public ulong EIP;
        public ulong EFLAGS;
        public ulong ERP;

        public ulong stackArgsMarker;
        public ulong stringsTableAddress;

        public Dictionary<string, List<InterruptHandler>> interruptTable;
        public Dictionary<uint, ulong> errorTable;
        public Stack<CallStackFrame> callStack;

        public Processor(MemoryManager memory, ulong firstInstructionAddress, ulong programSize,
            ulong stringsTableAddress, VirtualMachine vm)
        {
            this.memory = memory;
            this.vm = vm;
            this.stringsTableAddress = stringsTableAddress;

            interruptTable = new Dictionary<string, List<InterruptHandler>>();
            errorTable = new Dictionary<uint, ulong>();
            callStack = new Stack<CallStackFrame>();

            EIP = ERP = firstInstructionAddress;
            EAX = programSize;
        }

        #region Memory Read/Write
        private byte ReadProgramByte() => memory.ReadByte(EIP++);
        private sbyte ReadProgramSByte() => (sbyte)ReadProgramByte();

        private ushort ReadProgramWord()
        {
            var result = memory.ReadUShort(EIP);
            EIP += 2;
            return result;
        }
        private short ReadProgramSWord() => (short)ReadProgramWord();

        private uint ReadProgramDWord()
        {
            var result = memory.ReadUInt(EIP);
            EIP += 4;
            return result;
        }
        private int ReadProgramSDWord() => (int)ReadProgramDWord();

        private ulong ReadProgramQWord()
        {
            var result = memory.ReadULong(EIP);
            EIP += 8;
            return result;
        }
        private long ReadProgramSQWord() => (long)ReadProgramQWord();

        private string ReadProgramLPString()
        {
            var result = memory.ReadString(EIP, out uint stringLength);
            EIP += stringLength;
            return result;
        }
        #endregion

        [SuppressMessage("ReSharper", "MultipleStatementsOnOneLine")]
        public void ExecuteNextInstruction()
        {
            ushort opcode = ReadProgramWord();

            try
            {
                switch ((opcode >> 8))
                {
                    case 0x00: /* Control Flow Instructions */
                        switch (opcode & 0xFF)
                        {
                            case 0x00:
                                NoOperation();
                                break;
                            case 0x01:
                                End();
                                break;
                            case 0x02:
                                Jump();
                                break;
                            case 0x03:
                                Call();
                                break;
                            case 0x04:
                                Return();
                                break;
                            case 0x05:
                                JumpIfEqual();
                                break;
                            case 0x06:
                                JumpIfNotEqual();
                                break;
                            case 0x07:
                                JumpIfLessThan();
                                break;
                            case 0x08:
                                JumpIfGreaterThan();
                                break;
                            case 0x09:
                                JumpIfLessThanOrEqualTo();
                                break;
                            case 0x0A:
                                JumpIfGreaterThanOrEqualTo();
                                break;
                            case 0x0B:
                                break;
                            case 0x0C:
                                HardwareCall();
                                break;
                            case 0x0D:
                                StackArgumentPrologue();
                                break;
                            default: break;
                        }
                        break;
                    case 0x01:
                        switch (opcode & 0xFF)
                        {
                            case 0x00:
                                MoveData();
                                break;
                            case 0x01:
                                MoveDataWithLength();
                                break;
                            case 0x02:
                                PushToStack();
                                break;
                            case 0x03:
                                PopFromStack();
                                break;
                            case 0x04:
                                ArrayRead();
                                break;
                            case 0x05:
                                ArrayWrite();
                                break;
                            default: break;
                        }

                        break;
                    case 0x02:
                        int operation = opcode & 0xFF;

                        if (operation <= 0x11 /* 0x0211 is Stack Comparison */) { PerformStackOperation(opcode); }
                        else if (operation <= 0x23 /* 0x0223 is Long Comparison */) { PerformLongOperation(opcode); }
                        else if (operation >= 0x80 && operation <= 0x86) { PerformFloatingStackOperation(opcode); }

                        break;
                    default:
                        break;
                }
            }
            catch (VMErrorException errorEx)
            {
                RaiseError(errorEx.Error, errorEx.Message ?? errorEx.DefaultMessage);
            }
        }

        #region Internal Helpers
        private ulong ReadRegisterByIndex(ulong registerNumber)
        {
            switch (registerNumber)
            {
                case 0UL: return EAX;
                case 1UL: return EBX;
                case 2UL: return ECX;
                case 3UL: return EDX;
                case 4UL: return EEX;
                case 5UL: return EFX;
                case 6UL: return EGX;
                case 7UL: return EHX;
                case 8UL: return EBP;
                case 9UL: return ESP;
                case 10UL: return EIP;
                case 11UL: return EFLAGS;
                case 12UL: return ERP;
                default:
                    throw new ArgumentException($"There is no register numbered {registerNumber}. Please ensure you've masked out the high two bits.");
            }
        }

        [SuppressMessage("ReSharper", "MultipleStatementsOnOneLine")]
        private void WriteRegisterByIndex(ulong registerNumber, ulong value)
        {
            switch (registerNumber)
            {
                case 0UL: EAX = value; break;
                case 1UL: EBX = value; break;
                case 2UL: ECX = value; break;
                case 3UL: EDX = value; break;
                case 4UL: EEX = value; break;
                case 5UL: EFX = value; break;
                case 6UL: EGX = value; break;
                case 7UL: EHX = value; break;
                case 8UL: EBP = value; break;
                case 9UL: ESP = value; break;
                case 10UL: EIP = value; break;
                case 11UL:
                    EFLAGS = value;
                    memory.PerformAddressTranslation = (EFLAGS & EFlags.PerformAddressTranslation) != 0;
                    break;
                case 12UL: ERP = value; break;
                default:
                    throw new ArgumentException($"There is no register numbered {registerNumber}. Please ensure you've masked out the high two bits.");
            }
        }

        private static OperandSize ReadOperandSize(byte operandSizeBits)
        {
            switch (operandSizeBits)
            {
                case 0: return OperandSize.Byte;
                case 1: return OperandSize.Word;
                case 2: return OperandSize.DWord;
                case 3: return OperandSize.QWord;
                default:
                    throw new ArgumentException($"Implementation error: Received a value of {operandSizeBits} when a value from 0 to 3 is needed.");
            }
        }

        private static AddressType ReadAddressType(byte addressTypeBits)
        {
            switch (addressTypeBits)
            {
                case 0: return AddressType.MemoryAddress;
                case 1: return AddressType.Register;
                case 2: return AddressType.NumericLiteral;
                case 3: return AddressType.StringEntry;
                default:
                    throw new ArgumentException("Implementation error: You didn't give me a value between 0 and 3 for an address type in a flags byte.");
            }
        }

        private static ulong MaskDataBySize(ulong data, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte: return data & 0xFF;
                case OperandSize.Word: return data & 0xFFFF;
                case OperandSize.DWord: return data & 0xFFFF_FFFF;
                case OperandSize.QWord: return data;
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }

        [SuppressMessage("ReSharper", "MultipleStatementsOnOneLine")]
        private void WriteDataToMemoryBySize(ulong data, ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte: memory.WriteByte((byte)data, address); break;
                case OperandSize.Word: memory.WriteUShort((ushort)data, address); break;
                case OperandSize.DWord: memory.WriteUInt((uint)data, address); break;
                case OperandSize.QWord: memory.WriteULong(data, address); break;
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }

        private ulong GetMemoryAddressFromAddressBlock(byte addressTypeBits)
        {
            var addressBlock = new AddressBlock(OperandSize.QWord, ReadAddressType(addressTypeBits),
                memory, EIP);
            EIP += addressBlock.operandLength;

            return GetMemoryAddressFromAddressBlock(addressBlock);
        }

        private ulong GetMemoryAddressFromAddressBlock(AddressBlock block)
        {
            switch (block.type)
            {
                case AddressType.MemoryAddress:
                    return block.isPointer ? memory.ReadULong(block.value) : block.value;
                case AddressType.Register:
                    ulong valueWithOffset = ReadRegisterByIndex(block.value) + (ulong)block.offset;
                    return block.isPointer ? memory.ReadULong(valueWithOffset) : valueWithOffset;
                case AddressType.NumericLiteral:
                    if (block.size != OperandSize.QWord)
                    {
                        RaiseError(Error.InvalidAddressType);
                    }
                    else
                    {
                        return block.value;
                    }
                    break;
                case AddressType.StringEntry:
                    return LookupStringAddress((uint)block.value);
                default:
                    throw new ArgumentException("Implementation error: An address block has a wrong type.");
            }

            return 0UL;
        }

        private ulong LookupStringAddress(uint stringIndex)
        {
            uint numberOfStrings = memory.ReadUInt(stringsTableAddress);
            if (numberOfStrings <= stringIndex)
            {
                RaiseError(Error.StringIndexOutOfRange);
                return 0UL;
            }

            ulong stringAddressAddress = stringsTableAddress + 4 + (8 * stringIndex);
            return memory.ReadULong(stringAddressAddress);
        }

        private ulong ReadDataFromAddressBlock(AddressBlock block, OperandSize size)
        {
            switch (block.type)
            {
                case AddressType.MemoryAddress:
                    return memory.ReadData(block.isPointer ? memory.ReadULong(block.value) : block.value, size);
                case AddressType.Register:
                    if (block.offset != 0 && !block.isPointer)
                    {
                        RaiseError(Error.InvalidAddressType, $"Read from {ErrorMessages.GetRegisterName(block.value)} that has offset but is not a pointer");
                        return 0UL;
                    }
                    else if (block.isPointer)
                    {
                        ulong address = ReadRegisterByIndex(block.value) + (ulong)block.offset;
                        return memory.ReadData(address, size);
                    }
                    else
                    {
                        return MaskDataBySize(ReadRegisterByIndex(block.value), size);
                    }
                case AddressType.NumericLiteral:
                    return block.value;
                case AddressType.StringEntry:
                    return LookupStringAddress((uint)block.value);
                default:
                    throw new ArgumentException($"Invalid address block type {block.type}");
            }
        }

        private void WriteDataToAddressBlock(ulong data, AddressBlock block, OperandSize size)
        {
            switch (block.type)
            {
                case AddressType.MemoryAddress:
                    memory.WriteData(data, block.isPointer ? memory.ReadULong(block.value) : block.value, size);
                    break;
                case AddressType.Register:
                    if (block.offset != 0 && !block.isPointer)
                    {
                        RaiseError(Error.InvalidAddressType, $"Write to {ErrorMessages.GetRegisterName(block.value)} that has offset but is not a pointer");
                    }
                    else if (block.isPointer)
                    {
                        ulong address = ReadRegisterByIndex(block.value) + (ulong)block.offset;
                        memory.WriteData(data, address, size);
                    }
                    else
                    {
                        data = MaskDataBySize(data, size);
                        WriteRegisterByIndex(block.value, data);
                    }
                    break;
                case AddressType.NumericLiteral:
                case AddressType.StringEntry:
                    RaiseError(Error.InvalidAddressType);
                    break;
                default:
                    throw new ArgumentException($"Implementation error: Invalid address type {block.type}");
            }
        }

        internal void RaiseError(uint errorCode, string message)
        {
            vm.LastError = new ErrorDescription((Error)errorCode,
                message ?? ErrorMessages.GetDefaultMessage((Error)errorCode));
            
            // Look up an error handler for the code and call it if there is one.
            if (!errorTable.ContainsKey(errorCode))
            {
                vm.Error(errorCode);
                return;
            }

            var handlerAddress = errorTable[errorCode] + ERP;
            CallImpl(handlerAddress);
        }

        internal void RaiseError(Error error) => RaiseError((uint)error, null);

        internal void RaiseError(Error error, string message) => RaiseError((uint)error, message);

        private void CallImpl(ulong callAddress)
        {
            var oldFlags = EFLAGS;

            // Clear the stackargs flag.
            EFLAGS &= ~EFlags.StackArgsSet;

            // Create and push the call stack frame.
            var frame = new CallStackFrame(callAddress, EIP, EAX, EBX, ECX, EDX, EEX,
                EFX, EGX, EHX, EFLAGS, EBP);
            callStack.Push(frame);

            // Clear most registers.
            EAX = EBX = ECX = EDX = EEX = EFX = EGX = EHX = EFLAGS = 0UL;

            // Check stackargs and set EBP accordingly
            if ((oldFlags & EFlags.StackArgsSet) != 0)
            {
                EBP = stackArgsMarker;
                stackArgsMarker = 0;
            }
            else { EBP = ESP; }

            EIP = callAddress;
        }

        private void ReturnImpl()
        {
            if (!callStack.Any())
            {
                RaiseError(Error.CallStackUnderflow);
                return;
            }

            // Pop the top frame off the call stack.
            var frame = callStack.Pop();

            // Reset most registers.
            EAX = frame.EAX;
            EBX = frame.EBX;
            ECX = frame.ECX;
            EDX = frame.EDX;
            EEX = frame.EEX;
            EFX = frame.EFX;
            EGX = frame.EGX;
            EHX = frame.EHX;
            EFLAGS = frame.EFLAGS;
            EBP = frame.EBP;

            // We don't need to touch ESP - since return values are pushed on the stack, they can
            // become part of the previous stack frame.

            EIP = frame.returnAddress;
        }
        #endregion

        #region External Helpers
        public void PushExternal(byte[] bytes)
        {
            memory.Write(bytes, ESP);
            ESP += (ulong)bytes.Length;
        }

        public void PushExternal(ulong data, OperandSize size)
        {
            ulong sizeInBytes = size.GetSizeInBytes();
            memory.WriteData(data, ESP, size);
            ESP += sizeInBytes;
        }

        public ulong PopExternal(OperandSize size)
        {
            ulong sizeInBytes = size.GetSizeInBytes();
            ulong dataStartAddress = ESP - sizeInBytes;
            ESP -= sizeInBytes;

            if (ESP >= EBP) { return memory.ReadData(dataStartAddress, size); }

            RaiseError(Error.StackUnderflow);
            return 0;
        }

        public string ReadStringFromMemory(ulong stringAddress) => memory.ReadString(stringAddress, out _);

        public void WriteStringToMemory(ulong stringAddress, string text) =>
            memory.WriteString(text, stringAddress);

        internal void RegisterInterruptHandler(uint deviceId, string interruptName, ulong callAddress)
        {
            if (!interruptTable.ContainsKey(interruptName))
            {
                interruptTable.Add(interruptName, new List<InterruptHandler>());
            }
            
            // TODO: verify that the interrupt name actually exists
            // also take a device ID in a handler, too, and verify that
            // exists in the SystemDevice class (or maybe in VirtualMachine)
            int nextHandlerIndex = Enumerable.Range(0, 256)
                .First(i => interruptTable[interruptName].All(interrupt => interrupt.Index != i));

            if (nextHandlerIndex >= 256)
            {
                RaiseError(Error.HardwareError, $"All handlers for {interruptName} are registered");
                return;
            }
            
            interruptTable[interruptName].Add(new InterruptHandler
            {
                DeviceId = deviceId,
                CallAddress = callAddress,
                Index = (byte)nextHandlerIndex
            });
        }

        internal void UnregisterInterruptHandler(uint deviceId, string interruptName, byte index)
        {
            if (!interruptTable.ContainsKey(interruptName))
            {
                RaiseError(Error.HardwareError, $"Cannot remove handlers for {interruptName} as none are registered");
                return;
            }

            if (interruptTable[interruptName].All(interrupt => interrupt.Index != index))
            {
                RaiseError(Error.HardwareError, $"Cannot remove handler #{index} for {interruptName} because there isn't a handler with that index");
                return;
            }

            interruptTable[interruptName].RemoveAll(i => i.Index == index);
        }

        internal void RegisterErrorHandler(uint errorCode, ulong handlerAddress) => errorTable[errorCode] = handlerAddress;

        internal void UnregisterErrorHandler(uint errorCode) => errorTable.Remove(errorCode);
        #endregion

        #region Control Flow Instructions (0x00)
        private static void NoOperation()
        {
            // Here, at 2:43pm EST on Thursday, December 14, 2017, the first IronArc instruction
            // was executed.
        }

        private void End()
        {
            vm.Halt();
        }

        private void Jump()
        {
            // jmp <address>
            //	<address>: An addressing block containing the address + ERP to jump to.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
            EIP = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;
        }

        private void Call()
        {
            // call <address>
            //	<address>: An addressing block containing the address + ERP to call.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
            ulong callAddress = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;

            CallImpl(callAddress);
        }

        private void Return()
        {
            // ret
            // Errors:
            //	CallStackUnderflow: Raised if the call stack is empty.

            ReturnImpl();
        }

        private void JumpIfEqual()
        {
            // je <address>
            //	<address>: An addressing block containing the address + ERP to jump to.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);

            if ((EFLAGS & EFlags.EqualFlag) != 0)
            {
                EIP = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;
            }
        }

        private void JumpIfNotEqual()
        {
            // jne <address>
            //	<address>: An addressing block containing the address + ERP to jump to.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);

            if ((EFLAGS & EFlags.EqualFlag) == 0)
            {
                EIP = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;
            }
        }

        private void JumpIfLessThan()
        {
            // jlt <address>
            //	<address>: An addressing block containing the address + ERP to jump to.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);

            if ((EFLAGS & EFlags.LessThanFlag) != 0)
            {
                EIP = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;
            }
        }

        private void JumpIfGreaterThan()
        {
            // jgt <address>
            //	<address>: An addressing block containing the address + ERP to jump to.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);

            if ((EFLAGS & EFlags.GreaterThanFlag) != 0)
            {
                EIP = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;
            }
        }

        private void JumpIfLessThanOrEqualTo()
        {
            // jlte <address>
            //	<address>: An addressing block containing the address + ERP to jump to.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);

            if (((EFLAGS & EFlags.LessThanFlag) != 0) || ((EFLAGS & EFlags.EqualFlag) != 0))
            {
                EIP = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;
            }
        }

        private void JumpIfGreaterThanOrEqualTo()
        {
            // jgte <address>
            //	<address>: An addressing block containing the address + ERP to jump to.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);

            if (((EFLAGS & EFlags.GreaterThanFlag) != 0) || ((EFLAGS & EFlags.EqualFlag) != 0))
            {
                EIP = GetMemoryAddressFromAddressBlock(flagsByte) + ERP;
            }
        }

        private void AbsoluteJump()
        {
            // jmpa <address>
            //	<address>: An addressing block containing the address to jump to.
            // Errors:
            //	InvalidAddressType: Raised if the addressing block does not name a memory address.
            //	AddressOutOfRange: Raised if the memory address is out of the range of the memory space.
            // Flags byte: 00XX0000 where the X names the address block type.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
            EIP = GetMemoryAddressFromAddressBlock(flagsByte);
        }

        private void HardwareCall()
        {
            // hwcall <addressToString>
            //	<addressToString>: An address to a length-prefixed UTF-8 sequence of bytes representing the string containing the hardware call to perform.
            // Errors:
            //	InvalidAddressType: Raised if the address block is a numeric literal.
            //	AddressOutOfRange: Raised if the address is beyond the range of memory.
            // Flags byte: 00XX0000 where XX is the type of the address block.

            byte flagsByte = (byte)((ReadProgramByte() & 0x30) >> 4);
            var block = new AddressBlock(OperandSize.QWord, ReadAddressType(flagsByte),
                memory, EIP);
            EIP += block.operandLength;
            ulong stringAddress = ReadDataFromAddressBlock(block, OperandSize.QWord);

            string hwcall = memory.ReadString(stringAddress, out _);
            vm.HardwareCall(hwcall);
        }

        private void StackArgumentPrologue()
        {
            // stackargs
            // Errors: None.
            // Flags Byte: None.

            // Set the stackargs flag on EFLAGS.
            EFLAGS |= EFlags.StackArgsSet;

            // Set the stackargs marker to the current value of ESP.
            stackArgsMarker = ESP;
        }
        #endregion

        #region Data Operation Instruction (0x01)
        private void MoveData()
        {
            // mov <SIZE> <source> <dest>
            //	<SIZE>: The size of the operand to move.
            //	<source>: A memory address, register, or numeric literal.
            //	<destination>: A memory address or register.
            // Errors:
            //	InvalidAddressType: Raised if source is a string table entry or destination is a numeric literal or string entry table.
            //	AddressOutOfRange: Raised if any address is out of range.
            // Flags Byte: ZZSSDD00, where Z is the size, S is the source type, D is the destination type.

            byte flagsByte = ReadProgramByte();
            OperandSize size = ReadOperandSize((byte)(flagsByte >> 6));
            AddressType sourceType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));
            AddressType destinationType = ReadAddressType((byte)((flagsByte & 0x0C) >> 2));

            AddressBlock sourceBlock = new AddressBlock(size, sourceType, memory, EIP);
            EIP += sourceBlock.operandLength;
            AddressBlock destinationBlock = new AddressBlock(size, destinationType, memory, EIP);
            EIP += destinationBlock.operandLength;

            ulong sourceData = ReadDataFromAddressBlock(sourceBlock, size);
            WriteDataToAddressBlock(sourceData, destinationBlock, size);
        }

        private void MoveDataWithLength()
        {
            // movln <sourceAddr> <destAddr> <length>
            //	sourceAddr: An addressing block (memory address, pointer-to-memory, or
            //		pointer-in-register) pointing to the source data.
            //	destAddr: An addressing block (memory address, pointer-to-memory, or
            //		pointer-in-register) pointing to the destination address.
            //	length: An addressing block containing the length of bytes to write. Always assumed
            //		to be a DWORD.
            // Errors:
            //	InsufficientDestinationSize: Raised if the destination address is too close to the
            //		end of memory.
            //	InvalidAddressType: Raised if either the source or destination address blocks are
            //		registers, numeric literals, or string table entries.
            // Flags Byte: 00SSDDLL where SS is the source type, DD is the destination type, and
            //	LL is the length type.

            byte flagsByte = ReadProgramByte();
            AddressType sourceType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));
            AddressType destinationType = ReadAddressType((byte)((flagsByte & 0x0C) >> 2));
            AddressType lengthType = ReadAddressType((byte)(flagsByte & 0x03));

            AddressBlock sourceBlock = new AddressBlock(OperandSize.QWord, sourceType, memory, EIP);
            EIP += sourceBlock.operandLength;
            AddressBlock destinationBlock = new AddressBlock(OperandSize.QWord, destinationType, memory, EIP);
            EIP += destinationBlock.operandLength;
            AddressBlock lengthBlock = new AddressBlock(OperandSize.DWord, lengthType, memory, EIP);
            EIP += lengthBlock.operandLength;

            uint length = (uint)ReadDataFromAddressBlock(lengthBlock, OperandSize.DWord);

            ulong sourceAddress = GetMemoryAddressFromAddressBlock(sourceBlock);
            ulong destAddress = GetMemoryAddressFromAddressBlock(destinationBlock);

            memory.Write(sourceAddress, destAddress, length);
        }

        private void PushToStack()
        {
            // push <SIZE> <value>
            //	<SIZE>: The size of the data to push.
            //	<value>: An address block containing either the value to push or an address to it.
            // Errors:
            //	AddressOutOfRange: Raised if the address given is outside memory.
            //	StackOverflow: Raised if ESP leaves the bounds of memory after the push.
            // Flags Byte: SSDD0000, where SS is the size and DD is the type of the data.

            byte flagsByte = ReadProgramByte();
            OperandSize size = ReadOperandSize((byte)(flagsByte >> 6));
            AddressType dataType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));

            AddressBlock dataBlock = new AddressBlock(size, dataType, memory, EIP);
            EIP += dataBlock.operandLength;

            ulong data = ReadDataFromAddressBlock(dataBlock, size);

            memory.WriteData(data, ESP, size);
            ESP += size.SizeInBytes();
        }

        private void PopFromStack()
        {
            // pop <SIZE> <dest>
            //	<SIZE>: The size of the data to pop.
            //	<value>: An address block containing the destination to pop the value to.
            // Errors:
            //	InvalidDestinationType: Raised if the destination is a numeric literal or string table entry.
            //	AddressOutOfRange: Raised if the destination address is out of the bounds of memory.
            //	StackUnderflow: Raised if popping the data off the stack causes ESP to become less than EBP.
            // Flags Byte: SSDD0000, where SS is the size and DD is the destination type.

            byte flagsByte = ReadProgramByte();
            var size = ReadOperandSize((byte)(flagsByte >> 6));
            var sizeInBytes = size.GetSizeInBytes();
            var destType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));

            if (destType == AddressType.NumericLiteral || destType == AddressType.StringEntry)
            {
                RaiseError(Error.InvalidDestinationType);
            }

            var destBlock = new AddressBlock(size, destType, memory, EIP);
            EIP += destBlock.operandLength;

            if (ESP - sizeInBytes < EBP)
            {
                RaiseError(Error.StackUnderflow);
            }

            ulong data = memory.ReadData(ESP - sizeInBytes, size);
            ESP -= sizeInBytes;
            WriteDataToAddressBlock(data, destBlock, size);
        }

        private void ArrayRead()
        {
            // arrayread <SIZE> <index>
            //	<SIZE>: The size of all elements of the array.
            //	<index>: A DWORD containing the zero-based index of the element to read in the array.
            // Errors:
            //	StackUnderflow: Raised if the value atop the stack is less than 8 bytes.
            //	AddressOutOfRange: Raised if the index is beyond the edge of memory.
            // Flags byte: SSII0000, where SS is the size of the array elements, and II is the
            //	type of the address block of the index into the array.

            byte flagsByte = ReadProgramByte();
            var size = ReadOperandSize((byte)(flagsByte >> 6));
            var type = ReadAddressType((byte)((flagsByte & 0x30) >> 4));
            var elementSizeInBytes = size.GetSizeInBytes();

            if (ESP - 8 < EBP)
            {
                RaiseError(Error.StackUnderflow);
            }

            ulong arrayStartAddress = PopExternal(OperandSize.QWord);
            var indexBlock = new AddressBlock(OperandSize.DWord, type, memory, EIP);
            EIP += indexBlock.operandLength;

            uint arrayIndex = (uint)ReadDataFromAddressBlock(indexBlock, size);
            PushExternal(memory.ReadData(arrayStartAddress + (arrayIndex * elementSizeInBytes),
                size), size);
        }

        private void ArrayWrite()
        {
            // arraywrite <SIZE> <index> <data>
            //	<SIZE>: The size of all elements of the array.
            //	<index>: A DWORD containing the zero-based index of the element to write in the array.
            //	<data>: An address block containing the data to write.
            // Errors:
            //	StackUnderflow: Raised if the value atop the stack is less than 8 bytes.
            //	AddressOutOfRange: Raised if the index is beyond the edge of memory.
            // Flags byte: SSIIDD00, where SS is the size of the array elements, II is the type of
            //	address block containing the index of the array element, and DD is the type of the
            //	address block containing the data.

            byte flagsByte = ReadProgramByte();
            var size = ReadOperandSize((byte)(flagsByte >> 6));
            var indexType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));
            var dataType = ReadAddressType((byte)((flagsByte & 0x0C) >> 2));
            var elementSizeInBytes = size.GetSizeInBytes();

            if (ESP - 8 < EBP)
            {
                RaiseError(Error.StackUnderflow);
            }

            ulong arrayStartAddress = PopExternal(OperandSize.QWord);
            var indexBlock = new AddressBlock(OperandSize.DWord, indexType, memory, EIP);
            EIP += indexBlock.operandLength;
            var dataBlock = new AddressBlock(size, dataType, memory, EIP);
            EIP += indexBlock.operandLength;

            uint arrayIndex = (uint)ReadDataFromAddressBlock(indexBlock, size);
            ulong arrayIndexAddress = arrayStartAddress + (arrayIndex * elementSizeInBytes);
            memory.WriteData(ReadDataFromAddressBlock(dataBlock, size), arrayIndexAddress,
                size);
        }
        #endregion

        #region Integral/Bitwise Operations
        private static NumericOperation OpcodeToNumericOperation(ushort opcode)
        {
            switch ((opcode & 0xFF) % 18)
            {
                case 0: return NumericOperation.Add;
                case 1: return NumericOperation.Subtract;
                case 2: return NumericOperation.Multiply;
                case 3: return NumericOperation.Divide;
                case 4: return NumericOperation.ModDivide;
                case 5: return NumericOperation.Increment;
                case 6: return NumericOperation.Decrement;
                case 7: return NumericOperation.BitwiseAND;
                case 8: return NumericOperation.BitwiseOR;
                case 9: return NumericOperation.BitwiseXOR;
                case 10: return NumericOperation.BitwiseNOT;
                case 11: return NumericOperation.BitwiseShiftLeft;
                case 12: return NumericOperation.BitwiseShiftRight;
                case 13: return NumericOperation.LogicalAND;
                case 14: return NumericOperation.LogicalOR;
                case 15: return NumericOperation.LogicalXOR;
                case 16: return NumericOperation.LogicalNOT;
                case 17: return NumericOperation.Compare;
                default: throw new ArgumentException($"Implementation error: Invalid opcode {opcode}");
            }
        }

        private static int StackArgumentsToPop(NumericOperation operation)
        {
            switch (operation)
            {
                case NumericOperation.Add:
                case NumericOperation.Subtract:
                case NumericOperation.Multiply:
                case NumericOperation.Divide:
                case NumericOperation.ModDivide:
                case NumericOperation.BitwiseAND:
                case NumericOperation.BitwiseOR:
                case NumericOperation.BitwiseXOR:
                case NumericOperation.BitwiseShiftLeft:
                case NumericOperation.BitwiseShiftRight:
                case NumericOperation.LogicalAND:
                case NumericOperation.LogicalOR:
                case NumericOperation.LogicalXOR:
                case NumericOperation.Compare:
                    return 2;
                case NumericOperation.Increment:
                case NumericOperation.Decrement:
                case NumericOperation.BitwiseNOT:
                case NumericOperation.LogicalNOT:
                    return 1;
                default:
                    throw new ArgumentException($"Implementation error: Invalid operation {operation}");
            }
        }

        private void PerformStackOperation(ushort opcode)
        {
            // op <SIZE>
            //	<SIZE>: The size of the operands and the result.
            // Errors:
            //	StackUnderflow: Occurs if popping the operands causes ESP to be less than EBP.
            // Flags byte: SS000000, where SS is the size of the operands.

            var operation = OpcodeToNumericOperation(opcode);
            var stackArgsToPop = StackArgumentsToPop(operation);

            var flagsByte = ReadProgramByte();
            var size = ReadOperandSize((byte)(flagsByte >> 6));

            ulong right = PopExternal(size);
            ulong left = (stackArgsToPop == 2) ? PopExternal(size) : 0UL;
            ulong result = PerformNumericOperation(operation, left, right);

            if (operation != NumericOperation.Compare) { PushExternal(result, size); }
        }

        private void PerformLongOperation(ushort opcode)
        {
            // op <SIZE> <left> <right> <dest>
            //	<SIZE>: The size of the operands and the destination.
            //	<left>: The left operand.
            //	<right>: The right operand. Omitted for increment, decrement, bitwise and logical NOT.
            //	<dest>: The destination in which the operation should be stored in. Omitted for compare.
            // Errors:
            //	AddressOutOfRange: Raised if any operand is beyond the range of memory.
            //	InvalidDestinationType: Raised if the destination is a numeric literal or string table entry.
            // Flags byte: SSLLRRDD, where SS is the size, LL and RR are the types of the operands, and DD
            //	is the type of the destination. RR is always 00 if the instruction has no right operand
            //	(such as incl or decl).

            var operation = OpcodeToNumericOperation(opcode);
            var flagsByte = ReadProgramByte();

            var size = ReadOperandSize((byte)(flagsByte >> 6));
            var leftType = ReadAddressType((byte)((flagsByte & 0x30) >> 4));
            var rightType = ReadAddressType((byte)((flagsByte & 0x0C) >> 2));
            var destType = ReadAddressType((byte)(flagsByte & 0x03));

            bool isBinaryOperation = StackArgumentsToPop(operation) == 2;

            var leftBlock = new AddressBlock(size, leftType, memory, EIP);
            EIP += leftBlock.operandLength;
            var rightBlock = new AddressBlock();
            var destBlock = new AddressBlock();
            if (isBinaryOperation)
            {
                rightBlock = new AddressBlock(size, rightType, memory, EIP);
                EIP += rightBlock.operandLength;
            }
            if (operation != NumericOperation.Compare)
            {
                destBlock = new AddressBlock(size, destType, memory, EIP);
                EIP += destBlock.operandLength;
            }

            ulong left = ReadDataFromAddressBlock(leftBlock, size);
            ulong right = (isBinaryOperation) ? ReadDataFromAddressBlock(rightBlock, size) : 0UL;
            // ugh, so unary stack operations use right as the operand, but unary long operations
            // would want to use left instead. So let's just flip the operands. Not preferable, but...
            if (!isBinaryOperation)
            {
                ulong temp = left;
                left = right;
                right = temp;
            }

            ulong result = PerformNumericOperation(operation, left, right);

            if (operation != NumericOperation.Compare)
            {
                WriteDataToAddressBlock(result, destBlock, size);
            }
        }

        private ulong PerformNumericOperation(NumericOperation operation, ulong left, ulong right)
        {
            switch (operation)
            {
                case NumericOperation.Add: return left + right;
                case NumericOperation.Subtract: return left - right;
                case NumericOperation.Multiply: return left * right;
                case NumericOperation.Divide: return left / right;
                case NumericOperation.ModDivide: return left % right;
                case NumericOperation.Increment: return right + 1;
                case NumericOperation.Decrement: return right - 1;
                case NumericOperation.BitwiseAND: return left & right;
                case NumericOperation.BitwiseOR: return left | right;
                case NumericOperation.BitwiseXOR: return left ^ right;
                case NumericOperation.BitwiseNOT: return ~right;
                case NumericOperation.BitwiseShiftLeft: return left << (int)right;
                case NumericOperation.BitwiseShiftRight: return left >> (int)right;
                case NumericOperation.LogicalAND: return ((left != 0) && (right != 0)) ? 1UL : 0UL;
                case NumericOperation.LogicalOR: return ((left != 0) || (right != 0)) ? 1UL : 0UL;
                case NumericOperation.LogicalXOR: return ((left != 0) ^ (right != 0)) ? 1UL : 0UL;
                case NumericOperation.LogicalNOT: return (right == 0) ? 1UL : 0UL;
                case NumericOperation.Compare:
                    SetFlagsByComparison(left.CompareTo(right));
                    return 0UL;	// Compare doesn't push anything to the stack, but we still need to return something
                default: throw new ArgumentException($"Implementation error: Invalid operation {operation}");
            }
        }

        private static NumericOperation OpcodeToFloatingOperation(ushort opcode)
        {
            switch (opcode & 0x7F)
            {
                case 0x00: return NumericOperation.Add;
                case 0x01: return NumericOperation.Subtract;
                case 0x02: return NumericOperation.Multiply;
                case 0x03: return NumericOperation.Divide;
                case 0x04: return NumericOperation.ModDivide;
                case 0x05: return NumericOperation.Compare;
                case 0x06: return NumericOperation.SquareRoot;
                default: throw new ArgumentException($"Implementation error: Invalid opcode {opcode}");
            }
        }

        private void PerformFloatingStackOperation(ushort opcode)
        {
            var operation = OpcodeToFloatingOperation(opcode);
            var stackArgsToPop = (operation != NumericOperation.SquareRoot) ? 2 : 1;

            var flagsByte = ReadProgramByte();
            var size = ReadOperandSize((byte)(flagsByte >> 6));

            if (size != OperandSize.DWord || size != OperandSize.QWord)
            {
                throw new InvalidOperationException($"The operand size {size} is not valid. Floating operations must use DWORD (single) or QWORD (double).");
            }

            ulong rightBytes = PopExternal(size);
            ulong leftBytes = (stackArgsToPop == 2) ? PopExternal(size) : 0UL;

            double right = (size == OperandSize.QWord)
                ? BitConverter.Int64BitsToDouble((long)rightBytes)
                : ((uint)rightBytes).ToFloatBitwise();

            double left = (size == OperandSize.QWord)
                ? BitConverter.Int64BitsToDouble((long)leftBytes)
                : ((uint)leftBytes).ToFloatBitwise();
            double result = PerformFloatingOperations(left, right, operation);

            if (operation != NumericOperation.Compare)
            {
                if (size == OperandSize.DWord) { PushExternal(((float)result).ToUIntBitwise(), size); }
                else { PushExternal((ulong)BitConverter.DoubleToInt64Bits(result), size); }
            }
        }

        private double PerformFloatingOperations(double left, double right, NumericOperation operation)
        {
            switch (operation)
            {
                case NumericOperation.Add: return left + right;
                case NumericOperation.Subtract: return left - right;
                case NumericOperation.Multiply: return left * right;
                case NumericOperation.Divide: return left / right;
                case NumericOperation.ModDivide: return left % right;
                case NumericOperation.Compare:
                    SetFlagsByComparison(left.CompareTo(right));
                    return 0UL;
                case NumericOperation.SquareRoot: return Math.Sqrt(right);
                default: throw new InvalidOperationException($"Invalid floating operation {operation}.");
            }
        }

        private void SetFlagsByComparison(int comparison)
        {
            EFLAGS &= (~EFlags.EqualFlag);
            EFLAGS &= (~EFlags.LessThanFlag);
            EFLAGS &= (~EFlags.GreaterThanFlag);
            EFLAGS &= (~EFlags.LessThanOrEqualToFlag);
            EFLAGS &= (~EFlags.GreaterThanOrEqualToFlag);
            if (comparison < 0)
            {
                EFLAGS |= EFlags.LessThanFlag;
            }
            else if (comparison > 0)
            {
                EFLAGS |= EFlags.GreaterThanFlag;
            }
            else
            {
                EFLAGS |= EFlags.EqualFlag;
                EFLAGS |= EFlags.LessThanOrEqualToFlag;
                EFLAGS |= EFlags.GreaterThanOrEqualToFlag;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IronArc.Memory
{
    public class MemoryManager
    {
        private readonly List<Context> contexts;
        private ByteBlock currentContextMemory;
        private int currentContextIndex;
        private int moveDestinationContextID;
        private readonly Queue<int> destroyedContextIDs;

        public event EventHandler<ContextsChangedEventArgs> ContextsChanged;
        public event EventHandler<int> CurrentContextChanged;

        public int CurrentContextIndex
        {
            get => currentContextIndex;
            set
            {
                if (value >= contexts.Count || contexts[value] == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"No context has ID #{value}.");
                }

                currentContextIndex = value;
                // ReSharper disable once PossibleInvalidOperationException
                currentContextMemory = contexts[value].Memory;
                OnCurrentContextChanged();
            }
        }

        public int MoveDestinationContextID
        {
            get => moveDestinationContextID;
            set
            {
                if (value >= contexts.Count || contexts[value] == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"No context has ID #{value}.");
                }

                moveDestinationContextID = value;
            }
        }

        public long CurrentContextLength => (long)currentContextMemory.Length;

        public MemoryManager(ByteBlock context0Memory)
        {
            contexts = new List<Context>
            {
                new Context
                {
                    Memory = context0Memory
                }
            };
            CurrentContextIndex = 0;
            destroyedContextIDs = new Queue<int>();
        }

        public int CreateContext(ulong contextSize)
        {
            InContext0OrThrow();

            var newContext = new Context
            {
                Memory = ByteBlock.FromLength(contextSize)
            };
            var nextContextID = destroyedContextIDs.TryDequeue();

            if (nextContextID == 0)
            {
                nextContextID = contexts.Count;

                if (nextContextID == int.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(nextContextID), "Too many active contexts, cannot allocate another.");
                }
                
                contexts.Add(newContext);
            }
            else
            {
                contexts[nextContextID] = newContext;
            }

            OnContextsChanged();
            return nextContextID;
        }

        public void DestroyContext(int contextID)
        {
            InContext0OrThrow();
            
            if (contextID == 0)
            {
                throw new ArgumentException($"Cannot destroy context #{contextID}.", nameof(contextID));
            }

            if (contextID >= contexts.Count || contexts[contextID] == null)
            {
                throw new ArgumentOutOfRangeException($"No context has ID #{contextID}.");
            }
            
            destroyedContextIDs.Enqueue(contextID);
            contexts[contextID] = null;
            OnContextsChanged();
        }

        public void MoveMemoryToContext(ulong sourceAddress, ulong destinationAddress, uint count)
        {
            // ReSharper disable once PossibleInvalidOperationException
            var toContext = contexts[moveDestinationContextID].Memory;

            if (currentContextMemory.Length < sourceAddress + count
                || toContext.Length < destinationAddress + count)
            {
                throw new ArgumentOutOfRangeException(nameof(count),
                    $"Cannot move {count} bytes from 0x{sourceAddress:X16} in context {currentContextIndex} to 0x{destinationAddress:X16} in context {moveDestinationContextID}, out of range.");
            }
            
            currentContextMemory.Transfer(toContext, sourceAddress, destinationAddress, count);
        }

        public void SaveRegisterSet(ulong eax, ulong ebx, ulong ecx, ulong edx, ulong eex,
            ulong efx, ulong egx, ulong ehx, ulong ebp, ulong esp,
            ulong eflags, ulong erp)
        {
            contexts[currentContextIndex]
                .SaveRegisterSet(eax, ebx, ecx, edx, eex, efx, egx,
                    ehx, ebp, esp, eflags, erp);
        }

        public void LoadRegisterSet(out ulong eax, out ulong ebx, out ulong ecx, out ulong edx, out ulong eex,
            out ulong efx, out ulong egx, out ulong ehx, out ulong ebp, out ulong esp,
            out ulong eflags, out ulong erp)
        {
            contexts[currentContextIndex]
                .LoadRegisterSet(out eax, out ebx, out ecx, out edx, out eex, out efx, out egx,
                    out ehx, out ebp, out esp, out eflags, out erp);
        }

        private void OnContextsChanged()
        {
            ContextsChanged?.Invoke(this, new ContextsChangedEventArgs
            {
                HighestContextID = contexts.Count - 1,
                DestroyedContextIDs = destroyedContextIDs.ToList()
            });
        }

        public ulong GetContextLength(int contextId) => contexts[contextId]?.Memory.Length ?? 0UL;

        private void OnCurrentContextChanged()
        {
            CurrentContextChanged?.Invoke(this, CurrentContextIndex);
        }

        public byte[] Read(ulong address, ulong length) => currentContextMemory.ReadAt(address, length);

        public byte ReadByte(ulong address) => currentContextMemory.ReadByteAt(address);

        public byte ReadByteInContext(int contextID, ulong address)
        {
            var context = contexts[contextID];

            return context.Memory.ReadByteAt(address);
        }

        public sbyte ReadSByte(ulong address) => (sbyte)ReadByte(address);

        public ushort ReadUShort(ulong address) => currentContextMemory.ReadUShortAt(address);

        public short ReadShort(ulong address) => (short)ReadUShort(address);

        public uint ReadUInt(ulong address) => currentContextMemory.ReadUIntAt(address);

        public int ReadInt(ulong address) => (int)ReadUInt(address);

        public ulong ReadULong(ulong address) => currentContextMemory.ReadULongAt(address);

        public long ReadLong(ulong address) => (long)ReadULong(address);

        public string ReadString(ulong address, out uint length)
        {
            uint stringLength = ReadUInt(address);

            length = stringLength + 4;
            return Encoding.UTF8.GetString(Read(address + 4, stringLength));
        }

        public ulong ReadData(ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte:
                    return ReadByte(address);
                case OperandSize.Word:
                    return ReadUShort(address);
                case OperandSize.DWord:
                    return ReadUInt(address);
                case OperandSize.QWord:
                    return ReadULong(address);
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }

        public void Write(byte[] bytes, ulong address) => currentContextMemory.WriteAt(bytes, address);

        public void Write(ulong source, ulong destination, ulong length)
        {
            var bytes = Read(source, length);
            Write(bytes, destination);
        }

        public void WriteByte(byte value, ulong address) => currentContextMemory.WriteByteAt(value, address);

        public void WriteByteInContext(int contextID, byte value, ulong address)
        {
            var context = contexts[contextID];

            context.Memory.WriteByteAt(value, address);
        }

        public void WriteSByte(sbyte value, ulong address) => WriteByte((byte)value, address);

        public void WriteUShort(ushort value, ulong address) => currentContextMemory.WriteUShortAt(value, address);

        public void WriteShort(short value, ulong address) => WriteUShort((ushort)value, address);

        public void WriteUInt(uint value, ulong address) => currentContextMemory.WriteUIntAt(value, address);

        public void WriteInt(int value, ulong address) => WriteUInt((uint)value, address);

        public void WriteULong(ulong value, ulong address) => currentContextMemory.WriteULongAt(value, address);

        public void WriteLong(long value, ulong address) => WriteULong((ulong)value, address);

        public void WriteString(string value, ulong address)
        {
            var utf8 = Encoding.UTF8.GetBytes(value);
            uint stringLength = (uint)utf8.Length;
            WriteUInt(stringLength, address);
            Write(utf8, address + 4);
        }

        public void WriteData(ulong data, ulong address, OperandSize size)
        {
            switch (size)
            {
                case OperandSize.Byte:
                    WriteByte((byte)data, address);
                    break;
                case OperandSize.Word:
                    WriteUShort((ushort)data, address);
                    break;
                case OperandSize.DWord:
                    WriteUInt((uint)data, address);
                    break;
                case OperandSize.QWord:
                    WriteULong(data, address);
                    break;
                default:
                    throw new ArgumentException($"Implementation error: Invalid operand size {size}");
            }
        }

        public void TransferTo(ByteBlock dest, ulong sourceAddress, ulong destAddress, uint count) =>
            currentContextMemory.Transfer(dest, sourceAddress, destAddress, count);

        public void TransferFrom(ByteBlock source, ulong sourceAddress, ulong destAddress, uint count) =>
            source.Transfer(currentContextMemory, sourceAddress, destAddress, count);

        private void InContext0OrThrow()
        {
            if (currentContextIndex != 0)
            {
                throw new UnauthorizedAccessException("Cannot perform this operation except from context 0.");
            }
        }
    }
}

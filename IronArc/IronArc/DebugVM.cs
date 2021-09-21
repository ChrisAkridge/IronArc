using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using IronArc.Memory;

// ReSharper disable InconsistentNaming

namespace IronArc
{
    public sealed class DebugVM : IDisposable
    {
        private readonly VirtualMachine vm;
        private readonly List<Breakpoint> breakpoints;
        private readonly ConcurrentQueue<Message> uiMessageQueue;
        private readonly List<string> contextNames;
        private DebugRunUntilType runUntilType = DebugRunUntilType.NotRunning;

        public event EventHandler CallOccurred;
        public event EventHandler ReturnOccurred;
        public event EventHandler DebugDisplayInvalidated;
        public event EventHandler<ContextsChangedEventArgs> ContextsChanged;

        public long SystemMemorySize => (long)vm.SystemMemory.Length;
        public IReadOnlyList<Breakpoint> Breakpoints => breakpoints.AsReadOnly();
        public ConcurrentQueue<Message> MessageQueue { get; }
        public VMState VMState => vm.State;
        public Guid MachineID => vm.MachineId;
        public MemoryManager MemoryManager => vm.MemoryManager;

        public IEnumerable<string> ContextNames => contextNames;

        public DebugVM(VirtualMachine vm, ConcurrentQueue<Message> uiMessageQueue)
        {
            this.vm = vm;
            this.uiMessageQueue = uiMessageQueue;
            breakpoints = new List<Breakpoint>();
            MessageQueue = new ConcurrentQueue<Message>();

            this.vm.MemoryManager.ContextsChanged += OnContextsChanged;

            contextNames = new List<string>
            {
                "#0: Kernel Context",
                "#1: Hardware Memory"
            };
        }

        private void OnContextsChanged(object sender, ContextsChangedEventArgs args)
        {
            contextNames.Clear();
            for (var i = 0; i <= args.HighestContextID; i++)
            {
                if (!args.DestroyedContextIDs.Contains(i))
                {
                    contextNames.Add($"#{i}{(i == 0 ? ": Kernel Context" : i == 1 ? ": Hardware Memory" : "")}");
                }
            }
            
            ContextsChanged?.Invoke(this, args);
        }

        #region Register Properties
        public ulong EAX
        {
            get => vm.Processor.EAX;
            set => vm.Processor.EAX = value;
        }

        public ulong EBX
        {
            get => vm.Processor.EBX;
            set => vm.Processor.EBX = value;
        }

        public ulong ECX
        {
            get => vm.Processor.ECX;
            set => vm.Processor.ECX = value;
        }

        public ulong EDX
        {
            get => vm.Processor.EDX;
            set => vm.Processor.EDX = value;
        }

        public ulong EEX
        {
            get => vm.Processor.EEX;
            set => vm.Processor.EEX = value;
        }

        public ulong EFX
        {
            get => vm.Processor.EFX;
            set => vm.Processor.EFX = value;
        }

        public ulong EGX
        {
            get => vm.Processor.EGX;
            set => vm.Processor.EGX = value;
        }

        public ulong EHX
        {
            get => vm.Processor.EHX;
            set => vm.Processor.EHX = value;
        }

        public ulong EBP
        {
            get => vm.Processor.EBP;
            set => vm.Processor.EBP = value;
        }

        public ulong ESP
        {
            get => vm.Processor.ESP;
            set => vm.Processor.ESP = value;
        }

        public ulong EIP
        {
            get => vm.Processor.EIP;
            set => vm.Processor.EIP = value;
        }

        public ulong ERP
        {
            get => vm.Processor.ERP;
            set => vm.Processor.ERP = value;
        }

        public ulong EFLAGS
        {
            get => vm.Processor.EFLAGS;
            set => vm.Processor.EFLAGS = value;
        }

        public int ECC
        {
            get => vm.Processor.ECC;
            set => vm.Processor.ECC = value;
        }
        #endregion

        public UnmanagedMemoryStream CreateMemoryStream() => vm.SystemMemory.CreateStream();

        public CallStackFrame GetCallStackTop() => vm.Processor.callStack.Peek();

        public IEnumerable<CallStackFrame> GetCallStack() => vm.Processor.callStack;

        public byte ReadByte(long address) => vm.SystemMemory.ReadByteAt((ulong)address);
        public void WriteByte(long address, byte value) => vm.SystemMemory.WriteByteAt(value, (ulong)address);

        public void AddBreakpoint(ulong address, int context, bool isUserVisible) =>
            breakpoints.Add(new Breakpoint(address, context, isUserVisible));

        public bool RemoveBreakpoint(ulong address, int context) =>
            breakpoints.RemoveAll(b => b.Address == address && b.Context == context) > 0;

        public bool AddressHasUserVisibleBreakpoint(ulong address) =>
            breakpoints.Where(b => b.Address == address && b.Context == vm.MemoryManager.CurrentContextIndex)
                .Select(b => b.IsUserVisible)
                .FirstOrDefault();
        
        public bool AddressHasUserVisibleBreakpoint(ulong address, int context) =>
            (breakpoints.Where(b => b.Address == address && b.Context == context)
                .Select(b => b.IsUserVisible)).FirstOrDefault();

        private void StartInstructionLoopThread()
        {
            var uiMessage = new Message(VMMessage.None, UIMessage.VMStateChanged, vm.MachineId,
                (int)VMState.Running, 0L, null);
            uiMessageQueue.Enqueue(uiMessage);

            var worker = new Thread(InstructionLoop)
            {
                Name = runUntilType == DebugRunUntilType.StepOver
                    ? $"Step Over {{{vm.MachineId}}}"
                    : $"Debug {{{vm.MachineId}}}"
            };

            worker.Start();
        }

        private void InstructionLoop()
        {
            while (runUntilType != DebugRunUntilType.NotRunning)
            {
                vm.ExecuteOneInstruction();

                if (CheckAndBreakOnBreakpoint())
                {
                    NotifyUIThreadOfPause();
                    break;
                }

                if (!MessageQueue.IsEmpty)
                {
                    MessageQueue.TryDequeue(out var message);

                    if (message.VMMessage != VMMessage.SetVMState || (VMState)message.WParam != VMState.Paused) { continue; }

                    NotifyUIThreadOfPause();
                    break;
                }
            }
        }

        private void NotifyUIThreadOfPause()
        {
            runUntilType = DebugRunUntilType.NotRunning;

            var uiMessage = new Message(VMMessage.None, UIMessage.VMStateChanged, vm.MachineId,
                (int)VMState.Paused, 0L, null);
            uiMessageQueue.Enqueue(uiMessage);
        }

        private bool CheckAndBreakOnBreakpoint()
        {
            Breakpoint breakpointToRemove = null;

            foreach (var breakpoint in breakpoints.Where(breakpoint => EIP == breakpoint.Address))
            {
                if (!breakpoint.IsUserVisible)
                {
                    breakpointToRemove = breakpoint;
                }
                else { return true; }
            }

            if (breakpointToRemove == null) { return false; }

            breakpoints.Remove(breakpointToRemove);
            return true;
        }

        public void Run()
        {
            runUntilType = DebugRunUntilType.RunContinously;
            StartInstructionLoopThread();
        }

        public void Pause() => NotifyUIThreadOfPause();

        public void StepInto()
        {
            vm.ExecuteOneInstruction();
            OnDebugDisplayInvalidated();
        }

        public void StepOver()
        {
            bool isCallInstruction = vm.SystemMemory.ReadUShortAt(EIP) == 0x0003;
            if (!isCallInstruction)
            {
                StepInto();
            }
            else
            {
                runUntilType = DebugRunUntilType.StepOver;

                // Find the length of the call instruction so we can set a breakpoint right after
                // it.
                int callInstructionLength = 3;  // opcode + flags byte
                byte flagsByte = vm.SystemMemory.ReadByteAt(EIP + 2);
                flagsByte = (byte)((flagsByte & 0x30) >> 4);
                if (flagsByte == 0 || flagsByte == 2) { callInstructionLength += 8; }
                else if (flagsByte == 1)
                {
                    // Register: can be either 1 byte or 5 bytes if it has an offset
                    byte registerByte = vm.SystemMemory.ReadByteAt(EIP + 3);
                    if ((registerByte & 0x40) != 0) { callInstructionLength += 5; }
                    else { callInstructionLength += 1; }
                }
                else { throw new InvalidDataException($"Found a call instruction at 0x{EIP:X16} that had bad flags/operands."); }

                breakpoints.Add(new Breakpoint(EIP + (ulong)callInstructionLength, ECC, false));
                StartInstructionLoopThread();
            }
        }

        public void StepOut()
        {
            runUntilType = DebugRunUntilType.StepOut;

            var uiMessage = new Message(VMMessage.None, UIMessage.VMStateChanged, vm.MachineId,
                (int)VMState.Running, 0L, null);
            uiMessageQueue.Enqueue(uiMessage);

            var worker = new Thread(StepOutInstructionLoop)
            {
                Name = $"Step Out {{{vm.MachineId}}}"
            };
            worker.Start();
        }

        private void StepOutInstructionLoop()
        {
            while (true)
            {
                // We could do what we did in Step Over, which is set an invisible breakpoint
                // right after the call address, but we don't have an easy way to find the next
                // ret or end instruction that will actually be executed. So, instead, we're going
                // to just execute instructions until we reach a ret or end. When we reach a ret,
                // we DO want to execute it, but we DON'T want to execute an end instruction.
                ushort opcode = vm.SystemMemory.ReadUShortAt(EIP);
                if (opcode == 0x0004 /* ret */)
                {
                    vm.ExecuteOneInstruction();
                    NotifyUIThreadOfPause();
                    break;
                }

                if (opcode == 0x0001 /* end */)
                {
                    NotifyUIThreadOfPause();
                    break;
                }

                vm.ExecuteOneInstruction();
            }
        }

        private void OnDebugDisplayInvalidated() => DebugDisplayInvalidated?.Invoke(this, new EventArgs());

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            vm.MemoryManager.ContextsChanged -= OnContextsChanged;
        }
    }
}

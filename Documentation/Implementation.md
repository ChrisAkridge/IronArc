# IronArc Processor Implementation

This document describes how the processor and its required functionality will be implemented in the reference IronArc implementation.

## Memory

An existing type, `ByteBlock`, represents an unmanaged pointer to manually-allocated memory, along with its length. It has a number of useful methods that let us pull data in various types out of the unmanaged memory space. It will be used as the primary memory for all IronArc VMs.

## Addressing Blocks

An address block is implemented in the `AddressBlock` struct. It uses 32 bytes to describe an addressing block; a pointer to the memory/registers/string table or a numeric literal. Pointers-to-registers may also encode an offset. `AddressBlock` instances also keep track of whether the block is a pointer (i.e. `eax` vs. `*eax`), and the length of the addressing block in memory.

## Processor

Per the specification, the processor owns the following data:

* Eight eight-byte registers, implemented as `ulong` instances, named `EAX` through `EHX`.
* An instruction pointer `EIP`, which has the width of a memory address, eight bytes.
* A flags register `EFLAGS`, which is also eight bytes.
* A stack base pointer, `EBP`, which marks the bottom of a stack frame.
* A stack pointer, `ESP`, which marks the top of the stack.
* A relative pointer, `ERP`, which marks the address of the first instruction of the program in memory.
* Internally, a field `stackArgsMarker`, which marks the value of `ESP` when a `stackargs` instruction is executed.
* Internally, a call stack which retains information about what address to return to, plus the state of the processor at the time of a call.
* Internally, a `Dictionary<string, List<InterruptHandler>> InterruptTable` which holds addresses to call to handle interrupts.
* Internally, a `Dictionary<uint, ulong> ErrorTable` which holds addresses to call to handle errors.

Processors also hold a reference to the memory space owned by the VM, and have a way to make hardware calls into the VM.

Processors are responsible for decoding executing instructions.

## The Call Stack

The call stack is a `Stack<CallStackFrame>` in the processor. It is not exposed to the code running on the processor.

A `CallStackFrame` instance consists of:

* The address to return to upon a `ret` instruction.
* The address of the first byte of the `call` instruction.
* Copies of `EAX`-`EHX` and `EFLAGS` as they were when the `call` instruction was executed.
* The value of `EBP` when the `call` instruction is executed.

## Call Procedure

Upon a call..
1. The processor comes across a `call` instruction. It records the address to call and the first byte after the call instruction.
2. The processor creates a new call stack frames and gives it `EAX`-`EHX` `EFLAGS`, and `EBP`. The processor also gives the frame the address to call and the return address (first byte after the call instruction).
3. The processor pushes the new frame atop the call stack.
4. The processor clears `EAX`-`EHX` and `EFLAGS`.
5. The processor sets `EBP` to `stackArgsPointer`.
6. The processor sets `EIP` to the address to call.

Upon a return...
1. The processor comes across a `ret` instruction. It pops the top frame off the call stack.
2. The processor sets its registers according to the frame's registers.
3. The processor sets `EIP` to the return address.

## Hardware

A hardware device implements the `HardwareDevice` abstract class. It must implement the `HardwareCall(string, VirtualMachine)` method which accepts hardware calls.

### Core Hardware and Hardware Providers

Some hardware devices require system resources in order to work. For instance, a `TerminalDevice` needs an `ITerminal`. Just creating it from `Activator.CreateInstance` won't work as a parameterless constructor doesn't provide a way to get the `ITerminal` required in order for the terminal to work.

As a result, the concept of hardware providers is brought to light. In any assembly that implements hardware devices, a `HardwareProvider` class shall exist. This class is responsible for providing the required resources to any initializing hardware devices. This allows hardware devices to ask for their own resources in a parameterless constructor.

Core hardware is the hardware devices implemented in the IronArc assembly. Due to their special nature, the `IronArcHost` project will provide some of the required resources.

## Interrupt Handlers

The `InterruptHandler` type implements the following members: `Index`, which specifies the index returned by the `System::RegisterInterruptHandler` hardware call, and `CallAddress`, which names the address to call to handle the interrupt.

## Virtual Machine

The `VirtualMachine` type represents a virtual machine in memory. It owns the following:

* A `Processor` instance
* A `ByteBlock` instance representing system memory
* A `ConcurrentQueue<Message>` for sending messages to the host process and other VMs
* A `GUID` instance to store a unique ID for this VM.
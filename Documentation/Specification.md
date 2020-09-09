# IronArc

## Introduction

### What is IronArc?

IronArc is a virtual computer with a new virtual processor architecture, instruction set, and executable format. IronArc is similar to other hardware platforms like x86 and ARM, but a key difference is that IronArc runs entirely in software. The virtual computer is to be fairly simple and straightforward, as it does not need to be beholden to physical models of computing.

IronArc is thought to be Turing Complete, meaning that it is capable of solving any problem that any other Turing Complete machine can solve. Any program, platform, solution, or language that is Turing Complete can be implemented within the IronArc virtual machine.

### What are the goals?
The goal of the project is to create a specification defining a virtual machine using its own virtual processor architecture, instruction set, and binary executable format. An emulator will then be written for x86-based Windows machines, though the specification is platform-independent. Other tools that will be written for IronArc will include a debugger, assembler, and perhaps a C compiler. See the [Cix](https://github.com/ChrisAkridge/Cix) project for more details.

The potential for software on the IronArc platform specifically is also a goal. Everything from very simple, low level code to high-end software applications are possible in IronArc.

### Why IronArc?
This project serves as a learning experience into the low-level workings of modern computers. IronArc will implement a virtual computer from a very low level, that of the processor, registers, and memory manipulation.

IronArc is not expected to be a useful application for real-world problems, as they can be solved more easily on other platforms with pre-existing tools. This is not the goal or focus of IronArc. **Therefore, IronArc is not intended for serious usage. Do not rely on any IronArc implementations to store and manage critical or confidential data without risk of loss or security breach. This software is provided “AS IS” with no warranty. You use this software at your own risk.**

## How will it be implemented?
The specification (this document) will define the rules of the IronArc virtual machine. From this specification, implementations can be created for any platform in any language as per the terms of the MIT License. Additionally, tools for the IronArc virtual machine and other IronArc-related projects can be created per the specification or specifications for those tools.

The official implementation will be written using C# on any supported platform. The implementation will be output as a DLL library that can be referenced by other .NET projects. The official implementation will employ some usage of “unsafe” code that directly manipulates unsafe memory for performance reasons.

The official program that will run the implementation will be a Windows Form program that will provide a full environment to run and debug multiple instances of IronArc virtual machines.

Up next is the IronArc assembler, [IronAssembler](https://www.github.com/ChrisAkridge/IronAssembler). This C# program will take a text file containing IronArc instructions and assemble it into a binary file that can be loaded and executed by the VM. The format of the assembly files is defined further in this specification.

## The Processor

### Definition
The IronArc processor is a set of rules that apply to a set of data. The data, stored in memory, consists of an executable program containing instructions. Each instruction is composed of a two-byte opcode followed by zero to four operands. Each operand is an addressing block that can address multiple kinds of memory or a numeric literal. For instructions that take operands, a byte after the opcode specifies what each operand is, using two bits per operand.

The processor contains the following specific blocks of processor memory, all 8 bytes in size and initialized to 0 (except EAX) at VM start:

- Eight registers labelled EAX through EHX
- The instruction pointer register EIP, which points to the currently executing instruction in memory,
- An flags register labelled EFLAGS
- Two stack registers ESP (pointer to the top of the stack), and EBP (pointer to the start of the current stack frame)
- A relative base pointer register, ERB, which always has the address of the very first instruction of the program. This allows programs to be loaded anywhere in memory and still have jumps using absolute addresses within the program.

The processor reads and executes instruction sequentially through the memory space (real or virtual). Jump, call, and return instructions serve to change the flow of execution to different addresses. The execution of an instruction begins by reading in the opcode at EIP and incrementing EIP by two bytes. Then, the opcode is compared to a table of opcodes, and the specific instruction code is called. The instruction code will then read the required operands from memory, incrementing the instruction pointer as it goes. The operands are then used to execute the instruction. Finally, the cycle begins again as the next opcode is read from memory.

A program is loaded into memory by the host process. The host process also sets the start address, the address where the program will be loaded. EIP and ERP is set to this address and begins execution from there.

Opcodes are two-byte values that indicate the instruction to be executed. The first byte defines the “class” of instruction - a loose classification of sets of instructions. The second byte identifies the specific instruction. This allows for a total of 65,536 instructions spread across 256 classes, although the actual number of total instructions will be significantly smaller.

## Memory

### Memory Spaces
The processor has access to various spaces of memory mapped into a single address space. Memory is addressed using 64-bit pointers, and different sources of memory (system memory, hardware-mapped, memory-mapped files, etc.) are mapped to different portions of the address space.

The system memory space is the primary source of memory to the IronArc VM. This memory space has its size defined by the user. The default size is considered to be 1MB. The processor can address, read, and write memory in this space through its instructions. More advanced programs may also have code that can dynamically allocate memory.

### The Stack
Using the ESP and EBP registers, a stack can be defined somewhere in the system memory space. By default, it starts at (start address + program size), but can be set at another location when starting a VM. It is operated on by the push, pop, return, and stack arithmetic, logic, and comparison instructions.

The location of the top of the stack is defined by the ESP register. The EBP register stores the bottom of the “stack frame” - a block of stack memory used since the last call instruction.

### Memory Addresses, Pointers, and Memory Mapping
A memory address in an IronArc program is an eight-byte value that can point to most addresses available to the IronArc VM, including system memory or memory-mapped virtual hardware devices such as virtual monitors which require quick access to memory. Registers are accessed with different notation, see the section on Address Blocks below.

The bottom 55 bits of any address addresses a specific byte in a memory space. This allows the addressing of up to 8 exabytes (2^63 bytes) of memory, or 32 petabytes (2^55 bytes) per plane. The highest bit indicates that this memory address contains a pointer to another value in memory. Bits 62 to 54 contain the *plane number*, which divides the memory space into 256 planes. Plane 0 contains the system memory. Plane 1 contains any memory used by hardware devices. Planes 2 through 255 are reserved for future use.

Pointers are defined within the program itself. Within listings of assembly code, pointers are denoted with asterisk characters preceding the memory being addressed.

Machine code supports only one level of indirection per instruction, but multiple levels of indirection can be achieved by chaining instructions that move a pointer into a register or memory address. For example:

```
// eax contains a pointer to a pointer in memory
mov *eax eax    // moves the value being pointed to by eax into eax
mov *eax eax    // moves the value being pointed to by the pointer in memory into eax
```

### Virtual Memory

Plane 0 supports the concept of virtual memory. The host process contains a list of *page tables*, each of which has a 32-bit identifier and can be created or destroyed at will. Each page table contains a list of page table entries that map a 32 petabyte *virtual address space* into the "real" addresses of Plane 0 as pages of 4,096 bytes. A page table entry states the starting virtual address and the starting real address it maps to. The ending addresses can be found by adding 4,095 to the starting addresses.

One of the bits in the `EFLAGS` register is the *virtual mode bit*. If set, any address in Plane 0 that an IronArc program uses is treated as a virtual address and *translated* using the page table into a real address. If clear, all addresses directly address real memory without using translation. Address translation supports reading and writing byte ranges that span arbitrarily many pages. However, the host process will raise a `MemoryAccessOutOfBounds` error if the program tries to read or write a byte range that spans over multiple pages.

Virtual addresses are mapped to real addresses as needed. When a program tries to access a virtual address that has no real address mapped to it, a *page fault* is raised. In this case, the memory manager uses an internal value to set the starting real address of a new page table entry for the page the address resides in. This internal value is then increased by 4,096.

If there isn't enough system memory available to allocate a new page, `page compaction` occurs. Any unused pages in the real memory space are closed by copying the adjacent page into the unused page. The page table entry's starting real address is also updated to ensure the mapping still points to the correct addresses. Finally, `ENP` is set to the last allocated page. If there's still no free memory, the page fault fails, raising an `OutOfVirtualMemory` error.

### Control Flow Instructions
The `jmp`, `call`, `ret`, `je`, `jne`, `jlt`, `jgt`, `jlte`, `jgte`, `jmpa`, `hwcall`, and `stackargs` instructions all either modify or set up changes to the flow of the program.

The `jmp`, `call`, and conditional jump instructions all perform "relative jumps" - jumps relative to the start of the program, stored in ERP. ERP, or the relative register, is set to the address of the program's first instruction, and when a relative jump is performed, ERP is added to the address to be jumped to.

For example, consider a program loaded at the address `0x100000`. If a jump is performed to the instruction at `0x1000`, the actual address jumped to would be `0x100000 + 0x1000 = 0x101000`, the location of the instruction in actual memory. This allows programs to be loaded at arbitrary addresses, a powerful feature for operating system loading.

### Calling Addresses
Jumping to an address moves the instruction pointer to an address with no way of jumping back. A call is a jump that remembers which instruction made the call, as well as large parts of processor state.

A "call stack", internal to the implementation and not visible to the VM, stores a snapshot of the processor's state on each call. When a call is performed, a new call stack entry is pushed, saving the processor state at the time, and most registers are cleared for the called code to use.

Call stack entries save the following processor state:
- The address of the call instruction.
- The address that was called.
- All general purpose registers `EAX` through `EHX` are saved to the call stack entry and set to zero for the called code to use.
- The flags register `EFLAGS` is saved to the call stack entry and zeroed much like the general purpose registers.
- The stack base pointer `EBP` is saved and then set to the address of the first argument on the stack (see Calling Convention below).
- The stack pointer `ESP` is also saved and then set to the top of the stack, after all pushed arguments.
- The instruction pointer `EIP` is set to the called address.

All other state remains unchanged.

The return instruction will pop the top entry off the call stack and set all registers to the saved values in the entry. EIP is set to the address of the call instruction, which is then skipped over to continue execution after the call.

### Calling Convention
Most processor architectures store arguments to functions in registers. Since all registers are cleared on call, any arguments are pushed onto the stack before a call.

Consider the following example function:
```
int* func(int a, int b, int c);
```

To call this function with the arguments in IronArc, the first instruction to use is the `stackargs` instruction. This marks the start of the function's arguments, and any values pushed onto the stack are considered the function's arguments.

All arguments are pushed in the order they appear in the function. In our example (with made-up addresses for the arguments):
```
push DWORD *0x000000000000740A	// pushes a onto the stack
push DWORD *0x0034009F00DB00A4	// pushes b onto the stack
push DWORD *0x00007FFFFFFFFFFE	// pushes c onto the stack
```

Thus, after the pushed arguments, the stack looks like this:

```
ESP------>+===+
          | c |
          +===+
          | b |
          +===+
          | a |
stackargs>+===+
          |...|
          +===+
```

...that is, the arguments are in reverse order. Upon calling the function:

```
call 0x0000000004003212
```

...the flag set by stackargs is cleared, and `EBP` is set to the start of the arguments that were pushed - that is, the address `ESP` was at when `stackargs` was executed. ESP is still at the top of the stack, after `c`. The stack looks like this:

```
ESP------>+===+
          | c |
          +===+
          | b |
          +===+
          | a |
EBP------>+===+
```

Arguments can then be accessed using constructs like `DWORD *ebp`. for `a`, or `DWORD *ebp+8` for `c`.

## IronArc Programs

### IronArc Instruction Format
An IronArc program is, at minimum, a file containing a space for global variables, a series of bytes that encode instructions, and a list of length-prefixed strings located at the end of the file. Each instruction is composed of an opcode, a flags byte for some instructions, and one or more operands appearing as memory addresses, registers, or literal values. IronArc instructions are widely varying in size, from at least two bytes (an opcode with no flags or operands, such as the No Operation instruction) to many bytes (an opcode with flags and three operands).

The first portion of an instruction is a two-byte opcode. The opcode uniquely identifies the instruction and the processor can then tell how many operands will succeed the opcode. The high byte of an opcode defines a certain “class” of instructions, such as control flow (JMP, CALL, RET), data manipulation (PUSH, MOV, ADD), et cetera. The low byte of the opcode specifies the specific instruction. With 256 possible classes and 256 possible instructions per class, there are a total of 65,536 instructions that can be identified with these opcodes, although the actual number of instructions will be much lower in practice.

The next portion of the instruction is the flags byte. It is included in instructions that have operands. The flags byte provides information about the operands that follow, such as (but not limited to), whether the operand is a direct memory address, a pointer to a value or a literal number value. For some instructions, the flags byte also uses two bits to determine the size of the operand to operate on:

|**Bits in Flags Byte**|**Size of Operand**|**Size in Bytes**|**Equivalent C# Type**|**Equivalent Standard C Type**|
|--------------------|-----------------|---------------|--------------------|----------------------------|
| 00                 | BYTE            | 1             | byte               | uint8_t                    |
| 01                 | WORD            | 2             | ushort             | uint16_t                   |
| 10                 | DWORD           | 4             | uint               | uint32_t                   |
| 11                 | QWORD           | 8             | ulong              | uint64_t                   |

If an instruction has a size operand, it always appears first and the top two bits of the flags byte always name the size operand. The remaining 0-6 bits (or the top 0-8 bits for instructions that do not take a size operand) specify the type of each operand. The n-th bit pair specifies the type of the n-th operand. The types that can be specified are:

| **Bit Pair** | **Description**                                                                                                                                                                                                                                                                                                                                              |
|--------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 00           | The operand is an eight-byte memory address. This can address the memory, system program, stack memory, and memory mapped hardware. If the high bit is set, the memory address is treated as a pointer to another location in memory.                                                                                                                        |
| 01           | The operand is a processor register. The operand will have one byte identifying up to 64 registers. If the high bit is set, the register is treated as a pointer to a memory location. If the second-highest bit is set, the operand will have a four-byte signed integer immediately following it that represents an offset from the value in the register. |
| 10           | The operand is a literal numeric value. The size of the literal is determined by the size operand.                                                                                                                                                                                                                                                           |
| 11           | This value is reserved and is not used by any operand type.                                                                                                                                                                                                                                                                                   |

Each operand appears after the flags byte.

## IronArc Binary (IEXE) Format
The first, and simplest, format for IronArc binaries is the IronArc Binary (IEXE) format.

It starts with a header of the following form:
```
DWORD magicNumber = "IEXE";
DWORD specificationVersion;
DWORD assemblerVersion;
QWORD firstInstructionAddress;
```

The header is `4 + 4 + 4 + 8 = 20` bytes long.

The file opens with a magic number `IEXE`, followed by an IronArc specification version and an assembler version. These versions are split into the high and low words for the major and minor version.

**This specification is major version 2 and minor version 0.**

The assembler version is written by whichever assembler made the program. If the program is composed by hand, the version should be major version 0 and minor version 0. Up next is the address of the start of the first instruction.

Immediately following the header is a group of bytes that are used to store global variables. The number of bytes in this group is defined by the assembler. These bytes are all `00` by default, but can be changed by the program.

Following the global variables bytes is all the instructions of the program. The `EIP` register is initialiazed to this value and execution begins here. It will continue through the memory space unless control flow is changed by the control flow instructions.

After the instruction space is a list of strings. Each string is prefixed with the number of bytes it has as a four-byte unsigned integer, followed by the text of the string in UTF-8 format.

These programs are loaded at a start address specified when starting the VM in the memory space. If the program is larger than the assigned memory space, the processor will immediately fail. EIP and ERP is set to the specified address and execution begins from EIP. The remainder of memory is initialized to zeroes. The stack immediately follows (unless set to another address when starting the VM), as reflected by ESP = EBP = (start address + program size).

The size of the program, with the header, global variable space, and string table, in bytes, will be initially stored in the EAX register when the program is loaded. The program is free to use or clear this value as necessary.

## Hardware

### The Hardware Layer
Hardware in IronArc is implemented as a virtual layer separating physical hardware and the virtual machine. The host process contains code that interfaces with hardware devices and also has code to notify the virtual machine about events from the hardware device.

The "main" hardware device is device named "System". The system device does not map to physical hardware, but instead as a set of functions of the host process. The system device can be used to add handlers to hardware events, check which devices are connected, among other functions.

Events from hardware devices take the form of interrupts. A hardware interrupt occurs when a hardware device needs to inform the virtual machine about something that happened. For example, a keyboard will fire a hardware interrupt when a key is pressed. An IronArc program may register an interrupt handler, or a piece of code at a certain address in the system memory or system program that can handle the interrupt.

Hardware devices are assigned a name and an device ID. The name is a length-prefixed string and is the same for each instance of a hardware device. The device ID is 32 bits and varies for each instance attached to the VM.

The virtual machine may request a hardware device to perform some action or retrieve some information. The virtual machine does this through hardware calls, which are essentially advanced methods that accepts parameters and optionally return values.

Hardware may optionally map a portion of the memory space to itself. For instance, a monitor might map a portion of memory to represent the pixels on screen. The virtual machine may then read to and write from this memory space to update the screen.

### Adding and Removing Hardware
When a new hardware device is added (for instance, a keyboard, mouse, or other virtual hardware device) is connected to a virtual machine, the device is initialized. A hardware interrupt is then raised by the system device, the `HardwareDeviceAttached` interrupt. This interrupt contains a *device index* of the newly added device. The first hardware device added to the VM gets index 0, the second index 1, and so forth.

Hardware devices can be removed at any time for any reason, either physically (a physical device being unplugged), virtually (the host process removing a hardware device), or by the virtual machine itself. When a hardware device is removed by anything other than the virtual machine, the system devices raises the `HardwareDeviceRemoved` interrupt with the device index of the device removed.

### Interrupts
An interrupt is a named event with optional information raised by a hardware device. Interrupts can be used to notify the virtual machine that an event has occurred, and provide information about the event itself.

Interrupts are generated by the hardware device and can include one or more arguments containing information about the interrupt. The interrupt is sent to the host process, which references its table of interrupt handler addresses. If there is one registered event handler for the interrupt, the arguments are pushed onto the stack in reverse order, and the handler is called as if a call instruction was used (thus preserving lower stack frames). If there are no registered handlers, the interrupt and its arguments are discarded. If there are multiple registered handlers, the host process calls them in the order they were registered, pushing the arguments onto the stack before each call.

An interrupt event handler is a section of executable code in the program that can handle an interrupt and perform tasks related to it. Interrupt event handlers can be registered by placing a hardware call to the `uint8 hwcall System::RegisterInterruptHandler(uint32 deviceIndex, lpstring* interruptName, ptr handlerAddress)` hardware call. This hardware call accepts a device index, the name of the interrupt, the address of the handler, and a value indicating whether the handler is in the system program. The return code denotes the index of the handler (multiple handlers will receive sequential indices). If the call fails, the system will halt in an error state.

Up to 256 handlers can be registered on the same interrupt by calling the function multiple times with different addresses. Each handler will be given an index, starting at 0 for the first handler, up to 255. When the interrupt is raised, each registered handler will be called in the order they were registered.

The interrupt handler can handle the interrupt in any way it desires, but it must ensure that any values it pushed on the stack have been popped off. This is very important, as the program that was running before the interrupt expected the stack to be in a certain state.

Interrupt handlers can be unregistered by calling the `void hwcall System::UnregisterInterruptHandler(uint32 deviceIndex, lpstring* interruptName, uint8 handlerIndex)` hardware call. This frees the interrupt handler index, which will then be retaken first if another handler is registered for the interrupt.

In documentation, an interrupt is written as:

```c
void interrupt <DeviceName>::<InterruptName>
```

Interrupts can't return values, so their return type is written in documentation as `void`.

### Hardware Calls
A hardware call is a function of a hardware device that can be called by the virtual machine. Hardware calls accept zero or more parameters and can optionally return a result. Hardware calls can be made from any part of the program.

Before the call is made, the arguments of the call are pushed onto the stack in reverse order. Then, the program runs the hwcall instruction with a memory address pointing to a string in the table. This string contains the name of the hardware device, followed by two colons, and then the name of the function. The call is performed, and the return value (if any) is pushed onto the stack, where it can be used or discarded as necessary.

On the other side, hardware calls are intercepted by the host process. The host process performs a lookup into a table of available hardware calls and finds the appropriate hardware call. It pops the required arguments off the stack and then runs the hardware call, be it inlined in host code, or a separate function, or even a function on a physical hardware device.

Hardware call overloading is not available. “Overloads” can be performed only by hardware calls with different names.

Hardware calls should be stylized in literature and documentation in the form `<return-type> hwcall <device-name>::<function-name>(<arg-type> <arg-name>...)`. The types, typically only useful for documentation, are:

- (u)int[8/16/32/64]: A signed or unsigned 8-, 16-, 32-, or 64-bit integer.
- lpstring\*: A pointer to a UTF-8 string with its length in bytes prefixed as a 32-bit unsigned integer.
- void: Used in return types, this indicates a hardware call that returns no value.
- ptr: A pointer to a memory address.

### Hardware Memory Mapping

Hardware devices can map memory into the VM's memory space. All hardware memory is mapped into Plane 1. Each instance of a hardware device can map memory only once.

Hardware devices can directly call a function in the implementation to map memory into Plane 1. When called, the function creates a buffer of the requested size, places it into Plane 1, and returns the starting real address of the newly mapped memory, along with a pointer/reference to the buffer, so the hardware device can directly read and write to its memory. Another implementation function can be used to free mapped memory and remove it from Plane 1.

Hardware device memory is mapped contiguously into Plane 1, such that new memory is allocated immediately after the last. Freeing mapped memory does cause gaps in Plane 1, but I don't think we'll need compaction here.

The hardware calls `void hwcall System::GetHardwareDeviceDescription(ptr destination)` and `void hwcall System::GetAllHardwareDeviceDescriptions(ptr destination)` can be used by an IronArc program to find out what hardware devices are on the system and where their memory, if any, is mapped. A single device description looks like the following structure, with each field stored sequentially:

```c
struct HardwareDescription
{
	lpstring* NamePointer;
	uint32 DeviceIndex;
	uint64 MemoryStart;
	uint64 MemoryEnd;
	lpstring Name;
}
```

`MemoryStart` and `MemoryEnd` are both `0` if the device maps no memory. `NamePointer` always points to `Name`.

The result of `GetAllHardwareDeviceDescriptions` looks like the following struct:

```c
struct AllHardwareDescriptions
{
	HardwareDescription* Device0Pointer;
	HardwareDescription* Device1Pointer;
	...
	HardwareDescription Device0;
	HardwareDescription Device1;
	...
}
```

## Error Handling

### Raising Errors
An error is a notifier that the state of the virtual machine is no longer valid. Errors occur when the virtual machine performs an action that is illegal, such as dividing a number by zero or attempting to assign an eight-byte value into a four-byte memory location. Some errors are fatal, meaning that the virtual machine cannot continue execution without risking data loss. Others are more recoverable, and the virtual machine can resume execution.

Errors are composed of a four-byte error code that is shown to the user on the event of an error. These codes are mapped to error names, a table of which appears in the Table of Error Codes.

Errors can be raised from within the virtual machine, through the `void hwcall System::RaiseError(uint32 errorCode)` hardware call. However, most of the errors will be raised by the host process when an instruction from the virtual machine performs an illegal action. The specific method of error raising varies by implementation, but the following requirements must be met:

- Errors, when raised, will temporarily halt the execution of the virtual machine. All registers, memory, and hardware devices will remain loaded, in the state they were in immediately before the error.
- The host process should provide the ability to continue execution on any non-fatal error. Optionally, the host process may also allow execution to continue on fatal errors as well, but this could result in data loss.
- The host process displays the error code, error name, registers, and loaded hardware devices to the user in result of an error.

If the user chooses to continue execution, execution resumes from the instruction AFTER the one that raised the error.

If the user chooses to halt execution, the virtual machine will not continue, but all output will still be left visible.

### Handling Errors
Some non-fatal errors can be handled without the virtual machine halting execution. This involves a hardware call to a system device to register an error handler, a piece of code that handles an specific error.

To register an error handle, perform a hardware call to the `void hwcall System::RegisterErrorHandler(uint32 errorCode, ptr handlerAddress)`. When this specific error occurs, the host process will make the virtual machine call the handler WITHOUT pushing any arguments onto the stack.

The error handler can clean up and return, or merely return, or raise the same error code to truly halt execution without calling the handler again.

A flag in the host process will determine if the virtual machine is within an error handler; it is set when the call to an error handler is performed, and is cleared on the return.

Error handlers must be stack-neutral; any values pushed onto the stack while in an error handler must be popped before returning to the main code. Upon return, the virtual machine continues execution from the instruction after the one that raised the error.

Unlike interrupt handlers, error handlers can only be registered once for each error. However, error handlers can jump or call other addresses as necessary. The host process's error handler flag is not cleared until the error handler itself returns, though.

Error handlers can be unregistered by calling the `void hwcall System::UnregisterErrorHandler(uint32 errorCode)` hardware call.

### Error Information

The hardware calls `uint64 hwcall System::GetLastErrorDescriptionSize()` and `void hwcall System::GetLastErrorDescription(ptr destination)` write the following structure describing the last raised error to memory:

```c
struct ErrorDescription
{
	lpstring* MessagePointer;
	uint32 ErrorCode;
	lpstring Message;
}
```

### Checked and Unchecked Math
By default, overflows and underflows when using integral instructions will not raise any errors. However, if the `CHECKED` flag on `EFLAGS` is set, `ArithemticOverflow` and `ArithmeticUnderflow` errors will be raised when overflows or underflows occur.
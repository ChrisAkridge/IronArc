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

The official implementation will be written for x86-based Windows machines using C# and the .Net Framework 4.0. The implementation will be output as a DLL library that can be referenced by other .Net projects. The official implementation will employ some usage of “unsafe” code that directly manipulates unsafe memory for performance reasons.

The official program that will run the implementation will be a Windows Form program that will provide a full environment to run and debug multiple instances of IronArc virtual machines.

Up next is the IronArc assembler, [IronAssembler](https://www.github.com/ChrisAkridge/IronAssembler). This C# program will take a text file containing IronArc instructions and assemble it into a binary file that can be loaded and executed by the VM. The format of the assembly files is defined further in this specification.

## The Processor

### Definition
The IronArc processor is a set of rules that apply to a set of data. The data, stored in memory, consists of an executable program containing instructions. Each instruction is composed of a two-byte opcode followed by zero to four operands. Each operand is an addressing block that can address multiple kinds of memory or a numeric literal. For instructions that take operands, a byte after the opcode specifies what each operand is, using two bits per operand.

The processor contains the following specific blocks of processor memory:

* Eight eight-byte registers labelled EAX through EHX
* The instruction pointer EIP, which points to the currently executing instruction in memory,
* A flags register labelled EFLAGS
* Two stack registers ESP (pointer to the top of the stack), and EBP (pointer to the start of the current stack frame)
* A relative base pointer register, ERB, which always has the address of the very first instruction of the program. This allows programs to be loaded anywhere in memory and still have jumps using absolute addresses within the program.

The processor reads and executes instruction sequentially through the memory space. Jump, call, and return instructions serve to change the flow of execution to different addresses. The execution begins by reading in the opcode at EIP and incrementing EIP by two bytes. Then, the opcode is compared to a table of opcodes, and the specific instruction code is called. The instruction code will then read the required operands from memory, incrementing the instruction pointer as it goes. The operands are then used to execute the instruction. Finally, the cycle begins again as the next opcode is read from memory.

A program is loaded into memory by the host process. The host process also sets the start address. EIP and ERP is set to this address and begins execution from there.

Opcodes are two-byte values that indicate the instruction to be executed. The first byte defines the “class” of instruction - a loose classification of sets of instructions. The second byte identifies the specific instruction. This allows for a total of 65,536 instructions spread across 256 classes, although the actual number of total instructions will be significantly smaller.

## Memory

### Memory Spaces
The processor has access to various spaces of memory mapped into a single address space. Memory is addressed using 64-bit pointers, and different sources of memory (main memory, hardware-mapped, filesystem, etc.) are mapped to different portions of the address space.

The main memory space is the primary source of memory to the IronArc VM. This memory space has its size defined by the user. The default size is considered to be 1MB. The processor can address, read, and write memory in this space through its instructions. More advanced programs may also have code that can dynamically allocate memory.

### The Stack
Using the ESP and EBP registers, a stack can be defined somewhere in the main memory space. By default, it starts at (start address + program size), but can be set at another location when starting a VM. It is operated on by the push, pop, return, and stack arithmetic, logic, and comparison instructions.

The location of the top of the stack is defined by the ESP register. The EBP register stores the bottom of the “stack frame” - a block of stack memory used since the last call instruction.

### Memory Addresses, Pointers, and Memory Mapping
A memory address in an IronArc program is an eight-byte value that can point to most addresses available to the IronArc VM, including main memory or memory-mapped virtual hardware devices such as virtual monitors which require quick access to memory. Registers are accessed with different notation, see the section on Address Blocks below.

The bottom 63 bits of any address addresses a specific byte in a memory space. This allows the addressing of up to 8 exabytes (2^63 bytes) of memory. The highest bit indicates that this memory address contains a pointer to another value in memory.

Memory-mapped hardware devices are assigned memory spaces by the host process - a `HWMemoryMapped` interrupt is fired, and the argument contains the byte that the mapping uses.

Pointers are defined within the program itself. Within listings of assembly code, pointers are denoted with asterisk characters preceding the memory being addressed.

Machine code supports only one level of indirection per instruction, but multiple levels of indirection can be achieved by chaining instructions that move a pointer into a register or memory address. For example:

```
// eax contains a pointer to a pointer in memory
mov *eax eax    // moves the value being pointed to by eax into eax
mov *eax eax    // moves the value being pointed to by the pointer in memory into eax
```

### Control Flow Instructions
The `jmp`, `call`, `ret`, `je`, `jne`, `jlt`, `jgt`, `jlte`, `jgte`, `jmpa`, `hwcall`, and `stackargs` instructions all either modify or set up changes to the flow of the program.

The `jmp`, `call`, and conditional jump instructions all perform "relative jumps" - jumps relative to the start of the program, stored in ERP. ERP, or the relative register, is set to the address of the program's first instruction, and when a relative jump is performed, ERP is added to the address to be jumped to.

For example, consider a program loaded at the address `0x100000`. If a jump is performed to the instruction at `0x1000`, the actual address jumped to would be `0x100000 + 0x1000 = 0x101000`, the location of the instruction in actual memory. This allows programs to be loaded at arbitrary addresses, a powerful feature for operating system loading.

### Calling Addresses
Jumping to an address moves the instruction pointer to an address with no way of jumping back. A call is a jump that remembers which instruction made the call, as well as large parts of processor state.

A "call stack", internal to the implementation and not visible to the VM, stores a snapshot of the processor's state on each call. When a call is performed, a new call stack entry is pushed, saving the processor state at the time, and most registers are cleared for the called code to use.

Call stack entries save the following processor state:
* The address of the call instruction.
* The address that was called.
* All general purpose registers `EAX`-`EHX` are saved to the call stack entry and set to zero for the called code to use.
* The flags register `EFLAGS` is saved to the call stack entry and zeroed much like the general purpose registers.
* The stack base pointer `EBP` is saved and then set to the address of the first argument on the stack (see Calling Convention below).
* The stack pointer `ESP` is also saved and then set to the top of the stack, after all pushed arguments.
* The instruction pointer `EIP` is set to the called address.

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
An IronArc program is, at minimum, a file containing a series of bytes that encode instructions, along with a string table located at the end of the file. Each instruction is composed of an opcode, a flags byte for some instructions, and one or more operands appearing as memory addresses or literal values. IronArc instructions are widely varying in size, from at least two bytes (an opcode with no flags or operands, such as the No Operation instruction) to many bytes (an opcode with flags and four operands).

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
| 11           | The operand is a four-byte unsigned index to a string in the string table.                                                                                                                                                                                                                                                                                   |

Each operand appears after the flags byte.

## IronArc Flat(-ish) Binary (IFB) Format
The first, and simplest, format for IronArc binaries is the IronArc Flat Binary (IFB) format. It starts with a 32-bit unsigned integer that states the number of instructions, followed by only instructions and a string table, with no header or padding. All bytes in the file are executed sequentially until the processor enters an error state, halts, or executes its way through the memory space.

At the end of the instruction space is a string table. The string table start with a 32-bit unsigned integer stating the number of strings in the table, followed by all the strings. Each string is prefixed with the number of bytes it is as a four-byte unsigned integer, followed by the text of the string in UTF-8 format.

IronArc Flat Binary files are stored on the host machine as files with the IEXE extension, although any arbitrary file can be used as a program (most of them will crash the processor or invalidate state, though).

These programs are loaded at a start address specified when starting the VM in the memory space. If the program is larger than the assigned memory space, the processor will immediately fail. EIP and ERP is set to the specified address and execution begins from EIP. The remainder of memory is initialized to zeroes. The stack immediately follows (unless set to another address when starting the VM), as reflected by ESP = EBP = (start address + program size).

**The size of the program, in bytes, will be initially stored in the EAX register when the program is loaded. All IFB-format programs must account for this and all implementations must place the size of the program within EAX.** The program is free to use or clear this value as necessary.

## Hardware

### The Hardware Layer
Hardware in IronArc is implemented as a virtual layer separating physical hardware and the virtual machine. The host process contains code that interfaces with hardware devices and also has code to notify the virtual machine about events from the hardware device.

The "root" hardware device is device named "System". The system device does not map to physical hardware, but instead as a set of functions of the host process. The system device can be used to add handlers to hardware events, check which devices are connected, among other functions.

Events from hardware devices take the form of interrupts. A hardware interrupt occurs when a hardware device needs to inform the virtual machine about something that happened. For example, a keyboard will fire a hardware interrupt when a key is pressed. An IronArc program may register an interrupt handler, or a piece of code at a certain address in the main memory or system program that can handle the interrupt.

Hardware devices are assigned hardware numbers, a two-byte number that uniquely identifies the hardware device. These numbers are assigned sequentially as device are connected, but are not reused until the virtual machine is restarted or the `ClearHardwareNumbers` hardware call. The system receives HWN #0 and will always hold that number. If a total of 65,535 devices are connected to the device, either concurrently or separately, no new hardware devices can be connected, and the system device will raise the `HardwareNumberSpaceExhausted` interrupt.

The virtual machine may request a hardware device to perform some action or retrieve some information. The virtual machine does this through hardware calls, which are essentially advanced methods that accepts parameters and optionally return values.

Hardware may optionally map a portion of the memory space to itself. For instance, a monitor might map a portion of memory to represent the pixels on screen. The virtual machine may then read to and write from this memory space to update the screen.

### Adding and Removing Hardware
When a new hardware device is added (for instance, a keyboard, mouse, or other virtual hardware device) is connected to a virtual machine, the device is initialized. A hardware interrupt is then raised by the system device, the `HardwareDeviceConnected` interrupt. This interrupt contains the hardware number.

Hardware devices can be removed at any time for any reason, either physically (a physical device being unplugged), virtually (the host process removing a hardware device), or by the virtual machine itself. When a hardware device is removed by anything other than the virtual machine, the system devices raises the `HardwareDeviceRemoved` interrupt with the hardware number of the device removed.

### Interrupts
An interrupt is a named event with optional information raised by a hardware device. Interrupts can be used to notify the virtual machine that an event has occurred, and provide information about the event itself.

Interrupts are generated by the hardware device and can include one or more arguments containing information about the interrupt. The interrupt is sent to the host process, which references its table of interrupt handler addresses. If there is one registered event handler for the interrupt, the arguments are pushed onto the stack in reverse order, and the handler is called as if a call instruction was used (thus preserving lower stack frames). If there are no registered handlers, the interrupt and its arguments are discarded. If there are multiple registered handlers, the host process calls them in the order they were registered, pushing the arguments onto the stack before each call.

An interrupt event handler is a section of executable code in the program that can handle an interrupt and perform tasks related to it. Interrupt event handlers can be registered by placing a hardware call to the `int8 System::RegisterInterruptHandler(uint32 hardwareNumber, lpstring interruptName, uint64 handlerAddress)` hardware call. This hardware call accepts a hardware number, the name of the interrupt, the address of the handler, and a value indicating whether the handler is in the system program. The return code denotes the index of the handler (multiple handlers will receive sequential indices). If the call fails, the system will halt in an error state.

Up to 256 handlers can be registered on the same interrupt by calling the function multiple times with different addresses. Each handler will be given an index, starting at 0 for the first handler, up to 255. When the interrupt is raised, each registered handler will be called in the order they were registered, with the arguments being placed on the stack on each call.

The interrupt handler can handle the interrupt in any way it desires, but it must ensure that all arguments are removed from the stack before it returns. This is very important, as the program that was running before the interrupt expected the stack to be in a certain state.

Interrupt handlers can be unregistered by calling the `void System::UnregisterInterruptHandler(uint32 hardwareNumber, lpstring interruptName, uint8 handlerIndex)` hardware call. This frees the interrupt handler index, which will then be retaken first if another handler is registered for the interrupt.

### Hardware Calls
A hardware call is a function of a hardware device that can be called by the virtual machine. Hardware calls accept zero or more parameters and can optionally return a result. Hardware calls can be made from any part of the program.

Before the call is made, the arguments of the call are pushed onto the stack in reverse order. Then, the program runs the hwcall instruction with a memory address pointing to a string in the table. This string contains the name of the hardware device, followed by two colons, and then the name of the function. The call is performed, and the return value (if any) is pushed onto the stack, where it can be used or discarded as necessary.

On the other side, hardware calls are intercepted by the host process. The host process performs a lookup into a table of available hardware calls and finds the appropriate hardware call. It pops the required arguments off the stack and then runs the hardware call, be it inlined in host code, or a separate function, or even a function on a physical hardware device.

Hardware call overloading is not available. “Overloads” can be performed only by hardware calls with different names.

Hardware calls should be stylized in literature and documentation in the form `<return-type> hwcall <device-name>::<function-name>(<arg-type> <arg-name>...)`. The types, typically only useful for documentation, are:

* (u)int[8/16/32/64]: A signed or unsigned 8-, 16-, 32-, or 64-bit integer.
* lpstring: A UTF-8 string with its length in bytes prefixed as a 32-bit unsigned integer.
* void: Used in return types, this indicates a hardware call that returns no value.
* ptr: A pointer to a memory address.

## Error Handling
### Raising Errors
An error is a notifier that the state of the virtual machine is no longer valid. Errors occur when the virtual machine performs an action that is illegal, such as dividing a number by zero or attempting to assign an eight-byte value into a four-byte memory location. Some errors are fatal, meaning that the virtual machine cannot continue execution without risking data loss. Others are more recoverable, and the virtual machine can resume execution.

Errors are composed of a four-byte error code that is shown to the user on the event of an error. These codes are mapped to error names, a table of which appears in the Table of Error Codes.

Errors can be raised from within the virtual machine, through the `System::RaiseError(uint32 errorCode)` hardware call. However, most of the errors will be raised by the host process when an instruction from the virtual machine performs an illegal action. The specific method of error raising varies by implementation, but the following requirements must be met:

* Errors, when raised, will temporarily halt the execution of the virtual machine. All registers, memory, and hardware devices will remain loaded, in the state they were in immediately before the error.
* The host process should provide the ability to continue execution on any non-fatal error. Optionally, the host process may also allow execution to continue on fatal errors as well, but this could result in data loss.
* The host process displays the error code, error name, registers, and loaded hardware devices to the user in result of an error.

If the user chooses to continue execution, execution resumes from the instruction AFTER the one that raised the error.

If the user chooses to halt execution, the virtual machine will not continue, but all output will still be left visible.

### Handling Errors
Some non-fatal errors can be handled without the virtual machine halting execution. This involves a hardware call to a system device to register an error handler, a piece of code that handles an specific error.

To register an error handle, perform a hardware call to the `System::RegisterErrorHandler(uint32 errorCode, ptr handlerAddress)`. When this specific error occurs, the host process will make the virtual machine call the handler WITHOUT pushing any arguments onto the stack.

**The error handler can clean up and return, or merely return, or raise the same error code to truly halt execution without calling the handler again.**

**A flag in the host process will determine if the virtual machine is within an error handler; it is set when the call to an error handler is performed, and is cleared on the return.**

Error handlers must be stack-neutral; any values pushed onto the stack while in an error handler must be popped before returning to the main code. Upon return, the virtual machine continues execution from the instruction after the one that raised the error.

Unlike interrupt handlers, error handlers can only be registered once for each error. However, error handlers can jump or call other addresses as necessary. The host process's error handler flag is not cleared until the error handler itself returns, though.

Error handlers can be unregistered by calling the `System::UnregisterErrorHandler(uint32 errorCode)` hardware call.

### Checked and Unchecked Math
By default, overflows and underflows when using integral instructions will not raise any errors. However, if the `CHECKED` flag on `EFLAGS` is set, `ArithemticOverflow` and `ArithmeticUnderflow` errors will be raised when overflows or underflows occur.

## Implementation

**NOTE: This section is outdated and will be revised.**

### Basic Implementation
The following section describes the official implementation of the IronArc Virtual Machine, as written by [Chris](https://github.com/ChrisAkridge) and [Alex](https://github.com/tl-aram) Akridge. This section describes only the official implementation, but can be used as a guide for other implementations.

The official implementation will be written in C# using the .NET Framework. Windows Forms will be used to display a user interface, along with custom controls for things like the debugger.

The first section of the implementation is the virtual machine, composed of the processor and memory classes. These classes contain the logic for the processor, its instruction set, and the relations between processor and memory.

The debugger allows the user to view a disassembled version of the currently executing code, the memory space, the state of the registers, and other information. The debugger also allows the user to step through individual instructions, to step over or out of calls, and to manipulate the memory and registers.

The terminal is a window that displays the output of the virtual machine, and allows it to accept input. It is a hardware device, and has hardware calls that can read input and write output.

The forms layer contains the forms necessary to display the information, settings, and options to the user. The user can load program files or mount folders as disk storage through these forms.

### The Virtual Machine
The virtual machine is implemented as the following classes: `Processor, Memory, AddressBlock, Hardware`, and potentially others.

The `Processor` class contains all the methods for each instruction, as well as the instruction processor, a method containing a switch block of switch blocks that call instruction methods. Other methods include methods that call methods in the Memory class that read bytes from memory.

The Memory class contains a dictionary of MemoryBlocks representing the mapped memory spaces, and an indexer that accepts an address and retrieves or writes a byte from the mapped memory spaces. Other methods read or write larger blocks of memory as the numeric types of the BCL, and as UTF-8 strings. Finally, methods that map or unmap memory blocks are also available.

The memory of the IronArc virtual machine is allocated manually on the host machine through the `Marshal.AllocHGlobal` method. The `IntPtr` result of this method is converted into a byte pointer for quicker access than a managed byte array. The memory structures implement `IDisposable`, which allows the host process to deterministically reclaim memory.

The `AddressBlock` structure allows for quick and easy access to memory addresses. It can be constructed from the processor using EIP to read the next bytes to retrieve the address. The structure holds the address as a ulong and a number of properties that can retrieve the properties of the address.

The `Hardware` class is a static class interfacing the virtual machine's hardware support to the hardware device classes and the physical hardware. It contains methods that register hardware calls into a dictionary, intercepts hardware calls from the virtual machine, takes the arguments, and performs the correct call.

### The Debugger
The debugger is a component with a form that allows the user to view and modify the state of a virtual machine. The debugger form consists of a disassembled listing of instructions of the current program, a hexadecimal display of the memory, and a listing of the registers and their values. Controls allow the user to resume, step into, step over, step out, animate, or stop the execution of the virtual machine.

The disassembly listing displays a list of instructions and operands pertaining to the currently executing program in the memory space. The text of the disassembly is displayed in a monospaced font with the memory addresses displayed on the left, followed by the bytes of the instruction, the opcode, and its operands. The currently executing instruction is displayed with a yellow background.

The disassembly is produced by a method that takes a memory block, and address, and number of instructions, and returns a data structure containing disassembly strings and other metadata.

The user can right-click an instruction to summon a context menu. This menu has options to edit the instruction, or move the program counter to that instruction. The first option summons a dialog box displaying the bytes in hexadecimal format in a textbox, with the disassembled instruction beneath it. The user can edit the textbox to change the instruction or operands. The user cannot change the length of the instruction, though. If the user chooses to move the program counter to the selected instruction, execution will resume from that point.

The hexadecimal memory display has a combobox that lists the mapped memory spaces, sorted by their signifiers, including the stack. Beneath the combobox is a hex editor containing the memory space. Each line consists of a four-byte address, eight bytes of memory, and that memory rendered in ASCII.

The register display shows all processor registers as labels and editable textboxes. The user can edit the textboxes to contain any byte string, or paste in any representation of bytes. The fields can only be edited while execution is stopped, and they're only updated when execution is stopped or the debugger is in animation mode.

The control buttons allow the user to start or stop execution. The resume button continues execution and the debugger stops updating until execution stops again. The pause button stops execution, updates the debugger, and frees the controls for the user to edit. The step into button executes each instruction individually and steps into method calls, updating the debugger each time. The step out of button executes all instructions up to the next return instruction, and stops execution at the instruction after the call instruction. The step over instruction steps over any call instructions, executing all the instructions with it, and placing the debugger at the instruction after the call. The animate button executes each instruction but updates the debugger after each instruction, allowing the user to see the flow of execution.

### The Terminal
The terminal is a form containing a textbox that is used for input and output through the virtual machine. It appears as a hardware device with hardware calls for reading input and writing output, including formatted output, to the terminal. It is not a memory-mapped device and doesn’t act like the VGA-mode console.

To read input from the terminal, the following hardware calls are available:
* `lpstring hwcall Terminal::ReadLine()`: This method reads a line of input from the terminal, from the first character up to (and omitting) the newline. The virtual machine will wait for the call to return before resuming execution.
* `uint16 hwcall Terminal::ReadChar()`: This method reads a single UTF-16 characters from the terminal. The virtual machine will wait for the call to return before resuming execution.
* `lpstring hwcall Terminal::GetLine(uint32 lineNumber)`: Returns the entire line for the requested line number. The host process may throw an exception or return an empty string (four zero bytes) if the line number is out of bounds. This call does not force the virtual machine to wait.
* `void hwcall Terminal::GetText(ptr writeLocation, uint32 length)`: Copies all terminal text to the given pointer as a length-prefixed string.

To write input to the terminal, the following hardware calls are available:
* `void hwcall Terminal::Write(lpstring value)`: Writes a string to the terminal.
* `void hwcall Terminal::WriteLine(lpstring value)`: Writes a string to the terminal, followed by a newline.
* `void hwcall Terminal::WriteFormatted(..., lpstring value, uint32 argCount)`: Writes a formatted string to the terminal. The formatting for text is equivalent to the C# string formatting. The first parameter is a variable list of values that will form the formatted string, the second parameter is the string containing the format signifiers, and the final argument is the number of arguments in the first argument. Ensure the last variable is correct, otherwise the stack will be off.
*` void hwcall Terminal::WriteLineFormatted(..., lpstring value, uint32 argCount)`: This method writes a formatted string to the terminal, followed by the newline. The parameters are the same as the hardware call above.

## Assembly Code Format
### Composition
IronArc assembly code is composed of instructions, labels, a header, and resources. The actual format for assembly will vary based on the version of IronArc and the assemblers being used. This section lays out a certain standard, but other standards for assembly code can be created for other assemblers.

The header is at the top of the file. As of this specification, the header consists only of the text “IronArc Assembly” and can be omitted. The header and its variables will be defined as we require them.

Following the header begins the list of instructions. Each instruction is written as a mnemonic, followed by the operands for the instruction. Labels can be defined throughout, each label consisting of a string of letters and numbers starting with a letter and ending in a colon. Instructions inside a labelled block may optionally be indented to show they are inside a labelled block. In the assembly stage, labels are turned into the offset of their first instructions. An empty label followed by another label is given the offset of the first instruction of the second label.

If an operand requires a specific size in the assembly result or during execution, the size occurs before all operands as one of `BYTE`, `WORD`, `DWORD`, and `QWORD`, for 1, 2, 4, and 8-byte values, respectively. For instance, to push the value of 5 onto the stack as a 32-bit integer, the instruction would be `push WORD 5`.

Execution begins at offset 0 of the assembled file, unless in an operating system context. It will continue through all instructions, even those in a labelled block, until it reach an operation that changes the program counter or the END instruction, which permanently halts execution.

If the assembled file runs out of instructions before an END instruction or unconditional jump, the virtual machine will continue executing memory past the end of the program. Depending on what is contained, nothing may happen (all NOP instructions if all memory is zeroes), or the system might crash or data might be corrupted.
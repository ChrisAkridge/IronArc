# List of IronArc Processor Instructions
## Control Flow
### No Instruction
- Opcode: `0x0000`
- Mnemonic: `nop`
- Arguments: None
- Description: Performs no meaningful operation. Fills space.
- Errors: None

### End
- Opcode: `0x0001`
- Mnemonic: `end`
- Arguments: None
- Description: Immediately and permanently halts execution. Used at the end of programs.
- Errors: None

### Relative Jump
- Opcode: `0x0002`
- Mnemonic: `jmp`
- Arguments: `jmp <address>`
    * `<address>`: An addressing block containing the memory address to jump to.
- Description: Sets `EIP` to the given address + `ERP`. This moves execution to the given address.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Call
- Opcode: `0x0003`
- Mnemonic: `call`
- Arguments: `call <address>`
	- `<address>`: An addresing block containing the memory address to call.
- Description: Pushes the current processor state (registers, flags, EIP, etc.) onto the Call Stack and then clears the processor state. Then, if the `STACKARGS` flag on `EFLAGS` is set, sets `EBP` to the address `ESP` was at when `stackargs` was executed, otherwise sets `EBP` to the top of the stack. Then, the given address + `ERP` is jumped to.
- Errors:
	- `InvalidAddressType`: Raised if the given address block does not point to a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space.
	- `CallStackOverflow`: Raised if the Call Stack is full and cannot have values pushed onto it.

### Return
- Opcode: `0x0004`
- Mnemonic: `ret`
- Arguments: None
- Description: Used to return from a call instruction. This instruction pops the topmost processor state on the Call Stack into the current processor state.
- Errors:
	- `CallStackEmpty`: Raised if the call stack has no processor states loaded onto it.

### Jump if Equal
- Opcode: `0x0005`
- Mnemonic: `je`
- Arguments: `je <address>`
	- `<address>`: An addressing block containing the memory address to jump to.
- Description: Performs a jump to the specified address + `ERP` if the equal flag on `EFLAGS` is set.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Jump if Not Equal
- Opcode: `0x0006`
- Mnemonic: `jne`
- Arguments: `jne <address>`
	- `<address>`: An addressing block containing the memory address to jump to.
- Description: Performs a jump to the specified address + ERP if the equal flag on `EFLAGS` is clear.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Jump if Less Than
- Opcode: `0x0007`
- Mnemonic: `jlt`
- Arguments: `jlt <address>`
	- `<address>`: An addressing block containing the memory address to jump to.
- Description: Performs a jump to the specified address + `ERP` if the less-than flag on `EFLAGS` is set.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Jump if Greater Than
- Opcode: `0x0008`
- Mnemonic: `jgt`
- Arguments: `jgt <address>`
	- `<address>`: An addressing block containing the memory address to jump to.
- Description: Performs a jump to the specified address + `ERP` if the greater-than flag on `EFLAGS` is set.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Jump if Less Than or Equal
- Opcode: `0x0009`
- Mnemonic: `jlte`
- Arguments: `jlte <address>`
	- `<address>`: An addressing block containing the memory address to jump to.
- Description: Performs a jump to the specified address + `ERP` if the equal flag or the less-than flag on `EFLAGS` is set.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Jump if Greater Than or Equal
- Opcode: `0x000A`
- Mnemonic: `jgte`
- Arguments: `jgte <address>`
	- <address>: An addressing block containing the memory address to jump to.
- Description: Performs a jump to the specified address + `ERP` if the equal flag or the greater-than flag on `EFLAGS` is set.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Absolute Jump
- Opcode: `0x000B`
- Mnemonic: `jmpa`
- Arguments: `jmpa <address>`
- Description: Performs an absolute jump to an address, that is, a jump to an address to which `ERP` is not added.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Hardware Call
- Opcode: `0x000C`
- Mnemonic: `hwcall`
- Arguments: `hwcall <addressToString>`
- Description: Loads a length-prefixed string that contains a hardware command from the given address and performs a hardware call to perform the command. ~~If any arguments are needed for the call, `stackargs` should be executed, then all arguments should be pushed before the call.~~ I don't think we need `stackargs` for these.
- Errors:
	- `InvalidAddressType`: Raised if the given address block is not a memory address.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.
	- `InvalidString`: Raised if the string at the address is not a length-prefixed UTF-8 string.
	- `InvalidHWCall`: Raised if the string at the address does not name any hardware call.
	- Plus any number of errors that can be raised by any hardware call.

### Removed Instruction (formerly Stack Argument Marker)
- Opcode: `0x000D`
- Mnemonic: None
- Arguments: None
- Description: ~~Indicates that anything pushed onto the stack is going to be an argument to a function call. Calling a function sets EBP equal to ESP to create a new stack frame. Since arguments are pushed on the stack, anything pushed after a stackargs instruction will be placed after EBP.~~ Removed since I realized that having one stackargs marker prevents nested function calls.

## Data Operations
### Move Data
- Opcode: `0x0100`
- Mnemonic: `mov`
- Arguments: `mov SIZE <source-addr> <dest-addr>`
	- `SIZE`: The size of the value to move.
	- `<source-addr>`: An addressing block containing the value to move.
	- `<dest-addr>`: An addressing block containing the location to move it to.
- Description: Moves a value into a given destination.
- Errors:
	- `InsufficientDestinationSize`: Raised if the destination size is too small for the data.
	- `InvalidDestination`: Raised if the destination cannot receive a value; for example, if the destination is a numeric literal.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Move Data with Length
- Opcode: `0x0101`
- Mnemonic: `movln`
- Arguments: `movln <source-addr> <dest-addr> <length>`
	- `<source-addr>`: An addressing block containing the value to move.
	- `<dest-addr>`: An addressing block containing the location to move it to.
	- `<length>`: A four-byte unsigned integer representing the size of the value to move, in bytes.
- Description: Moves a certain length of a value into a given destination
- Errors:
	- `InsufficientDestinationSize`: Raised if the destination size is too small for the data.
	- `InvalidDestination`: Raised if the destination cannot receive a value; for example, if the destination is a numeric literal.
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.

### Push to Stack
- Opcode: `0x0102`
- Mnemonic: `push`
- Arguments: `push <SIZE> <data>`
	- `SIZE`: The size of the value to move.
	- `<data>`: An addressing block containing either the data to move, or an address or pointer at which it can be found.
- Description: Depending on the value of `SIZE`, pushes a `BYTE`, `WORD`, `DWORD`, or `QWORD` value onto the stack by writing it to the memory pointed to by `ESP` then adding the size to `ESP`.
- Errors:
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.
	- `StackOverflow`: Raised if `ESP` points to an invalid address after the push.

### Pop from Stack
- Opcode: `0x0103`
- Mnemonic: `pop`
- Arguments: `pop <SIZE> <destination>`
	- `SIZE`: The size of the value to move.
	- `<data>`: An addressing block containing the destination for the popped value.
- Description: Depending on the value of `SIZE`, pops a `BYTE`, `WORD`, `DWORD`, or `QWORD` value off the top of the stack, writing it to the specified destination then subtracting the size from `ESP`.
- Errors:
	- `AddressOutOfRange`: Raised if the given address points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.
	- `StackUnderflow`: Raised if `ESP` is less than `EBP` after the pop.
	- `InvalidDestination`: Raised if the destination cannot receive a value; for example, if the destination is a numeric literal.

### Read Array Value
- Opcode: `0x0104`
- Mnemonic: `arrayread`
- Arguments: `arrayread SIZE <index>`
	- `SIZE`: The size of the value to read.
	- `<index>`: A four-byte unsigned integer that is the index of the value to read.
- Description: Given a pointer to an array at the top of the stack, `arrayread` reads an array element pointed to by the index, popping the array pointer and pushing the resulting value. The index is zero-based.
- Errors:
	- `AddressOutOfRange`: Raised if the address calculated from the size and index points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device.
	- `StackUnderflow`: Raised if `ESP` is less than `EBP` after the array's start address is popped off the stack.
	- `StackOverflow`: Raised if, after pushing the read value onto the stack, `ESP` points to an invalid address.

### Write Array Value
- Opcode: `0x0105`
- Mnemonic: `arraywrite`
- Arguments: `arraywrite SIZE <index> <data>`
	- `SIZE`: The size of the value to write.
	- `<index>`: A four-byte unsigned integer that is the index of the value to write.
	- `<data>`: The data, or a pointer to it, to write to the array
- Description: Given a pointer to an array at the top of the stack, `arraywrite` writes the given data to the specified index. The array pointer is popped off the top of the stock. The index is zero-based.
- Errors:
	- `AddressOutOfRange`: Raised if the address calculated from the size and index points to a memory location beyond the bounds of the memory space or not mapped to any memory space or hardware device. Also raised if `<data>` doesn't point to a valid address.
	- `StackUnderflow`: Raised if `ESP` is less than `EBP` after the array's start address is popped off the stack.
	
## Create Context

- Opcode: `0x0106`
- Mnemonic: `ctxcreate`
- Arguments: `ctxcreate MEM_SIZE`
	- `MEM_SIZE`: The amount of memory to allocate for the new context. Implicitly QWORD.
- Description: Creates a new context with `MEM_SIZE` bytes of memory. The context is given the next available ID. If the last used context ID is `0xFFFFFFFF`, the next ID is `0x00000002`, skipping 0. If `0x00000002` happens to be in use, a VM error is raised. The context's new ID is pushed onto the stack and its saved set of registers are all initialized to 0, except for ECC.
- Errors:
  - Raises `MaybeJustKeepItToTheLowHundredMillionsOfContexts` if the new context's ID is already in use
  - Raises `UnauthorizedContextOperation` if `ECC` is not 0 (that is, we're not in the kernel context)
  
## Destroy Context

- Opcode: `0x0107`
- Mnemonic: `ctxdestroy`
- Arguments: `ctxdestroy CTX_ID`
	- `CTX_ID`: The ID of the context to destroy. Implicitly DWORD.
- Description: Destroys the context with ID `CTX_ID` and frees its memory block.
- Errors:
  - Raises `NoSuchContext` if the context ID is not in use.
  - Raises `ProtectedContext` if the context ID is 0 or 1.
  - Raises `UnauthorizedContextOperation` if `ECC` is not 0 (that is, we're not in the kernel context)
  
## Context Switch

- Opcode: `0x0108`
- Mnemonic: `ctxswitch`
- Arguments: `ctxswitch CTX_ID`
	- `CTX_ID`: The ID of the context to switch to. Implicitly DWORD.
- Description: Changes `ECC` to `CTX_ID` and loads saved registers for that context. Execution immediately continues in the new context.
- Errors:
  - Raises `NoSuchContext` if the context ID is not in use.
  - Raises `CannotSwitchToHardwareContext` if `CTX_ID` is 1.

## Set Destination Context

- Opcode: `0x0109`
- Mnemomic: `ctxdest`
- Arguments: `ctxdest CTX_ID`
	- `CTX_ID`: The ID of the context that should be the destination of memory moves. Implicitly DWORD.
- Description: Specifies which context that any further `ctxmov` instructions should move memory to until the next `ctxdest` instruction is executed.
- Errors:
	- Raises `NoSuchContext` if the context ID is not in use.

## Move Memory to Context

- Opcode: `0x010A`
- Mnemonic: `ctxmov`
- Arguments: `ctxmov CA_SRC CB_DST COUNT`
	- `CA_SRC`: The address of the first byte in the current context to move memory from. Implicitly QWORD.
	- `CB_DST`: The address of the first byte in the destination context to move memory to. Implicitly QWORD.
	- `COUNT`: The number of bytes to move. Implicitly DWORD.
- Description: Copies `COUNT` bytes from `CA_SRC` in the current context (indicated by `ECC`) to `CB_DST` in the destination context (as set by `ctxdest`).
- Errors:
	- Raises `AddressOutOfRange` if either `CA_SRC`, `CB_DST`, `CA_SRC + COUNT`, or `CB_DST + COUNT` are out of range of the context's memory space. This also occurs if `CA_SRC`, `CB_DST`, `CA_SRC + COUNT`, or `CB_DST + COUNT` are inside of memory that has 

## Integral/Bitwise Operations
### Stack Addition
- Opcode: `0x0200`
- Mnemonic: `add`
- Arguments: `add SIZE`
	- `SIZE`: The size of the operands to add.
- Description: Pops the top two values off the stack, adds them together, and pushes their sum onto the stack.
- Errors:
	- `ArithmeticOverflow`: Raised if the sum is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the sum is lower than the minimum value of the datatype.
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Subtraction
- Opcode: `0x0201`
- Mnemonic: `sub`
- Arguments: `sub SIZE`
	- `SIZE`: The size of the operands to subtract.
- Description: Pops the top two values off the stack, subtracts them, and pushes their difference onto the stack.
- Errors:
	- `ArithmeticOverflow`: Raised if the difference is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the difference is lower than the minimum value of the datatype.
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Multiplication
- Opcode: `0x0202`
- Mnemonic: `mult`
- Arguments: `mult SIZE`
	- `SIZE`: The size of the operands to multiply.
- Description: Pops the top two values off the stack, multiplies them, and pushes their product onto the stack.
- Errors:
	- `ArithmeticOverflow`: Raised if the product is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the product is lower than the minimum value of the datatype.
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Division
- Opcode: `0x0203`
- Mnemonic: `div`
- Arguments: `div SIZE`
	- `SIZE`: The size of the operands to divide.
- Description: Pops the top two values off the stack, divides them, and pushes their quotient onto the stack.
- Errors:
	- `ArithmeticOverflow`: Raised if the quotient is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the quotient is lower than the minimum value of the datatype.
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Modulus Division
- Opcode: `0x0204`
- Mnemonic: `mod`
- Arguments: `mod SIZE`
	- `SIZE`: The size of the operands to divide.
- Description: Pops the top two values off the stack, divides them, and pushes the remainder onto the stack.
- Errors:
	- `ArithmeticOverflow`: Raised if the remainder is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the remainder is lower than the minimum value of the datatype.
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Increment
- Opcode: `0x0205`
- Mnemonic: `inc`
- Arguments: `inc SIZE`
	- `SIZE`: The size of the operand to increment.
- Description: Pops the top value off the stack, increments it, and pushes it back onto the stack.
- Errors:
	- `ArithmeticOverflow`: Raised if the value is higher than the maximum value of the datatype.
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Decrement
- Opcode: `0x0206`
- Mnemonic: `dec`
- Arguments: `dec SIZE`
	- `SIZE`: The size of the operand to decrement.
- Description: Pops the top value off the stack, decrements it, and pushes it back onto the stack.
- Errors:
	- `ArithmeticUnderflow`: Raised if the value is lower than the maximum value of the datatype.
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Bitwise AND
- Opcode: `0x0207`
- Mnemonic: `bwand`
- Arguments: `bwand SIZE`
	- `SIZE`: The size of the operands to AND.
- Description: Pops the top two values off the stack, performs a bitwise AND on them, and pushes the result onto the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Bitwise OR
- Opcode: `0x0208`
- Mnemonic: `bwor`
- Arguments: `bwor SIZE`
	- `SIZE`: The size of the operands to OR.
- Description: Pops the top two values off the stack, performs a bitwise OR on them, and pushes the result onto the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Bitwise XOR
- Opcode: `0x0209`
- Mnemonic: `bwxor`
- Arguments: `bwxor SIZE`
	- `SIZE`: The size of the operands to XOR.
- Description: Pops the top two values off the stack, performs a bitwise XOR on them, and pushes the result onto the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Bitwise NOT
- Opcode: `0x020A`
- Mnemonic: `bwnot`
- Arguments: `bwnot SIZE`
	- `SIZE`: The size of the operands to NOT.
- Description: Pops the top value off the stack, performs a bitwise NOT on it, and pushes the result onto the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Bitwise Shift Left
- Opcode: `0x020B`
- Mnemonic: `lshift`
- Arguments: `lshift SIZE`
	- `SIZE`: The size of the operands to shift.
- Description: Pops the top two values off the stack, shifts left the second value by the number of bits specified in the first value, and pushes the result on the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Bitwise Shift Right
- Opcode: `0x020C`
- Mnemonic: `rshift`
- Arguments: `rshift SIZE`
	- `SIZE`: The size of the operands to shift.
- Description: Pops the top two values off the stack, shifts right the second value by the number of bits specified in the first value, and pushes the result on the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Logical AND
- Opcode: `0x020D`
- Mnemonic: `land`
- Arguments: `land SIZE`
	- `SIZE`: The size of the operands to AND.
- Description: Pops the top two values off the stack. If they are both non-zero, the value 1 is pushed onto the stack. Else, the value of 0 is pushed onto the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Logical OR
- Opcode: `0x020E`
- Mnemonic: `lor`
- Arguments: `lor SIZE`
	- `SIZE`: The size of the operands to OR.
- Description: Pops the top two values off the stack. If either or both of them are non-zero, the value 1 is pushed onto the stack. Else, the value of 0 is pushed onto the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Logical XOR
- Opcode: `0x020F`
- Mnemonic: `lxor`
- Arguments: `lxor SIZE`
	- `SIZE`: The size of the operands to XOR.
- Description: Pops the top two values off the stack. If either (but not both) of them are non-zero, the value 1 is pushed onto the stack. Else, the value of 0 is pushed onto the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Logical NOT
- Opcode: `0x0210`
- Mnemonic: `lnot`
- Arguments: `lnot SIZE`
	- `SIZE`: The size of the operands to NOT.
- Description: Pops the top value off the stack. If it's nonzero, the value of 0 is pushed onto the stack. Else, the value of 1 is pushed onto the stack.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Stack Comparison
- Opcode: `0x0211`
- Mnemonic: `cmp`
- Arguments: `cmp SIZE`
	- `SIZE`: The size of the operands to compare.
- Description: Pops the top two values off the stack and compares them, setting the equals, less-than, and greater-than flags in EFLAGS accordingly.
- Errors:
	- `StackUnderflow`: Raised if popping one or both values causes `ESP` to be less than `EBP`.

### Long Addition
- Opcode: `0x0212`
- Mnemonic: `addl`
- Arguments: `addl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to add.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the sum.
- Description: Using the two source address, two values are added and their sum is stored in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.
	- `ArithmeticOverflow`: Raised if the sum is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the sum is lower than the minimum value of the datatype.

### Long Subtraction
- Opcode: `0x0213`
- Mnemonic: `subl`
- Arguments: `subl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to subtract.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the difference.
- Description: Using the two source address, two values are subtracted and their difference is stored in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.
	- `ArithmeticOverflow`: Raised if the difference is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the difference is lower than the minimum value of the datatype.

### Long Multiplication
- Opcode: `0x0214`
- Mnemonic: `multl`
- Arguments: `multl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to multiply.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the product.
- Description: Using the two source address, two values are multiplied and their product is stored in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.
	- `ArithmeticOverflow`: Raised if the product is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the product is lower than the minimum value of the datatype.

### Long Division
- Opcode: `0x0215`
- Mnemonic: `divl`
- Arguments: `divl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to divide.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the quotient.
- Description: Using the two source address, two values are divided and their quotient is stored in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.
	- `ArithmeticOverflow`: Raised if the quotient is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the quotient is lower than the minimum value of the datatype.

### Long Modulus Division
- Opcode: `0x0216`
- Mnemonic: `modl`
- Arguments: `modl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to divide.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the remainder.
- Description: Using the two source address, two values are divided and their remainder is stored in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.
	- `ArithmeticOverflow`: Raised if the remainder is higher than the maximum value of the datatype.
	- `ArithmeticUnderflow`: Raised if the remainder is lower than the minimum value of the datatype.

### Long Increment
- Opcode: `0x0217`
- Mnemonic: `incl`
- Arguments: `incl SIZE <source-address>`
	- `SIZE`: The size of the operand to increment.
	- `<source-address>`: An addressing block containing the operand.
- Description: The value at the source address is incremented.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.
	- `ArithmeticOverflow`: Raised if the value is higher than the maximum value of the datatype.

### Long Decrement
- Opcode: `0x0218`
- Mnemonic: `decl`
- Arguments: `decl SIZE <source-address>`
	- `SIZE`: The size of the operand to decrement.
	- `<source-address>`: An addressing block containing the operand.
- Description: The value at the source address is decremented.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.
	- `ArithmeticUnderflow`: Raised if the value is lower than the minimum value of the datatype.

### Long Bitwise AND
- Opcode: `0x0219`
- Mnemonic: `bwandl`
- Arguments: `bwandl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to AND.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the result.
- Description: Performs a bitwise AND on the values at the two source addresses and stores the result in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Bitwise OR
- Opcode: `0x021A`
- Mnemonic: `bworl`
- Arguments: `bworl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to OR.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the result.
- Description: Performs a bitwise OR on the values at the two source addresses and stores the result in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Bitwise XOR
- Opcode: `0x021B`
- Mnemonic: `bwxorl`
- Arguments: `bwxorl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to XOR.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the result.
- Description: Performs a bitwise XOR on the values at the two source addresses and stores the result in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Bitwise NOT
- Opcode: `0x021C`
- Mnemonic: `bwnotl`
- Arguments: `bwnotl SIZE <source-address> <dest-address>`
	- `SIZE`: The size of the operands to NOT.
	- `<source-address>`: An addressing block containing the operand.
	- `<dest-address>`: An addressing block containing the destination.
- Description: Performs a bitwise NOT on the value at the source address and stores the result in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Bitwise Shift Left
- Opcode: `0x021D`
- Mnemonic: `lshiftl`
- Arguments: `lshiftl SIZE <operand-address> <positions-address> <dest-address>`
	- `SIZE`: The size of the operands to shift.
	- `<operand-address>`: An addressing block containing the operand.
	- `<positions-address>`: An addressing block containing the number of positions to shift the value by.
	- `<dest-address>`: An addressing block containing the destination.
- Description: Shifts left the value at the operand's address by the number of specified positions and stores the result in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Bitwise Shift Right
- Opcode: `0x021E`
- Mnemonic: `rshiftl`
- Arguments: `rshiftl SIZE <operand-address> <positions-address> <dest-address>`
	- `SIZE`: The size of the operands to shift.
	- `<operand-address>`: An addressing block containing the operand.
	- `<positions-address>`: An addressing block containing the number of positions to shift the value by.
	- `<dest-address>`: An addressing block containing the destination.
- Description: Shifts right the value at the operand's address by the number of specified positions and stores the result in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Logical AND
- Opcode: `0x021F`
- Mnemonic: `landl`
- Arguments: `landl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to AND.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the result.
- Description: If both of the source values are non-zero, the value of 1 is placed in the destination. Else, the value of 0 is placed in the destination.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Logical OR
- Opcode: `0x0220`
- Mnemonic: `lorl`
- Arguments: `lorl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to OR.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the result.
- Description: If either or both of the source values are non-zero, the value of 1 is placed in the destination. Else, the value of 0 is placed in the destination.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Logical XOR
- Opcode: `0x0221`
- Mnemonic: `lxorl`
- Arguments: `lxorl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to XOR.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the result.
- Description: If either (but not both) of the source values are non-zero, the value of 1 is placed in the destination. Else, the value of 0 is placed in the destination.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Logical NOT
- Opcode: `0x0222`
- Mnemonic: `lnotl`
- Arguments: `lnotl SIZE <source-address> <dest-address>`
	- `SIZE`: The size of the operands to NOT.
	- `<source-address>`: An addressing block containing the operand.
	- `<dest-address>`: An addressing block containing the destination.
- Description: If the source value is non-zero, the value of 0 is placed in the destination. Else, the value of 1 is placed in the destination.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

### Long Comparison
- Opcode: `0x0223`
- Mnemonic: `cmpl`
- Arguments: `cmpl SIZE <source1-address> <source2-address> <dest-address>`
	- `SIZE`: The size of the operands to compare.
	- `<source1-address>`: An addressing block containing the first operand.
	- `<source2-address>`: An addressing block containing the second operand.
	- `<dest-address>`: The addressing block containing the destination of the result.
- Description: Compares the two source values and stores either -1, 0, or +1 in the destination address.
- Errors:
	- `AddressOutOfRange`: Raised if either the source addresses or the destination address is not valid.

## Floating Point Stack Operations
### Floating Stack Addition
- Opcode: 0x0280
- Mnemonic: `fadd`
- Arguments: `fadd SIZE`
	- `SIZE`: The size of the operands to add, either `DWORD` (`float32`) or `QWORD` (`float64`).
- Description: Pops two floating point values off the top of the stack, adds them, and pushes the sum.
- Errors:
	`InvalidFloatingSize`: The size of the operand was `BYTE` or `WORD`.
	`StackUnderflow`: Popping the top two values off the stack caused `ESP` to be less than `EBP`.

### Floating Stack Subtraction
- Opcode: 0x0281
- Mnemonic: `fsub`
- Arguments: `fsub SIZE`
	- `SIZE`: The size of the operands to subtract, either `DWORD` (`float32`) or `QWORD` (`float64`).
- Description: Pops two floating point values off the top of the stack, subtracts them, and pushes the difference.
- Errors:
	`InvalidFloatingSize`: The size of the operand was `BYTE` or `WORD`.
	`StackUnderflow`: Popping the top two values off the stack caused `ESP` to be less than `EBP`.

### Floating Stack Multiplication
- Opcode: 0x0282
- Mnemonic: `fmult`
- Arguments: `fmult SIZE`
	- `SIZE`: The size of the operands to multiply, either `DWORD` (`float32`) or `QWORD` (`float64`).
- Description: Pops two floating point values off the top of the stack, multiplies them, and pushes the product.
- Errors:
	`InvalidFloatingSize`: The size of the operand was `BYTE` or `WORD`.
	`StackUnderflow`: Popping the top two values off the stack caused `ESP` to be less than `EBP`.

### Floating Stack Division
- Opcode: 0x0283
- Mnemonic: `fdiv`
- Arguments: `fdiv SIZE`
	- `SIZE`: The size of the operands to divide, either `DWORD` (`float32`) or `QWORD` (`float64`).
- Description: Pops two floating point values off the top of the stack, divides them, and pushes the quotient.
- Errors:
	`InvalidFloatingSize`: The size of the operand was `BYTE` or `WORD`.
	`StackUnderflow`: Popping the top two values off the stack caused `ESP` to be less than `EBP`.

### Floating Stack Modulus
- Opcode: 0x0284
- Mnemonic: `fmod`
- Arguments: `fmod SIZE`
	- `SIZE`: The size of the operands to add, either `DWORD` (`float32`) or `QWORD` (`float64`).
- Description: Pops two floating point values off the top of the stack, divides them, and pushes the remainder.
- Errors:
	`InvalidFloatingSize`: The size of the operand was `BYTE` or `WORD`.
	`StackUnderflow`: Popping the top two values off the stack caused `ESP` to be less than `EBP`.

### Floating Stack Comparison
- Opcode: 0x0285
- Mnemonic: `fcmp`
- Arguments: `fcmp SIZE`
	- `SIZE`: The size of the operands to add, either `DWORD` (`float32`) or `QWORD` (`float64`).
- Description: Pops two floating point values off the top of the stack, compares them, and then sets the according flags on `EFLAGS`.
- Errors:
	`InvalidFloatingSize`: The size of the operand was `BYTE` or `WORD`.
	`StackUnderflow`: Popping the top two values off the stack caused `ESP` to be less than `EBP`.

### Floating Stack Square Root
- Opcode: 0x0286
- Mnemonic: `fsqrt`
- Arguments: `fsqrt SIZE`
	- `SIZE`: The size of the operands to add, either `DWORD` (`float32`) or `QWORD` (`float64`).
- Description: Pops two floating point values the top value off the stack, takes its square root, and pushes it back on the stack.
- Errors:
	`InvalidFloatingSize`: The size of the operand was `BYTE` or `WORD`.
	`StackUnderflow`: Popping the top value off the stack caused `ESP` to be less than `EBP`.
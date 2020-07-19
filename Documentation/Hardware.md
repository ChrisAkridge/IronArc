# IronArc Hardware Devices

All IronArc implementations should include the following devices:

## System

### Hardware Calls

The `System` device has the following hardware calls:

#### RegisterInterruptHandler

```c
uint8 hwcall System::RegisterInterruptHandler(uint32 deviceIndex, lpstring* interruptName, ptr handlerAddress)
```

Registers a pointer that will be jumped to when a hardware device with a given index raises an interrupt. Up to 256 handlers can be registered on the same interrupt by calling the function multiple times with different addresses. Each handler will be given an index, starting at 0 for the first handler, up to 255. If the index of the last assigned handler was 255, another handler can only be added if other handlers have been unregistered, thereby freeing their indices - these handlers will get the first freed index, in order. If there are no freed indices, a hardware error indicating that no handler index was available will be raised.

- Parameters:
	- `uint32 deviceIndex`: The index of the hardware device to handle interrupts for.
	- `lpstring* interruptName`: A pointer to a string containing the name of the interrupt to handle.
	- `ptr handlerAddress`: The pointer to jump to when the interrupt occurs.
- Return value: A byte equal to the count of handlers registered for this interrupt on this device after the registration.
- Errors:
	- If the device index isn't used by any device
	- If the interrupt name isn't one of the interrupts the device raises
	- If there wasn't an index available for a handler

#### UnregisterInterruptHandler

```c
void hwcall System::UnregisterInterruptHandler(uint32 deviceIndex, lpstring* interruptName, uint8 handlerIndex)
```

Unregisters a handler for a given interrupt on a given hardware device with a given index, freeing the index for registering again later.

- Parameters:
	- `uint32 deviceIndex`: The index of the hardware device to remove the handler on.
	- `lpstring* interruptName`: A pointer to a string containing the name of the interrupt to remove the handler on.
	- `uint8 handlerIndex`: The index of the handler to unregister.
- Errors:
	- If the device index isn't used by any device
	- If the interrupt name isn't one of the interrupts the device raises
	- If the handler index isn't used by any interrupt handler

#### RaiseError

```c
void hwcall System::RaiseError(uint32 errorCode)
```

Raises an error, given its error code.

- Parameters:
	- `uint32 errorCode`: The code of the error to raise.	

#### RegisterErrorHandler

```c
void hwcall System::RegisterErrorHandler(uint32 errorCode, ptr handlerAddress)
```

Registers a pointer that will be jumped to when a given error code is raised.

- Parameters:
	- `uint32 errorCode`: The error code that, when raised, will cause the VM to jump to `handlerAddress`.
	- `ptr handlerAddress`: The pointer to jump to when this error is raised.

#### UnregisterErrorHandler

```c
void hwcall System::UnregisterErrorHandler(uint32 errorCode)
```

Unregisters the pointer that would be jumped to when a given error code is raised.

- Parameters:
	- `uint32 errorCode`: The error code to unregister the handler for.
- Errors:
	- If the error code did not already have a handler registered for it.
	
#### GetLastErrorDescriptionSize

```c
uint64 hwcall System::GetLastErrorDescriptionSize()
```

- Return value: The size, in bytes, that will be written by a call to `System::GetLastErrorDescription`.

#### GetLastErrorDescription

```c
void hwcall System::GetLastErrorDescription(ptr destination)
```

Writes the description of the last raised error, in the form of the following structure:
```c
struct ErrorDescription
{
	lpstring* MessagePointer;
	uint32 ErrorCode;
	lpstring Message;
}
```

All fields are stored contiguously. `MessagePointer` always points to `Message`.

- Parameters:
	- `ptr destination`: The address to write the description to.

#### GetHardwareDeviceCount

```c
int32 hwcall System::GetHardwareDeviceCount()
```

- Return value: The number of hardware devices attached to the VM.

#### GetHardwareDeviceDescriptionSize

```c
uint64 hwcall System::GetHardwareDeviceDescriptionSize(int32 deviceIndex)
```

- Return value: The size, in bytes, that will be written by a call to `System::GetHardwareDeviceDescription`.

#### GetHardwareDeviceDescription

```c
void hwcall System::GetHardwareDeviceDescription(ptr destination)
```

Writes the description of a single hardware device, in the form of the following structure:
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

All fields are contiguous in memory. `MemoryStart` and `MemoryEnd` are both `0` if the device maps no memory. `NamePointer` always points to `Name`.

- Parameters:
	- `ptr destination`: The address to write the description to.

#### GetAllHardwareDeviceDescriptionSize

```c
uint64 hwcall System::GetAllHardwareDeviceDescriptionsSize()
```

- Return value: The size, in bytes, that will be written by a call to `System::GetAllHardwareDeviceDescriptions`.

#### GetAllHardwareDeviceDescriptions

```c
void hwcall System::GetAllHardwareDeviceDescriptions(ptr destination)
```

Writes the descriptions of all hardware devices, in the form of the following structure:

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

All fields are contiguous in memory.

- Parameters:
	- `ptr destination`: The address to write the descriptions to.

#### CreatePageTable

```c
uint32 hwcall System::CreatePageTable()
```

Creates a new page table and returns its page table ID.

- Return value: The page table ID of the newly created page table.

#### DestroyPageTable

```c
void hwcall System::DestroyPageTable(uint32 pageTableId)
```

Destroys a page table, given its ID.

- Parameters:
	- `uint32 pageTableId`: The ID of the page table to destroy.
- Errors:
	- If the page table to destroy is also the current page table.

#### ChangeCurrentPageTable

```c
void hwcall System::ChangeCurrentPageTable(uint32 pageTableId)
```

Changes the current page table to the one with the given ID.

- Parameters:
	- `uint32 pageTableId`: The ID of the page table to changes to.
- Errors:
	- If no page table has that ID.
	
### Interrupts

### HardwareDeviceAttached

```c
void interrupt System::HardwareDeviceAttached(uint32 deviceId)
```

Raised when a hardware device is attached.

- Parameters:
	- `uint32 deviceId`: The device ID of the newly attached deviced.

### HardwareDeviceRemoved

```c
void interrupt System::HardwareDeviceRemoved(uint32 deviceId)
```

Raised when a hardware device is removed.

- Parameters:
	- `uint32 deviceId`: The device ID of the newly removed deviced.

## Terminal

### Hardware Calls

#### Write

```c
void hwcall Terminal::Write(lpstring* text)
```

Writes the string at the pointer to the terminal.

- Parameters:
	- `lpstring* text`: A pointer to the text to write.

#### WriteLine

```c
void hwcall Terminal::WriteLine(lpstring* text)
```

Writes the string at the pointer, followed by a line break, to the terminal.

- Parameters:
	- `lpstring* text`: A pointer to the text to write.

#### Read

```c
uint16 hwcall Terminal::Read()
```

Reads one UTF-16 character from the terminal.

- Return value: The character that was read.

#### ReadLine

```c
void hwcall Terminal::ReadLine(ptr destination)
```

Reads a line from the terminal, then writes it to the destination pointer.

- Parameters:
	- `ptr destination`: The pointer to write the string to.

## Debugger

### Hardware Calls

#### Break

```c
void hwcall Debugger:Break()
```

If a debugger is attached to the IronArc host process, calling this hardware call causes the debugger to break immediately.
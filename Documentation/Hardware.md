# IronArc Hardware Devices

All IronArc implementations should include the following devices:

## System

The `System` device has the following hardware calls:

### RegisterInterruptHandler

```c
int8 hwcall System::RegisterInterruptHandler(uint32 instanceId, lpstring* interruptName, ptr handlerAddress)
```

(Up to 256 handlers can be registered on the same interrupt by calling the function multiple times with different addresses. Each handler will be given an index, starting at 0 for the first handler, up to 255.)

### UnregisterInterruptHandler

```c
void hwcall System::UnregisterInterruptHandler(uint32 hardwareNumber, lpstring* interruptName, uint8 handlerIndex)
```

### RaiseError

```c
void hwcall System::RaiseError(uint32 errorCode)
```

### RegisterErrorHandler

```c
void hwcall System::RegisterErrorHandler(uint32 errorCode, ptr handlerAddress)
```

### UnregisterErrorHandler

```c
void hwcall System::UnregisterErrorHandler(uint32 errorCode)
```

### GetHardwareDeviceCount

```c
int32 hwcall System::GetHardwareDeviceCount()
```

### GetHardwareDeviceDescriptionSize

```c
uint64 hwcall System::GetHardwareDeviceDescriptionSize(int32 deviceIndex)
```

### GetHardwareDeviceDescription

```c
void hwcall System::GetHardwareDeviceDescription(ptr destination)
```

### GetAllHardwareDeviceDescriptionSize

```c
uint64 hwcall System::GetAllHardwareDeviceDescriptionsSize()
```

### GetAllHardwareDeviceDescriptions

```c
void hwcall System::GetAllHardwareDeviceDescriptions(ptr destination)
```

### CreatePageTable

```c
uint32 hwcall System::CreatePageTable()
```

### DestroyPageTable

```c
void hwcall System::DestroyPageTable(uint32 pageTableId)
```

### ChangeCurrentPageTable

```c
void hwcall System::ChangeCurrentPageTable(uint32 pageTableId)
```

## Terminal

### Write

### WriteLine

### Read

### ReadLine

## Debugger

### Break
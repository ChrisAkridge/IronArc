# `StorageDevice`

The `StorageDevice` hardware device loads a file on the host, given its path. A single VM can run only one `StorageDevice` instance and can only hold open one file. Another VM on the same host is not allowed to open a file if it's already opened on another VM.

The device then provides access to the file for the VM through a concept called streams which provide views into a portion of the file. A stream consists of the following properties:

- Stream ID: An unsigned 32-bit integer that identifies the stream.
- Starting Position: The offset from the beginning of the file that the stream starts at.
- Ending Position: The offset from the beginning of the file that the stream ends at. Must be equal to or greater than the Starting Position.
- Length: The length of the stream in bytes, calculated by Ending Position - Starting Position.
- Position: An index with its 0 point at the Starting Position and is strictly less than the Length. This property can be set manually through a hardware call and is advanced through hardware calls that read from or write to the stream.

Multiple streams can be opened at the same time and can overlap. All read and write calls are blocking and only one can occur at a time due to the lack of multithreading. While two streams cannot simultaneously overwrite the same memory on different threads, a stream overlapping with another may see the contents change unexpectedly if the other stream writes into it.

## Stream IDs 

Stream IDs are assigned when a stream is opened. IDs start at 0 and increment by 1 for each opened stream, wrapping back around to 0 when the ID reaches 4,294,967,295. If the next stream ID is still in use by an earlier stream, a HardwareError is raised.

## Hardware Calls

### `OpenStream`

```c
uint32 hwcall StorageDevice::OpenStream(uint64 startPosition, uint64 length)
```

Opens a stream into the file and returns the allocated ID.

- Parameters:
	- `uint64 startPosition`: A position into the file. Must be less than the file's length.
	- `uint64 length`: The desired length of the stream. `startPosition + length` must be less than the file's length.
- Return value: The ID of the opened stream.

### `CloseStream`

```c
void hwcall StorageDevice::CloseStream(uint32 streamID)
```

Opens a stream into the file and returns the allocated ID.

- Parameters:
	- `uint32 streamID`: The ID of the stream to close. Must be an ID of an open stream.

### `GetStreamLength`

```c
uint64 hwcall StorageDevice::GetStreamLength(uint32 streamID)
```

Returns the length of a stream.

- Parameters:
	- `uint32 streamID`: The ID of the stream to get the length for. Must be an ID of an open stream.

### `Seek`

```c
void hwcall StorageDevice::Seek(uint32 streamID, uint64 newPosition)
```

Returns the length of a stream.

- Parameters:
	- `uint32 streamID`: The ID of the stream to seek. Must be an ID of an open stream.
	- `uint64 newPosition`: The position to seek to. Must be less than the stream's length.


### `Read`

```c
void hwcall StorageDevice::Read(uint32 streamId, ptr destination, uint64 length)
```

Reads `length` bytes from the stream at its current position and advances its position by `length` bytes. The bytes are read into memory at `destination`.

- Parameters:
	- `uint32 streamID`: The ID of the stream to read from. Must be an ID of an open stream.
	- `ptr destination`: The location in memory to read into. `destination + length` must be within the bounds of memory in the current context.
	- `uint64 length`: The number of bytes to read. `length` plus the stream's position must be less than the stream's length.

### `Write`

```c
void hwcall StorageDevice::Write(uint32 streamId, ptr source, uint64 length)
```

Writes `length` bytes from the current context's memory at `source` into the stream at its current position and advances the position by `length` bytes.

- Parameters:
	- `uint32 streamID`: The ID of the stream to read from. Must be an ID of an open stream.
	- `ptr destination`: The location in memory to read into. `source + length` must be within the bounds of memory in the current context.
	- `uint64 length`: The number of bytes to write. `length` plus the stream's position must be less than the stream's length.
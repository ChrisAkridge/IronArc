# `Terminal`

## Hardware Calls

### Write

```c
void hwcall Terminal::Write(lpstring* text)
```

Writes the string at the pointer to the terminal.

- Parameters:
	- `lpstring* text`: A pointer to the text to write.

### WriteLine

```c
void hwcall Terminal::WriteLine(lpstring* text)
```

Writes the string at the pointer, followed by a line break, to the terminal.

- Parameters:
	- `lpstring* text`: A pointer to the text to write.

### Read

```c
uint16 hwcall Terminal::Read()
```

Reads one UTF-16 character from the terminal.

- Return value: The character that was read.

### ReadLine

```c
void hwcall Terminal::ReadLine(ptr destination)
```

Reads a line from the terminal, then writes it to the destination pointer.

- Parameters:
	- `ptr destination`: The pointer to write the string to.
# Debugger

## Hardware Calls

### Break

```c
void hwcall Debugger:Break()
```

If a debugger is attached to the IronArc host process, calling this hardware call causes the debugger to break immediately.
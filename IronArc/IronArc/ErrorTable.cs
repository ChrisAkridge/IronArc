using System;
using System.Linq;
using System.Text;

namespace IronArc
{
    public enum Error : uint
    {
        GeneralError,
        InvalidAddressType,
        AddressOutOfRange,
        CallStackUnderflow,
        NotImplemented,
        HardwareDeviceNotPresent,
        InvalidDestinationType,
        HeaderInvalid,
        StringIndexOutOfRange,
        StackUnderflow,
        HardwareError,
        RegisterReadOnly,
        UnauthorizedContextOperation,
        MaybeJustKeepItToTheLowHundredMillionsOfContexts,
        ProtectedContext,
        NoSuchContext,
        CannotSwitchToHardwareContext
    }

    public sealed class ErrorDescription
    {
        public Error Error { get; }
        public string Message { get; }
        public byte[] MessageUtf8 { get; }

        public ErrorDescription(Error error, string message)
        {
            Error = error;
            Message = message;
            MessageUtf8 = Encoding.UTF8.GetBytes(Message);
        }

        public ulong GetErrorDescriptionSize()
        {
            const ulong PointerSize = 8UL;
            const ulong ErrorCodeSize = 4UL;
            const ulong StringLengthSize = 4UL;

            return PointerSize + ErrorCodeSize + StringLengthSize + (ulong)MessageUtf8.Length;
        }

        public byte[] GetErrorDescription(ulong address)
        {
            var errorCodeBytes = BitConverter.GetBytes((uint)Error);
            var messageLengthBytes = BitConverter.GetBytes(MessageUtf8.Length);
            var messagePointerBytes = BitConverter.GetBytes(address + 8UL + (ulong)errorCodeBytes.Length);

            return messagePointerBytes.Concat(errorCodeBytes.Concat(messageLengthBytes.Concat(MessageUtf8))).ToArray();
        }
    }

    public static class ErrorMessages
    {
        public static string GetDefaultMessage(Error error)
        {
            switch (error)
            {
                case Error.GeneralError: return "An error has occurred.";
                case Error.InvalidAddressType: return "This address block cannot be used this way.";
                case Error.AddressOutOfRange: return "The address is beyond the range of system memory.";
                case Error.CallStackUnderflow:
                    return "Attempted to return from a call, but there was nothing on the call stack.";
                case Error.NotImplemented: return "This feature is not yet implemented.";
                case Error.HardwareDeviceNotPresent: return "This hardware device is not attached to the VM.";
                case Error.InvalidDestinationType: return "This value cannot be written here.";
                case Error.HeaderInvalid: return "The loaded program has an invalid header.";
                case Error.StringIndexOutOfRange: return "There is no string at the requested index.";
                case Error.StackUnderflow: return "The stack has popped more items than it has.";
                case Error.HardwareError: return "A hardware device has encountered an error.";
                case Error.RegisterReadOnly: return "This register is read-only and cannot be written to.";
                case Error.UnauthorizedContextOperation:
                    return "Cannot perform this operation from another context besides #0.";
                case Error.MaybeJustKeepItToTheLowHundredMillionsOfContexts:
                    return "Over 2.1 billion active contexts at the same time. Impressive. You do know you can destroy contexts, right?";
                case Error.ProtectedContext: return "This context does not support this operation.";
                case Error.NoSuchContext: return "No context has this ID.";
                case Error.CannotSwitchToHardwareContext:
                    return
                        "Cannot switch to the hardware context. Please use the CTXMOV instruction to read/write hardware memory.";
                default: return $"Attempted to raise non-existant error {(uint)error}. Congratulations.";
            }
        }

        public static string GetRegisterName(ulong registerIndex)
        {
            switch (registerIndex)
            {
                case 0UL: return "EAX";
                case 1UL: return "EBX";
                case 2UL: return "ECX";
                case 3UL: return "EDX";
                case 4UL: return "EEX";
                case 5UL: return "EFX";
                case 6UL: return "EGX";
                case 7UL: return "EHX";
                case 8UL: return "EBP";
                case 9UL: return "ESP";
                case 10UL: return "EIP";
                case 11UL: return "EFLAGS";
                case 12UL: return "ERP";
                case 13UL: return "ECC";
                default: return $"unknown register #{registerIndex}";
            }
        }
    }
}

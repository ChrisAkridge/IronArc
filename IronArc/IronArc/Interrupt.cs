namespace IronArc
{
    public sealed class Interrupt
    {
        public uint DeviceId { get; }
        public string InterruptName { get; }
        public object[] Arguments { get; }

        public Interrupt(uint deviceId, string interruptName, params object[] arguments)
        {
            DeviceId = deviceId;
            InterruptName = interruptName;
            Arguments = arguments;
        }
    }
}

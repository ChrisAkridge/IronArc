namespace IronArc
{
    public sealed class Interrupt
    {
        public uint DeviceId { get; }
        public string InterruptName { get; }

        public Interrupt(uint deviceId, string interruptName)
        {
            DeviceId = deviceId;
            InterruptName = interruptName;
        }
    }
}

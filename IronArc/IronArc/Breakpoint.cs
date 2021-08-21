namespace IronArc
{
    public sealed class Breakpoint
    {
        public ulong Address { get; }
        public bool IsUserVisible { get; }

        public Breakpoint(ulong address, bool isUserVisible)
        {
            Address = address;
            IsUserVisible = isUserVisible;
        }
    }
}

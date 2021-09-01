namespace IronArc
{
    public sealed class Breakpoint
    {
        public ulong Address { get; }
        public int Context { get; }
        public bool IsUserVisible { get; }

        public Breakpoint(ulong address, int context, bool isUserVisible)
        {
            Address = address;
            Context = context;
            IsUserVisible = isUserVisible;
        }
    }
}

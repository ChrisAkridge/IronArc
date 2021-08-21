using System.Collections.Concurrent;
using IronArc.Hardware;

namespace IronArc
{
    public interface ICoreHardwareProvider
    {
        // ReSharper disable once InconsistentNaming
        ConcurrentQueue<Message> UIMessageQueue { get; }

        ITerminal CreateTerminal();
        void DestroyTerminal(ITerminal terminal);
    }
}

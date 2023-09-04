using System.Collections.Concurrent;
using IronArc.Hardware;

namespace IronArc
{
    public interface ICoreHardwareProvider
    {
        // ReSharper disable once InconsistentNaming
        ConcurrentQueue<Message> UIMessageQueue { get; }

        ITerminal CreateTerminal();
        IDynamicTerminal CreateDynamicTerminal();
        void DestroyTerminal(ITerminal terminal);
        void DestroyDynamicTerminal(IDynamicTerminal dynamicTerminal);
    }
}

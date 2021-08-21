using System;

namespace IronArc.Hardware
{
    public interface ITerminal
    {
        Guid MachineId { get; set; }
        bool CanPerformWaitingRead { get; }

        void Write(string text);
        void WriteLine(string text);

        char Read();
        string ReadLine();

        char NonWaitingRead();
        string NonWaitingReadLine();
    }
}

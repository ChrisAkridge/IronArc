using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

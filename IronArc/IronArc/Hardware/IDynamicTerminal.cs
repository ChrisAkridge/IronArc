using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Hardware
{
	/// <summary>
	/// Represents a terminal that supports setting characters at specific positions.
	/// </summary>
	public interface IDynamicTerminal
	{
		Guid MachineId { get; set; }
		bool CanPerformWaitingRead { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }

		public int CursorX { get; set; }
		public int CursorY { get; set; }

		public void Write(string text);
		public void WriteLine(string text);

		public int ReadKey();
		public char ReadChar();
		public string ReadLine();

		public int NonWaitingReadKey();
		public char NonWaitingReadChar();
		public string NonWaitingReadLine();
	}
}

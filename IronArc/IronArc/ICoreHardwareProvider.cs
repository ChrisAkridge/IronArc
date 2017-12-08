using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronArc.Hardware;

namespace IronArc
{
	public interface ICoreHardwareProvider
	{
		ITerminal CreateTerminal();
	}
}

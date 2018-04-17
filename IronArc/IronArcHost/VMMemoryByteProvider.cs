using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Be.Windows.Forms;
using IronArc;

namespace IronArcHost
{
	internal sealed class VMMemoryByteProvider : IByteProvider
	{
		private DebugVM vm;

		public VMMemoryByteProvider(DebugVM vm)
		{
			this.vm = vm;
		}

		public long Length => vm.MemorySize;

		public event EventHandler LengthChanged;
		public event EventHandler Changed;

		public void OnChanged()
		{
			Changed?.Invoke(this, new EventArgs());
		}

		public void ApplyChanges()
		{
			throw new NotImplementedException();
		}

		public bool HasChanges()
		{
			throw new NotImplementedException();
		}

		public byte ReadByte(long index) => vm.ReadByte(index);

		public void WriteByte(long index, byte value)
		{
			vm.WriteByte(index, value);
			OnChanged();
		}

		public bool SupportsDeleteBytes() => false;
		public bool SupportsInsertBytes() => false;
		public bool SupportsWriteByte() => true;

		public void InsertBytes(long index, byte[] bs)
		{
			throw new InvalidOperationException("IronArc VM memory cannot have its length changed.");
		}

		public void DeleteBytes(long index, long length)
		{
			throw new InvalidOperationException("IronArc VM memory cannot have its length changed.");
		}
	}
}

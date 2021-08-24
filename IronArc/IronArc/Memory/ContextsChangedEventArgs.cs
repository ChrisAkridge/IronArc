using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Memory
{
    public sealed class ContextsChangedEventArgs : EventArgs
    {
        public int HighestContextID { get; set; }
        public List<int> DestroyedContextIDs { get; set; }
    }
}

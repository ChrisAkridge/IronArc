using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronArc.Memory
{
    public sealed class Context
    {
        public ByteBlock Memory { get; set; }
        
        public ulong EAX { get; set; }
        public ulong EBX { get; set; }
        public ulong ECX { get; set; }
        public ulong EDX { get; set; }
        public ulong EEX { get; set; }
        public ulong EFX { get; set; }
        public ulong EGX { get; set; }
        public ulong EHX { get; set; }
        
        public ulong EBP { get; set; }
        public ulong ESP { get; set; }
        public ulong EFLAGS { get; set; }
        public ulong ERP { get; set; }

        public void SaveRegisterSet(ulong eax, ulong ebx, ulong ecx, ulong edx, ulong eex,
            ulong efx, ulong egx, ulong ehx, ulong ebp, ulong esp,
            ulong eflags, ulong erp)
        {
            EAX = eax;
            EBX = ebx;
            ECX = ecx;
            EDX = edx;
            EEX = eex;
            EFX = efx;
            EGX = egx;
            EHX = ehx;
            EBP = ebp;
            ESP = esp;
            EFLAGS = eflags;
            ERP = erp;
        }

        public void LoadRegisterSet(out ulong eax, out ulong ebx, out ulong ecx, out ulong edx, out ulong eex,
            out ulong efx, out ulong egx, out ulong ehx, out ulong ebp, out ulong esp,
            out ulong eflags, out ulong erp)
        {
            eax = EAX;
            ebx = EBX;
            ecx = ECX;
            edx = EDX;
            eex = EEX;
            efx = EFX;
            egx = EGX;
            ehx = EHX;
            ebp = EBP;
            esp = ESP;
            eflags = EFLAGS;
            erp = ERP;
        }
    }
}

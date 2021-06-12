﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Be.Windows.Forms;
using IronArc.Memory;

namespace IronArcHost
{
    public sealed class MemoryManagerByteProvider : IByteProvider
    {
        private readonly MemoryManager manager;
        
        public uint ViewedPageTableId { get; set; }

        public MemoryManagerByteProvider(MemoryManager manager) => this.manager = manager;

        /// <summary>
        /// Reads a byte from the provider
        /// </summary>
        /// <param name="index">the index of the byte to read</param>
        /// <returns>the byte to read</returns>
        public byte ReadByte(long index) => manager.ReadByteInPageTable((ulong)index, ViewedPageTableId);

        /// <summary>
        /// Writes a byte into the provider
        /// </summary>
        /// <param name="index">the index of the byte to write</param>
        /// <param name="value">the byte to write</param>
        public void WriteByte(long index, byte value) =>
            manager.WriteByteInPageTable(value, (ulong)index, ViewedPageTableId);

        /// <summary>
        /// Inserts bytes into the provider
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bs"></param>
        /// <remarks>This method must raise the LengthChanged event.</remarks>
        public void InsertBytes(long index, byte[] bs) => throw new InvalidOperationException();

        /// <summary>
        /// Deletes bytes from the provider
        /// </summary>
        /// <param name="index">the start index of the bytes to delete</param>
        /// <param name="length">the length of the bytes to delete</param>
        /// <remarks>This method must raise the LengthChanged event.</remarks>
        public void DeleteBytes(long index, long length) => throw new InvalidOperationException();

        /// <summary>
        /// Returns the total length of bytes the byte provider is providing.
        /// </summary>
        public long Length => (long)manager.SystemMemoryLength;

        /// <summary>
        /// Occurs, when the Length property changed.
        /// </summary>
        public event EventHandler LengthChanged;

        /// <summary>
        /// True, when changes are done.
        /// </summary>
        public bool HasChanges() => throw new InvalidOperationException();

        /// <summary>
        /// Applies changes.
        /// </summary>
        public void ApplyChanges() => throw new InvalidOperationException();

        /// <summary>
        /// Occurs, when bytes are changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Returns a value if the WriteByte methods is supported by the provider.
        /// </summary>
        /// <returns>True, when it´s supported.</returns>
        public bool SupportsWriteByte() => true;

        /// <summary>
        /// Returns a value if the InsertBytes methods is supported by the provider.
        /// </summary>
        /// <returns>True, when it´s supported.</returns>
        public bool SupportsInsertBytes() => false;

        /// <summary>
        /// Returns a value if the DeleteBytes methods is supported by the provider.
        /// </summary>
        /// <returns>True, when it´s supported.</returns>
        public bool SupportsDeleteBytes() => false;
    }
}

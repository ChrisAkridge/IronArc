using System;

namespace IronArc
{
    public readonly struct InterruptHandlerKey : IEquatable<InterruptHandlerKey>
    {
        public uint DeviceId { get; }
        public string InterruptName { get; }

        public InterruptHandlerKey(uint deviceId, string interruptName)
        {
            DeviceId = deviceId;
            InterruptName = interruptName;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(InterruptHandlerKey other) =>
            DeviceId == other.DeviceId
            && string.Equals(InterruptName, other.InterruptName, StringComparison.OrdinalIgnoreCase);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj) => obj is InterruptHandlerKey other && Equals(other);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked { return ((int)DeviceId * 397) ^ (InterruptName != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(InterruptName) : 0); }
        }
    }
}

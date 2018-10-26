using System;

namespace Radical.ComponentModel.ChangeTracking
{
    /// <summary>
    /// Represents an atomic operation within the lifecycle
    /// of an <see cref="IChangeTrackingService"/>. An atomic
    /// operation can group a set of changes as a single change.
    /// </summary>
    public interface IAtomicOperation : IDisposable
    {
        /// <summary>
        /// Completes this atomic operation.
        /// </summary>
        void Complete();
    }
}

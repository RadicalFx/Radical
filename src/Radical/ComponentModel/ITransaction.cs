using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// A generic transaction abstraction layer.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Commits the pending transaction.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks the pending transaction.
        /// </summary>
        void Rollback();
    }
}

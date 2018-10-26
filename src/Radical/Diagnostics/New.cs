using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Radical.Diagnostics
{
    /// <summary>
    /// Entry point to create a new logical operation.
    /// </summary>
    public sealed class New : IDisposable
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="New"/> class from being created.
        /// </summary>
        /// <param name="operationId">The operation id.</param>
        private New( Object operationId )
        {
            Trace.CorrelationManager.StartLogicalOperation( operationId );
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Trace.CorrelationManager.StopLogicalOperation();
        }

        /// <summary>
        /// Creates a new logical operation.
        /// </summary>
        /// <param name="operationId">The operation id.</param>
        /// <returns>A self stopping logical operation instance.</returns>
        public static IDisposable LogicalOperation( Object operationId ) 
        {
            return new New(operationId);
        }
    }
}

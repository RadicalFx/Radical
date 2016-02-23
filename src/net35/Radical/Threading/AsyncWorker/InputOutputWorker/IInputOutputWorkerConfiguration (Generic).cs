using System;

namespace Topics.Radical.Threading
{
    /// <summary>
    /// Defines the configuration for a worker with input
    /// arguments and a result.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IInputOutputWorkerConfiguration<T, TResult> : IWorkerConfiguration
    {
        /// <summary>
        /// Gets or sets the before execution handler.
        /// </summary>
        /// <value>The before execution handler.</value>
        Action<IBeforeArgs<T>> Before { get; set; }

        /// <summary>
        /// Gets or sets the after execution handler.
        /// </summary>
        /// <value>The after execution handler.</value>
        Action<IAfterArgs<T, TResult>> After { get; set; }
    }
}

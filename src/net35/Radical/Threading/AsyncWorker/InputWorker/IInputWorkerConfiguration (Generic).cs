using System;

namespace Topics.Radical.Threading
{
    /// <summary>
    /// Defines the configuration for a worker with input arguments.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    public interface IInputWorkerConfiguration<T> : IWorkerConfiguration
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
        Action<IAfterArgs<T>> After { get; set; }
    }
}

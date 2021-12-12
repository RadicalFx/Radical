using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Defines a container entry.
    /// </summary>
    [Obsolete("PuzzleContainer has been removed in v2.0.0. Related contracts will be removed in v3.0.0.")]
    public interface IPuzzleContainerEntry : IContainerEntry
    {
        /// <summary>
        /// Sets the service instance to use as resolve result.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>This entry instance.</returns>
        IPuzzleContainerEntry UsingInstance(object instance);

        /// <summary>
        /// Defines the type that implements the service.
        /// </summary>
        /// <param name="componentType">The type of the component.</param>
        /// <returns>This entry instance.</returns>
        IPuzzleContainerEntry ImplementedBy(Type componentType);

        /// <summary>
        /// Defines the lifestyle of this entry.
        /// </summary>
        /// <param name="lifestyle">The lifestyle.</param>
        /// <returns>This entry instance.</returns>
        IPuzzleContainerEntry WithLifestyle(Lifestyle lifestyle);

        /// <summary>
        /// Defines the factory to use.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>This entry instance.</returns>
        IPuzzleContainerEntry UsingFactory(Func<object> factory);

        /// <summary>
        /// Defines the specified component registration as override allowed.
        /// </summary>
        /// <returns>
        /// This entry instance.
        /// </returns>
        IPuzzleContainerEntry Overridable();

        /// <summary>
        /// Forwards this instance.
        /// </summary>
        /// <param name="forwardedType">The forwarded type.</param>
        /// <returns></returns>
        IPuzzleContainerEntry Forward(Type forwardedType);
    }
}

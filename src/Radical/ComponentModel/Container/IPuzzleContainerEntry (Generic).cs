using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Defines a container entry.
    /// </summary>
    [Obsolete("PuzzleContainer has been removed in v2.0.0. Related contracts will be removed in v3.0.0.")]
    public interface IPuzzleContainerEntry<T> : IContainerEntry
    {
        /// <summary>
        /// Sets the service instance to use as resolve result.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>This entry instance.</returns>
        IPuzzleContainerEntry<T> UsingInstance<TComponent>(TComponent instance)
            where TComponent : T;

        /// <summary>
        /// Defines the type that implements the service.
        /// </summary>
        /// <param name="componentType">The type of the component.</param>
        /// <returns>This entry instance.</returns>
        IPuzzleContainerEntry<T> ImplementedBy(Type componentType);

        /// <summary>
        /// Defines the type that implements the service.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <returns>This entry instance.</returns>
        IPuzzleContainerEntry<T> ImplementedBy<TComponent>() where TComponent : T;

        /// <summary>
        /// Defines the lifestyle of this entry.
        /// </summary>
        /// <param name="lifestyle">The lifestyle.</param>
        /// <returns>This entry instance.</returns>
        IPuzzleContainerEntry<T> WithLifestyle(Lifestyle lifestyle);

        /// <summary>
        /// Defines the factory to use.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <returns>This entry instance</returns>
        IPuzzleContainerEntry<T> UsingFactory(Func<T> factory);

        /// <summary>
        /// Defines the specified component registration as override allowed.
        /// </summary>
        /// <returns>
        /// This entry instance.
        /// </returns>
        IPuzzleContainerEntry<T> Overridable();

        /// <summary>
        /// Forwards this instance.
        /// </summary>
        /// <typeparam name="TForwarded">The type of the forwarded.</typeparam>
        /// <returns></returns>
        IPuzzleContainerEntry<T> Forward<TForwarded>();
    }
}

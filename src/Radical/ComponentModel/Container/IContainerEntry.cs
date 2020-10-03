using System;
using System.Collections.Generic;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Defines a container entry.
    /// </summary>
    [Obsolete("PuzzleContainer has been removed in v2.0.0. Related contracts will be removed in v3.0.0.")]
    public interface IContainerEntry
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        string Key { get; }

        /// <summary>
        /// Gets the component type.
        /// </summary>
        /// <value>The component type.</value>
        Type Component { get; }

        /// <summary>
        /// Gets the service types.
        /// </summary>
        /// <value>
        /// The service types.
        /// </value>
        IEnumerable<Type> Services { get; }

        /// <summary>
        /// Gets the factory used to build up a concrete type.
        /// </summary>
        /// <value>The factory.</value>
        Delegate Factory { get; }

        /// <summary>
        /// Gets the lifestyle of this component.
        /// </summary>
        /// <value>The lifestyle.</value>
        Lifestyle Lifestyle { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        IDictionary<string, object> Parameters { get; }

        /// <summary>
        /// Gets an indication is this component can be overrode.
        /// </summary>
        /// <value>
        /// An indication is this component can be overrode.
        /// </value>
        bool IsOverridable { get; }
    }
}

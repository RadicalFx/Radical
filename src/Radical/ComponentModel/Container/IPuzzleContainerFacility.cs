using System;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Defines a facility for the puzzle container.
    /// </summary>
    [Obsolete("IPuzzleContainerFacility has been obsoleted and will be removed in v3.0.0")]
    public interface IPuzzleContainerFacility
    {
        /// <summary>
        /// Initializes this facility.
        /// </summary>
        /// <param name="container">The container hosting the facility.</param>
        void Initialize(IPuzzleContainer container);

        /// <summary>
        /// Tears down this facility.
        /// </summary>
        /// <param name="container">The container hosting the facility.</param>
        void Teardown(IPuzzleContainer container);
    }
}

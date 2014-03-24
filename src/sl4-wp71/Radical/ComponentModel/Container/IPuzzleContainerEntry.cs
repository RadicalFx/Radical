using System;
using System.Collections.Generic;

namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Defines a container entry.
	/// </summary>
	public interface IPuzzleContainerEntry : IContainerEntry
	{
		/// <summary>
		/// Sets the serviceinstance to use as resolve result.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <returns>This entry instance.</returns>
		IPuzzleContainerEntry UsingInstance( Object instance );

		/// <summary>
		/// Defines the type that implements the service.
		/// </summary>
		/// <param name="componentType">The type of the component.</param>
		/// <returns>This entry instance.</returns>
		IPuzzleContainerEntry ImplementedBy( Type componentType );

		/// <summary>
		/// Defines the lifestyle of this entry.
		/// </summary>
		/// <param name="lifestyle">The lifestyle.</param>
		/// <returns>This entry instance.</returns>
		IPuzzleContainerEntry WithLifestyle( Lifestyle lifestyle );
		
		/// <summary>
		/// Defines the factory to use.
		/// </summary>
		/// <param name="factory">The factory.</param>
		/// <returns>This entry instance.</returns>
		IPuzzleContainerEntry UsingFactory( Func<Object> factory );

		//IPuzzleContainerEntry WithParameters( IDictionary<String, Object> parameters );
	}
}

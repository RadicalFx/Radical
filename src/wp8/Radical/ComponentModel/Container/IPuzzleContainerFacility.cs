using System;
using System.Collections.Generic;

namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Defines a facility for the puzzle container.
	/// </summary>
	public interface IPuzzleContainerFacility
	{
		/// <summary>
		/// Initializes this facility.
		/// </summary>
		/// <param name="container">The container hosting the facility.</param>
		void Initialize( IPuzzleContainer container );

		/// <summary>
		/// Teardowns this facility.
		/// </summary>
		/// <param name="container">The container hosting the facility.</param>
		void Teardown( IPuzzleContainer container );
	}
}

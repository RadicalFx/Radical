using System;
using System.Collections.Generic;

namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Defines a container entry.
	/// </summary>
	public interface IContainerEntry
	{
		/// <summary>
		/// Gets the component type.
		/// </summary>
		/// <value>The component type.</value>
		Type Component { get; }

		/// <summary>
		/// Gets the service type.
		/// </summary>
		/// <value>The service type.</value>
		Type Service { get; }

		/// <summary>
		/// Gets the factory used to build up a concrete type.
		/// </summary>
		/// <value>The factory.</value>
		Delegate Factory { get; }

		/// <summary>
		/// Gets the lifestyle of ths component.
		/// </summary>
		/// <value>The lifestyle.</value>
		Lifestyle Lifestyle { get; }

		/// <summary>
		/// Gets the parameters.
		/// </summary>
		IDictionary<String, Object> Parameters { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	/// <summary>
	/// Manage the release process of a component.
	/// </summary>
	public interface IReleaseComponents
	{
		/// <summary>
		/// Releases the given component.
		/// </summary>
		/// <param name="component">The component to release.</param>
		void Release( Object component );
	}
}

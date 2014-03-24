using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Topics.Radical.Validation;

namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Defines event args for the ComponentRegistered event.
	/// </summary>
	public class ComponentRegisteredEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ComponentRegisteredEventArgs"/> class.
		/// </summary>
		/// <param name="entry">The entry.</param>
		public ComponentRegisteredEventArgs( IContainerEntry entry )
		{
			Ensure.That( entry ).Named( "entry" ).IsNotNull();
			this.Entry = entry;
		}

		/// <summary>
		/// Gets the entry.
		/// </summary>
		public IContainerEntry Entry
		{
			get;
			private set;
		}
	}
}

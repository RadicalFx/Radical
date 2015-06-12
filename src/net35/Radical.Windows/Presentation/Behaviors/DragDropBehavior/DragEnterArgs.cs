using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Behaviors
{
	public class DragEnterArgs : DragDropOperationArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DragEnterArgs" /> class.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="keyStates">The key states.</param>
		/// <param name="dropTarget">The drop target.</param>
		/// <param name="allowedEffects">The allowed effects.</param>
		public DragEnterArgs( IDataObject data, DragDropKeyStates keyStates, Object dropTarget, DragDropEffects allowedEffects )
			: base( data, keyStates, dropTarget )
		{
			Ensure.That( allowedEffects ).Named( "allowedEffects" ).IsTrue( v => v.IsDefined() );

			this.AllowedEffects = allowedEffects;
		}

		/// <summary>
		/// Gets the allowed effects.
		/// </summary>
		/// <value>The allowed effects.</value>
		public DragDropEffects AllowedEffects { get; private set; }
	}
}

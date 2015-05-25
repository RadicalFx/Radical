using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Behaviors
{
	public class DragEnter : DragDropOperationArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DragEnter"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="keyStates">The key states.</param>
		/// <param name="dropTarget">The drop target.</param>
		/// <param name="allowedEffects">The allowed effects.</param>
		public DragEnter( IDataObject data, DragDropKeyStates keyStates, Object dropTarget, DragDropEffects allowedEffects )
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

		/// <summary>
		/// Gets or sets the effects.
		/// </summary>
		/// <value>The effects.</value>
		public DragDropEffects Effects { get; set; }
	}
}

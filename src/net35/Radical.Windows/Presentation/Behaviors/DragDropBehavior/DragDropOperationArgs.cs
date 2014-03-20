using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Behaviors
{
	public abstract class DragDropOperationArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DragDropOperationArgs"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="keyStates">The key states.</param>
		/// <param name="dropTarget">The drop target.</param>
		protected DragDropOperationArgs( IDataObject data, DragDropKeyStates keyStates, Object dropTarget )
		{
			Ensure.That( data ).Named( "data" ).IsNotNull();
			Ensure.That( keyStates ).Named( "keyStates" ).IsTrue( ks => ks.IsDefined() );

			this.Data = data;
			this.KeyStates = keyStates;
			this.DropTarget = dropTarget;
		}

		/// <summary>
		/// Gets the data.
		/// </summary>
		/// <value>The data.</value>
		public IDataObject Data { get; private set; }
	
		/// <summary>
		/// Gets the key states.
		/// </summary>
		/// <value>The key states.</value>
		public DragDropKeyStates KeyStates { get; private set; }

		/// <summary>
		/// Gets or sets the drop target.
		/// </summary>
		/// <value>The drop target.</value>
		public Object DropTarget { get; private set; }
	}
}

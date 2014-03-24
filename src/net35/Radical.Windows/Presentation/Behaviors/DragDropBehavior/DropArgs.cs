using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Topics.Radical.Windows.Behaviors
{
	public class DropArgs : DragDropOperationArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DropArgs"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="keyStates">The key states.</param>
		/// <param name="dropTarget">The drop target.</param>
		public DropArgs( IDataObject data, DragDropKeyStates keyStates, Object dropTarget )
			: base( data, keyStates, dropTarget )
		{

		}
	}
}

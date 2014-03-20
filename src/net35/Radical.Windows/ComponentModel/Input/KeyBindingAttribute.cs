using System;

namespace Topics.Radical.ComponentModel.Windows.Input
{
	[AttributeUsage( AttributeTargets.Method, AllowMultiple = true )]
	public class KeyBindingAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KeyBindingAttribute"/> class.
		/// </summary>
		/// <param name="key">The keyboard key.</param>
		public KeyBindingAttribute( System.Windows.Input.Key key )
		{
			this.Key = key;
		}

		/// <summary>
		/// Gets the keyboard key to associate with the command.
		/// </summary>
		/// <value>The keyboard key.</value>
		public System.Windows.Input.Key Key { get; private set; }

		/// <summary>
		/// Gets the modifier keyboard key to associate with the command.
		/// </summary>
		/// <value>The modifier keyboard key.</value>
		public System.Windows.Input.ModifierKeys Modifiers { get; set; }
	}
}

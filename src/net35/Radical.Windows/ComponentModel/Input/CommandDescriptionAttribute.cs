using System;

namespace Topics.Radical.ComponentModel.Windows.Input
{
	[AttributeUsage( AttributeTargets.Method, AllowMultiple = false )]
	public class CommandDescriptionAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommandDescriptionAttribute"/> class.
		/// </summary>
		/// <param name="displayText">The display text.</param>
		public CommandDescriptionAttribute( String displayText )
		{
			this.DisplayText = displayText;
		}

		/// <summary>
		/// Gets the command display text.
		/// </summary>
		/// <value>The display text.</value>
		public virtual String DisplayText { get; private set; }
	}
}

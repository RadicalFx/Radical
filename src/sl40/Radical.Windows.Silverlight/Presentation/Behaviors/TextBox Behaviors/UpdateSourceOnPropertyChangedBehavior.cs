using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Topics.Radical.Windows.Behaviors
{
	/// <summary>
	/// Adds a behavior to the TextBox to update the source property
	/// whenever the text property changes.
	/// </summary>
	public class UpdateSourceOnPropertyChangedBehavior : Behavior<TextBox>
	{
		readonly TextChangedEventHandler handler;

		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateSourceOnPropertyChangedBehavior"/> class.
		/// </summary>
		public UpdateSourceOnPropertyChangedBehavior()
		{
			this.handler = ( s, e ) =>
			{
				var be = this.AssociatedObject.GetBindingExpression( TextBox.TextProperty );
				be.UpdateSource();
			};
		}

		/// <summary>
		/// Called after the behavior is attached to an AssociatedObject.
		/// </summary>
		/// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.TextChanged += this.handler;
		}

		/// <summary>
		/// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		/// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
		protected override void OnDetaching()
		{
			this.AssociatedObject.TextChanged -= this.handler;

			base.OnDetaching();
		}
	}
}

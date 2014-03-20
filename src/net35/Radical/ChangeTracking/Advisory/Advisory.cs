namespace Topics.Radical.ChangeTracking
{
	using System.Collections.Generic;
	using Topics.Radical.Collections;
	using Topics.Radical.ComponentModel.ChangeTracking;

	/// <summary>
	/// An advisory is a read-only list of <c>IAdvisedAction</c>(s) that describes
	/// the action that the change tracking provisioning engine suggest that should
	/// be executed in order to persist the occurred changes.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" )]
	public class Advisory : ReadOnlyCollection<IAdvisedAction>, IAdvisory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Advisory"/> class.
		/// </summary>
		/// <param name="actions">The actions.</param>
		public Advisory( IEnumerable<IAdvisedAction> actions )
			: base( actions )
		{
 
		}
	}
}

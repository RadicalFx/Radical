using System.Collections.Generic;
namespace Topics.Radical.ComponentModel.ChangeTracking
{

	/// <summary>
	/// An advisory is a read-only list of <c>IAdvisedAction</c>(s) that describes
	/// the action that the change tracking provisioning engine suggest that should
	/// be executed in order to persist the occurred changes.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" )]
	public interface IAdvisory : IReadOnlyCollection<IAdvisedAction>
	{
		
	}
}

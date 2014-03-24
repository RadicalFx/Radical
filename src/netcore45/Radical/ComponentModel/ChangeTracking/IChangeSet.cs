using System.Collections.Generic;
namespace Topics.Radical.ComponentModel.ChangeTracking
{

	/// <summary>
	/// A change set is a readonly list of <see cref="IChange"/> instances.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" )]
	public interface IChangeSet : IReadOnlyCollection<IChange>
	{
		
	}
}

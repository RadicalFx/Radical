namespace Topics.Radical.ComponentModel.ChangeTracking
{
	/// <summary>
	/// <c>IAdvisoryBuilder</c> interface is used by the change tracking service
	/// in order to allow callers to customize the advisory build process.
	/// </summary>
	public interface IAdvisoryBuilder
	{
		/// <summary>
		/// Generates the advisory.
		/// </summary>
		/// <param name="svc">The service that holds the data to generate the advisory for.</param>
		/// <param name="changeSet">The subset of changes to generate the advisory for.</param>
		/// <returns>The generated advisory.</returns>
		IAdvisory GenerateAdvisory( IChangeTrackingService svc, IChangeSet changeSet );
	}
}

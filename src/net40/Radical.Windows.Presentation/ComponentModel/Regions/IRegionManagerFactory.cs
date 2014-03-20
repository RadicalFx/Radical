namespace Topics.Radical.Windows.Presentation.ComponentModel
{

	/// <summary>
	/// A region manager factory.
	/// </summary>
	public interface IRegionManagerFactory
	{
		/// <summary>
		/// Creates a new region manager.
		/// </summary>
		/// <returns>A new region manager.</returns>
		IRegionManager Create();
	}
}
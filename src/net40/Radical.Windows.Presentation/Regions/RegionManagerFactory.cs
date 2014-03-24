using System;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Regions
{
	/// <summary>
	/// Default region manager factory.
	/// </summary>
	public class RegionManagerFactory : IRegionManagerFactory
	{
		readonly IServiceProvider container;

		/// <summary>
		/// Initializes a new instance of the <see cref="RegionManagerFactory"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public RegionManagerFactory( IServiceProvider container )
		{
			Ensure.That( container ).Named( "container" ).IsNotNull();

			this.container = container;
		}

		/// <summary>
		/// Creates a new region manager.
		/// </summary>
		/// <returns>
		/// A new region manager.
		/// </returns>
		public IRegionManager Create()
		{
			return ( IRegionManager )this.container.GetService( typeof( IRegionManager ) );
		}
	}
}

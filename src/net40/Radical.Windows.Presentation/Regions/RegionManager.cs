using System;
using System.Collections.Generic;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Regions
{
	/// <summary>
	/// Default region manager.
	/// </summary>
	public class RegionManager : IRegionManager
	{
		readonly IDictionary<String, IRegion> regions = new Dictionary<String, IRegion>();

		/// <summary>
		/// Registers the supplied region in this region manager.
		/// </summary>
		/// <param name="region">The region to register.</param>
		public void RegisterRegion( IRegion region )
		{
			if( this.regions.ContainsKey( region.Name ) )
			{
				throw new InvalidOperationException();
			}

			this.regions.Add( region.Name, region );
		}

		/// <summary>
		/// Gets the <see cref="Topics.Radical.Windows.Presentation.ComponentModel.IRegion"/> with the specified name.
		/// </summary>
		public IRegion this[ string name ]
		{
			get { return this.regions[ name ]; }
		}


		/// <summary>
		/// Gets the region registered with the given name.
		/// </summary>
		/// <param name="name">The name of the region.</param>
		/// <returns>
		/// The searched region, or an ArgumentOutOfRangeException if no region is registered with the given name.
		/// </returns>
		public IRegion GetRegion( string name )
		{
			return this[ name ];
		}

		/// <summary>
		/// Gets the region.
		/// </summary>
		/// <typeparam name="TRegion">The type of the region.</typeparam>
		/// <param name="name">The name.</param>
		/// <returns>
		/// The searched region, or an ArgumentOutOfRangeException if no region is registered with the given name.
		/// </returns>
		public TRegion GetRegion<TRegion>( string name ) where TRegion : IRegion
		{
			return ( TRegion )this.GetRegion( name );
		}

		/// <summary>
		/// Tries the get region.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="region">The region.</param>
		/// <returns></returns>
		public Boolean TryGetRegion( string name, out IRegion region )
		{
			return this.regions.TryGetValue( name, out region );
		}

		/// <summary>
		/// Tries to get the region.
		/// </summary>
		/// <typeparam name="TRegion">The type of the region.</typeparam>
		/// <param name="regionName">Name of the region.</param>
		/// <param name="region">The region.</param>
		/// <returns>
		///   <c>True</c> if the region has been found; otherwise <c>false</c>.
		/// </returns>
		public bool TryGetRegion<TRegion>( string regionName, out TRegion region ) where TRegion : IRegion
		{
			IRegion rg;
			if( this.TryGetRegion( regionName, out rg ) && rg is TRegion )
			{
				region = ( TRegion )rg;
				return true;
			}

			region = default( TRegion );
			return false;
		}

		/// <summary>
		/// Closes this region manager, the close process is invoked by the host at close time.
		/// </summary>
		public void Shutdown()
		{
			this.regions.Values.ForEach( r => r.Shutdown() );
			this.regions.Clear();
		}

		/// <summary>
		/// Gets all the registered the regions.
		/// </summary>
		/// <returns>
		/// All the registered the regions.
		/// </returns>
		public IEnumerable<IRegion> GetAllRegisteredRegions()
		{
			return this.regions.Values;
		}
	}
}

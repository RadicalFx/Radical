using System;
using System.Linq;
using Castle.Windsor;
using System.Collections.Generic;

namespace Castle.MicroKernel.SubSystems.Naming
{
	/// <summary>
	/// Extends the default Castle naming subsystem.
	/// </summary>
	public class DelegateNamingSubSystem : DefaultNamingSubSystem
	{
		/// <summary>
		/// Gets or sets the sub system handler.
		/// </summary>
		/// <value>
		/// The sub system handler.
		/// </value>
		public Func<Type, IEnumerable<IHandler>, IHandler> SubSystemHandler { get; set; }

		/// <summary>
		/// Gets the handler for the given service type.
		/// </summary>
		/// <param name="service">The service.</param>
		/// <returns>The handler for the given service type.</returns>
		public override IHandler GetHandler( Type service )
		{
			IHandler handler = null;

			if( this.SubSystemHandler != null )
			{
				var handlers = base.GetHandlers( service );
				handler = this.SubSystemHandler( service, handlers );
			}

			return handler ?? base.GetHandler( service );
		}
	}
}

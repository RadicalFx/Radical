using System;
using System.Linq;
using System.Collections.Generic;
using Topics.Radical.Windows.Presentation.Boot;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Reflection;
using Autofac;
using Autofac.Core;

namespace Topics.Radical.Windows.Presentation.Modules
{
	public class InjectViewInRegionModule : IModule
    {
		readonly BootstrapConventions conventions;

		public InjectViewInRegionModule( BootstrapConventions conventions )
		{
			this.conventions = conventions;
		}

		public void Configure( IComponentRegistry componentRegistry )
		{
			componentRegistry.Registered += OnRegistered;
		}

		Dictionary<String, List<Type>> buffer = new Dictionary<string, List<Type>>();

		void OnRegistered( object sender, ComponentRegisteredEventArgs e )
		{
			var typeTo = e.ComponentRegistration.Activator.LimitType;
			var regionName = this.conventions.GetInterestedRegionNameIfAny( typeTo );
			if ( !String.IsNullOrWhiteSpace( regionName ) )
			{
				var viewType = typeTo;

				if ( this.buffer.ContainsKey( regionName ) )
				{
					this.buffer[ regionName ].Add( viewType );
				}
				else
				{
					this.buffer.Add( regionName, new List<Type>() { viewType } );
				}
			}
		}

		internal void Commit(IContainer container)
		{
			if ( this.buffer.Any() )
			{
				var mh = container.Resolve<IRegionInjectionHandler>();
				foreach ( var kvp in this.buffer )
				{
					mh.RegisterViewsAsInterestedIn(
						regionName: kvp.Key,
						views: kvp.Value.AsEnumerable() );
				}

				this.buffer.Clear();
			}
		}
	}
}

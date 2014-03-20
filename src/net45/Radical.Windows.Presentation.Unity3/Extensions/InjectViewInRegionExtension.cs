using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Topics.Radical.Windows.Presentation.Boot;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Reflection;
using Microsoft.Practices.ObjectBuilder2;

namespace Topics.Radical.Windows.Presentation.Extensions
{
	public class InjectViewInRegionExtension : UnityContainerExtension
    {
		Dictionary<String, List<Type>> buffer = new Dictionary<string, List<Type>>();

		public override void Remove()
		{
			this.Context.Registering -= OnRegistering;

			//should never be interesting for regions and view
			//this.Context.RegisteringInstance -= OnRegisteringInstance;

			base.Remove();
		}

		protected override void Initialize()
		{
			this.Context.Registering += OnRegistering;
		
			//should never be interesting for regions and view
			//this.Context.RegisteringInstance += OnRegisteringInstance;
		}

		void OnRegistering( object sender, RegisterEventArgs e )
		{
			//var model = e.ComponentModel;
			var conventions = this.Container.Resolve<BootstrapConventions>();

			if ( e.TypeFrom.Is<IRegionInjectionHandler>() )
			{
				if ( this.buffer.Any() )
				{
					var mh = this.Container.Resolve<IRegionInjectionHandler>();
					foreach ( var kvp in this.buffer )
					{
						mh.RegisterViewsAsInterestedIn(
							regionName: kvp.Key,
							views: kvp.Value.AsEnumerable() );
					}

					this.buffer.Clear();
				}
			}
			else
			{
				var regionName = conventions.GetInterestedRegionNameIfAny( e.TypeTo );
				if ( !String.IsNullOrWhiteSpace( regionName ) )
				{
					var viewType = e.TypeTo;

					if ( this.Container.IsRegistered<IRegionInjectionHandler>() )
					{
						var mh = this.Container.Resolve<IRegionInjectionHandler>();
						mh.RegisterViewAsInterestedIn( regionName, viewType );
					}
					else if ( this.buffer.ContainsKey( regionName ) )
					{
						this.buffer[ regionName ].Add( viewType );
					}
					else
					{
						this.buffer.Add( regionName, new List<Type>() { viewType } );
					}
				}
			}
		}
	}
}

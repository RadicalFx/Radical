using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Topics.Radical.ComponentModel;
using Topics.Radical.Reflection;

namespace Topics.Radical
{
	/// <summary>
	/// Defines a facility that automatically boot a component at registration time.
	/// </summary>
	public class BootableFacility : IPuzzleContainerFacility
	{
		IPuzzleContainer container;

		/// <summary>
		/// Initializes this facility.
		/// </summary>
		/// <param name="container">The container hosting the facility.</param>
		public void Initialize( IPuzzleContainer container )
		{
			this.container = container;
			container.ComponentRegistered += new EventHandler<ComponentRegisteredEventArgs>( OnComponentRegistered );
		}

		void OnComponentRegistered( object sender, ComponentRegisteredEventArgs e )
		{
			if( e.Entry.Service.Is<IBootable>() || e.Entry.Component.Is<IBootable>() )
			{
				var t = this.GetTypeToResolve( e.Entry );
				var svc = ( IBootable )this.container.Resolve( t );
				svc.Boot();
			}
		}

		Type GetTypeToResolve( IContainerEntry entry ) 
		{
			return entry.Service ?? entry.Component;
		}

		/// <summary>
		/// Teardowns this facility.
		/// </summary>
		/// <param name="container">The container hosting the facility.</param>
		public void Teardown( IPuzzleContainer container )
		{
			container.ComponentRegistered -= new EventHandler<ComponentRegisteredEventArgs>( OnComponentRegistered );
		}
	}
}

using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Radical.ComponentModel;
//using Radical.Reflection;

namespace Radical
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

        Boolean IsBootable( Type type )
        {
            return type.IsAssignableFrom( typeof( IBootable ).GetType() );
        }

        void OnComponentRegistered( object sender, ComponentRegisteredEventArgs e )
        {
            if ( e.Entry.Services.Any( s => this.IsBootable( s ) ) || this.IsBootable( e.Entry.Component ) )
            {
                var t = this.GetTypeToResolve( e.Entry );
                var svc = ( IBootable )this.container.Resolve( t );
                svc.Boot();
            }
        }

        Type GetTypeToResolve( IContainerEntry entry )
        {
            return entry.Services.Any() 
                ? entry.Services.First() 
                : entry.Component;
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

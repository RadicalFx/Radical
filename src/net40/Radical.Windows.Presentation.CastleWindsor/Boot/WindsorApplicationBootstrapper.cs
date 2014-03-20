using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using Castle;
using Castle.Facilities;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Naming;
using Castle.Windsor;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.Messaging;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public class WindsorApplicationBootstrapper : ApplicationBootstrapper
    {
        IWindsorContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorApplicationBootstrapper"/> class.
        /// </summary>
        public WindsorApplicationBootstrapper()
        {

        }

        protected override IServiceProvider CreateServiceProvider()
        {
            var nss = new DelegateNamingSubSystem()
            {
                SubSystemHandler = ( s, hs ) =>
                {
                    if ( hs.Any( h => h.ComponentModel.IsOverridable() ) )
                    {
                        var nonOverridable = hs.Except( hs.Where( h => h.ComponentModel.IsOverridable() ) );
                        if ( nonOverridable.Any() )
                        {
                            return nonOverridable.Single();
                        }
                    }

                    return null;
                }
            };

            this.container = new Castle.Windsor.WindsorContainer();

            this.container.Kernel.AddSubSystem( SubSystemConstants.NamingKey, nss );
            this.container.Kernel.Resolver.AddSubResolver( new ArrayResolver( this.container.Kernel, true ) );

            var wrapper = new ServiceProviderWrapper( this.container );

            this.container.Register( Component.For<IServiceProvider>().Instance( wrapper ) );
            this.container.Register( Component.For<IWindsorContainer>().Instance( this.container ) );
            this.container.Register( Component.For<Boot.BootstrapConventions>() );

            this.container.AddFacility<Castle.Facilities.SubscribeToMessageFacility>();
            this.container.AddFacility<InjectViewInRegionFacility>();

            return wrapper;
        }

        [ImportMany]
        IEnumerable<IWindsorInstaller> Installers { get; set; }

        protected override void OnCompositionContainerComposed( CompositionContainer container, IServiceProvider serviceProvider )
        {
            base.OnCompositionContainerComposed( container, serviceProvider );

            var toInstall = this.Installers.Where( i => this.ShouldInstall( i ) ).ToArray();
            this.container.Install( toInstall );
        }

        protected virtual Boolean ShouldInstall( IWindsorInstaller installer )
        {
            return true;
        }

        protected override void OnBootCompleted( IServiceProvider serviceProvider )
        {
            base.OnBootCompleted( serviceProvider );

            var callbacks = this.container.ResolveAll<IExpectBootCallback>();
            if ( callbacks != null && callbacks.Any() )
            {
                foreach ( var cb in callbacks )
                {
                    cb.OnBootCompleted();
                }
            }
        }

        protected override void OnShutdown( ApplicationShutdownArgs e )
        {
            base.OnShutdown( e );

            if ( e.IsBootCompleted )
            {
                var callbacks = this.container.ResolveAll<IExpectShutdownCallback>();
                if ( callbacks != null && callbacks.Any() )
                {
                    foreach ( var cb in callbacks )
                    {
                        cb.OnShutdown( e.Reason );
                    }
                }
            }
        }
    }
}

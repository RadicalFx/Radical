using System.Collections.Generic;
using System.ComponentModel.Composition;
using Castle.Core;
using Castle.MicroKernel.Registration;
using System.Linq;
using System;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
    /// <summary>
    /// A Windsor installer.
    /// </summary>
    [Export( typeof( IWindsorInstaller ) )]
    public class PresentationInstaller : IWindsorInstaller
    {
        IEnumerable<Type> GetServices( ComponentRegistration r ) 
        {
            var bf = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
            var property = r.GetType().GetProperties( bf ).Single( p => p.Name == "Services" );
            var types = property.GetValue( r, null ) as IEnumerable<Type>;

            return types;
        }

        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install( Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store )
        {
            var conventions = container.Resolve<BootstrapConventions>();
            var currentDirectory = Helpers.EnvironmentHelper.GetCurrentDirectory();

            container.Register
            (
				AllTypes.FromAssemblyInDirectory( new AssemblyFilter( currentDirectory ).FilterByAssembly( conventions.IncludeAssemblyInContainerScan ) )
                    .IncludeNonPublicTypes()
                    .Where( t => conventions.IsViewModel( t ) && !conventions.IsExcluded( t ) )
                    .WithService.Select( ( type, baseTypes ) => conventions.SelectViewModelContracts( type ) )
                    .Configure( r =>
                    {
                        r.PropertiesIgnore( conventions.IgnoreViewModelPropertyInjection );

                        var services = this.GetServices( r );

                        if ( conventions.IsShellViewModel( services, r.Implementation) )
                        {
                            r.LifeStyle.Is( LifestyleType.Singleton );
                        }
                        else
                        {
                            r.LifeStyle.Is( LifestyleType.Transient );
                        }
                    } )
            );

            container.Register
            (
				AllTypes.FromAssemblyInDirectory( new AssemblyFilter( currentDirectory ).FilterByAssembly( conventions.IncludeAssemblyInContainerScan ) )
                    .IncludeNonPublicTypes()
                    .Where( t => conventions.IsView( t ) && !conventions.IsExcluded( t ) )
                    .WithService.Select( ( type, baseTypes ) => conventions.SelectViewContracts( type ) )
                    .Configure( r =>
                    {
						r.PropertiesIgnore( conventions.IgnoreViewPropertyInjection );

                        var services = this.GetServices( r );

                        if ( conventions.IsShellView( services, r.Implementation ) )
                        {
                            r.LifeStyle.Is( LifestyleType.Singleton );
                        }
                        else
                        {
                            r.LifeStyle.Is( LifestyleType.Transient );
                        }
                    } )

            );
        }
    }
}
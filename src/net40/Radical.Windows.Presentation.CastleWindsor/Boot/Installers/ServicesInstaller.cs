using System.ComponentModel.Composition;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
    /// <summary>
    /// A Windsor installer.
    /// </summary>
    [Export( typeof( IWindsorInstaller ) )]
    public class ServicesInstaller : IWindsorInstaller
    {
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
                    .Where( t => conventions.IsService( t ) && !conventions.IsExcluded( t ) )
                    .WithService.Select( ( type, baseTypes ) => conventions.SelectServiceContracts( type ) )
                    .Configure( r =>
                    {
                        r.Overridable();
                        r.PropertiesIgnore( conventions.IgnorePropertyInjection );
                    } )
            );
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using Autofac;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Regions;

namespace Topics.Radical.Windows.Presentation.Boot.Modules
{
	public class UICompositionModule : IAutofacBuiltinModule
	{
		public void Configure( ContainerBuilder builder, BootstrapConventions conventions, IEnumerable<Assembly> assemblies )
		{
			builder.RegisterType<RegionManagerFactory>().As<IRegionManagerFactory>().SingleInstance();
			builder.RegisterType<RegionService>().As<IRegionService>().SingleInstance();
			builder.RegisterType<RegionManager>().As<IRegionManager>().InstancePerDependency();
		}
	}
}
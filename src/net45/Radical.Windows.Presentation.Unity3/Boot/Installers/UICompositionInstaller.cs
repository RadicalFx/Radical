using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.Practices.Unity;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Extensions;
using Topics.Radical.Windows.Presentation.Regions;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	public class UICompositionInstaller : IUnityInstaller
	{
		public void Install( IUnityContainer container, BootstrapConventions conventions, IEnumerable<Type> allTypes )
		{
			container.RegisterType<IRegionManagerFactory, RegionManagerFactory>( new ContainerControlledLifetimeManager(), new CandidateConstructorSelector( container ) );
			container.RegisterType<IRegionService, RegionService>( new ContainerControlledLifetimeManager(), new CandidateConstructorSelector( container ) );
			container.RegisterType<IRegionManager, RegionManager>( new TransientLifetimeManager(), new CandidateConstructorSelector( container ) );
		}
	}
}
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
using Castle.Windsor;

namespace Radical.Presentation.Samples.Boot
{
	class WindsorServiceProviderWrapper : IServiceProvider
	{
		private IWindsorContainer windsorContainer;

		public WindsorServiceProviderWrapper( IWindsorContainer windsorContainer )
		{
			this.windsorContainer = windsorContainer;
		}

		public object GetService( Type serviceType )
		{
			if( this.windsorContainer.Kernel.HasComponent( serviceType ) )
			{
				return this.windsorContainer.Resolve( serviceType );
			}

			return null;
		}
	}
}
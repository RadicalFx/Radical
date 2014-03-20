using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Topics.Radical.ComponentModel;

namespace Topics.Radical
{
	public class PuzzleContainerServiceProviderFacade : IServiceProvider
	{
		readonly IPuzzleContainer container;

		public PuzzleContainerServiceProviderFacade( IPuzzleContainer container )
		{
			this.container = container;
		}

		public object GetService( Type serviceType )
		{
			if( this.container.IsRegistered( serviceType.GetTypeInfo() ) )
			{
				return this.container.Resolve( serviceType.GetTypeInfo() );
			}

			return null;
		}
	}
}

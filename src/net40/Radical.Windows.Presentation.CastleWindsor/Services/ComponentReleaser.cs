using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Services
{
	class ComponentReleaser : IReleaseComponents
	{
		readonly IWindsorContainer container;

		public ComponentReleaser(IWindsorContainer container)
		{
			this.container = container;
		}

		public void Release( object component )
		{
			if ( component != null ) 
			{
				this.container.Release( component );
			}
		}
	}
}

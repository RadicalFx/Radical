using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Services
{
	class ComponentReleaser : IReleaseComponents
	{
		readonly IContainer container;

		public ComponentReleaser( IContainer container )
		{
			this.container = container;
		}

		public void Release( object component )
		{
			//NOP
		}
	}
}

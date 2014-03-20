using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Topics.Radical.Windows.Presentation.Boot
{
	[InheritedExport]
	public interface IAutofacModule
	{
		void Configure( Autofac.ContainerBuilder updater, BootstrapConventions conventions, IEnumerable<Assembly> assemblies );
	}

	internal interface IAutofacBuiltinModule : IAutofacModule { }
}

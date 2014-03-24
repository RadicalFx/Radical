using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Topics.Radical.Windows.Presentation.Boot
{
	[InheritedExport]
	public interface IUnityInstaller
	{
		void Install( IUnityContainer container, BootstrapConventions conventions, IEnumerable<Type> allTypes );
	}
}

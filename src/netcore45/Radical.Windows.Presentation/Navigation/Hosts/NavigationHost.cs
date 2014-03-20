using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Presentation.Navigation.Hosts
{
	public abstract class NavigationHost
	{
		public abstract DependencyObject Content { get; set; }
	}
}

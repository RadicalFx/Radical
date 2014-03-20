using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows.Presentation.Navigation.Hosts
{
	public class WindowNavigationHost : NavigationHost
	{
		readonly Window window;

		public WindowNavigationHost( Window window )
		{
			this.window = window;
		}

		public override DependencyObject Content
		{
			get { return this.window.Content; }
			set { this.window.Content = ( UIElement )value; }
		}
	}
}

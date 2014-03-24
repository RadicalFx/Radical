using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	public class NavigatingAwayEventArgs
	{
		public NavigatingAwayEventArgs( NavigationMode mode, INavigationScopedStorage storage )
		{
			this.Mode = mode;
			this.Storage = storage;
		}

		public NavigationMode Mode { get; private set; }
		public INavigationScopedStorage Storage { get; private set; }
		public Boolean Cancel { get; set; }
	}
}

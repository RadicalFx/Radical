using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	public class NavigationEventArgs
	{
		public NavigationEventArgs( NavigationMode mode, object arguments, INavigationScopedStorage storage, Boolean retrievedFromCache )
		{
			this.Mode = mode;
			this.Arguments = arguments;
			this.RetrievedFromCache = retrievedFromCache;
			this.Storage = storage;
		}

		public NavigationMode Mode { get; private set; }
		public Object Arguments { get; private set; }
		public bool RetrievedFromCache { get; private set; }
		public INavigationScopedStorage Storage { get; private set; }
	}
}

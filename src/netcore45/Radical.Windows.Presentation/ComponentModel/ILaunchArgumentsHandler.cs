using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	public interface ILaunchArgumentsHandler
	{
		Task OnLaunch( ILaunchActivatedEventArgs args );
	}
}

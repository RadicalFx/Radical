using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	public interface IProtocolRequestHandler
	{
		Task OnProtocolRequest( IProtocolActivatedEventArgs e );
	}
}

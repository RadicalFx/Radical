using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Presentation.Messaging;

namespace Radical.Samples.Unity2.Messaging.Handlers
{
	class ApplicationBootCompletedHandler : AbstractMessageHandler<ApplicationBootCompleted>
	{
		public override void Handle( object sender, ApplicationBootCompleted message )
		{
			
		}
	}
}

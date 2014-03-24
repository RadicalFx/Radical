using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Conversions;
using Radical.Presentation.Samples.Messaging;
using Topics.Radical.Windows.Presentation.Messaging;

namespace Radical.Presentation.Samples.Messaging.Handlers
{
	class ApplicationBootCompletedHandler : MessageHandler<ApplicationBootCompleted>, INeedSafeSubscription
	{
		public override void Handle( ApplicationBootCompleted message )
		{
			
		}
	}
}

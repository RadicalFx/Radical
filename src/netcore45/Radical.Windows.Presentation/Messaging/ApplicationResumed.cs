using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Messaging
{
	public class ApplicationResumed
	{
		public ApplicationResumed( ISuspensionManager sender )
		{
            this.SuspentionManager = sender;
        }

        public ISuspensionManager SuspentionManager
        {
            get;
            private set;
        }
	}
}

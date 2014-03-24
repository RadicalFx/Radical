using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Presentation.Messaging;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Radical.Presentation.Samples.Messaging.Handlers
{
    class ApplicationBootCompletedHandler : 
        AbstractMessageHandler<ApplicationBootCompleted>, 
        INeedSafeSubscription
    {
        readonly CoreDispatcher dispatcher;

        public ApplicationBootCompletedHandler( CoreDispatcher dispatcher )
        {
            this.dispatcher = dispatcher;
        }

        public override void Handle( object sender, ApplicationBootCompleted message )
        {
            
        }
    }
}

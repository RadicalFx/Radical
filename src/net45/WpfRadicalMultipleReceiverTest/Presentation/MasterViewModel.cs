using System;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using WpfRadicalMultipleReceiver.Messaging;

namespace WpfRadicalMultipleReceiver.Presentation
{
    public class MasterViewModel : AbstractViewModel, IExpectViewLoadedCallback
    {
        private readonly IMessageBroker _broker;

        public MasterViewModel(IMessageBroker broker)
        {
            _broker = broker;
        }

        public void OnViewLoaded()
        {
            _broker.Broadcast(this, new LoadViewInRegionRequest(typeof(ChildOneView), MasterViewRegion.LeftRegion));
            _broker.Broadcast(this, new LoadViewInRegionRequest(typeof(ChildTwoView), MasterViewRegion.RightRegion));

            count++;
            if (count > 1) throw new Exception("too many OnViewLoaded()");
        }

        private int count = 0;
    }
}

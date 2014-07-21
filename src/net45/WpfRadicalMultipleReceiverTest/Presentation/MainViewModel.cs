using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.ComponentModel.Windows.Input;
using Topics.Radical.Windows.Input;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using WpfRadicalMultipleReceiver.Messaging;

namespace WpfRadicalMultipleReceiver.Presentation
{
    public class MainViewModel : AbstractViewModel, IExpectViewLoadedCallback
    {
        private readonly IMessageBroker _broker;

        public IDelegateCommand OpenAnotherMasterViewCommand { get; protected set; }

        public MainViewModel(IMessageBroker broker)
        {
            _broker = broker;

            this.OpenAnotherMasterViewCommand =
                DelegateCommand.Create()
                               .OnExecute(x => _broker.Broadcast(this, new LoadViewInShellRequest(typeof(MasterView))));
        }

        public void OnViewLoaded()
        {
            _broker.Broadcast(this, new LoadViewInShellRequest(typeof(MasterView)));
        }
    }
}
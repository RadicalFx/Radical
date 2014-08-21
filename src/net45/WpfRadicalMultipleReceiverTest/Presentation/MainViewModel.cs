using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.ComponentModel.Windows.Input;
using Topics.Radical.Windows.Input;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.Boot;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Messaging;
using WpfRadicalMultipleReceiver.Messaging;

namespace WpfRadicalMultipleReceiver.Presentation
{
    public class MainViewModel : AbstractViewModel, IExpectViewLoadedCallback
    {
        private readonly IMessageBroker _broker;

        public IDelegateCommand OpenAnotherMasterViewCommand { get; protected set; }

        public IDelegateCommand ShutdownAppCommand { get; protected set; }

        public MainViewModel( IMessageBroker broker )
        {
            _broker = broker;

            broker.Subscribe<ApplicationShutdownRequested>( this, ( sender, msg ) =>
            {
                if ( msg.Reason == ApplicationShutdownReason.UserRequest )
                {
                    msg.Cancel = true;
                }
            } );

            this.OpenAnotherMasterViewCommand =
                DelegateCommand.Create()
                               .OnExecute( x => _broker.Broadcast( this, new LoadViewInShellRequest( typeof( MasterView ) ) ) );

            this.ShutdownAppCommand =
                DelegateCommand.Create()
                               .OnExecute( x => _broker.Broadcast( this, new ApplicationShutdownRequest( this ) ) );
        }

        public void OnViewLoaded()
        {
            _broker.Broadcast( this, new LoadViewInShellRequest( typeof( MasterView ) ) );
        }
    }
}
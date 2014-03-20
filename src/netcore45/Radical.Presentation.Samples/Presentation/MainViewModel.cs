using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Input;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Messaging;

namespace Radical.Presentation.Samples.Presentation
{
    class MainViewModel : AbstractViewModel,
        IExpectNavigatedToCallback,
        IExpectNavigatingAwayCallback
    {
        readonly INavigationService ns;
        readonly IMessageBroker broker;

        public MainViewModel( INavigationService ns, IMessageBroker broker )
        {
            this.ns = ns;
            this.broker = broker;

            this.broker.Subscribe<ApplicationSuspend>( this, ( sender, msg ) =>
            {
                msg.SuspentionManager.SetValue( "viewModelData", "hi, there", StorageLocation.Local );
            } );

            this.broker.Subscribe<ApplicationResumed>( this, ( sender, msg ) =>
            {
                var data = msg.SuspentionManager.GetValue<String>( "viewModelData" );
            } );

            this.GoToNextPage = DelegateCommand.Create()
                .OnExecute( o =>
                {
                    this.ns.Navigate( "/Basic/Foo/Bar", "hi there!" );
                } );

            this.Change = DelegateCommand.Create()
                .OnExecute( o =>
                {
                    this.Sample = "/Basic/Foo/Bar";
                } );

            this.SetInitialPropertyValue( () => this.Sample, "Hi, there!" );

            this.GetPropertyMetadata( () => this.IsOpen )
                .OnChanged( pvc =>
                {
                    this.AppBarText = DateTime.Now.Ticks.ToString();
                } );

            this.ManualOpen = DelegateCommand.Create()
                .OnExecute( o =>
                {
                    this.IsOpen = true;
                } );
        }

        public Boolean IsOpen
        {
            get { return this.GetPropertyValue( () => this.IsOpen ); }
            set { this.SetPropertyValue( () => this.IsOpen, value ); }
        }

        public ICommand ManualOpen { get; private set; }

        public String AppBarText
        {
            get { return this.GetPropertyValue( () => this.AppBarText ); }
            set { this.SetPropertyValue( () => this.AppBarText, value ); }
        }

        public String Sample
        {
            get { return this.GetPropertyValue( () => this.Sample ); }
            set { this.SetPropertyValue( () => this.Sample, value ); }
        }

        public ICommand Change { get; private set; }

        public ICommand GoToNextPage { get; private set; }

        void IExpectNavigatedToCallback.OnNavigatedTo( NavigationEventArgs e )
        {
            if ( e.Mode == NavigationMode.Resume && e.Storage.Contains( "myData" ) )
            {
                var data = e.Storage.GetData<String>( "myData" );
            }
        }

        void IExpectNavigatingAwayCallback.OnNavigatingAway( NavigatingAwayEventArgs e )
        {
            e.Storage.SetData( "myData", "this is the data", StorageLocation.Roaming );
        }
    }
}
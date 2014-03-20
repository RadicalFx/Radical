using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical;
using Topics.Radical.ComponentModel;
using Topics.Radical.Windows.Presentation.Boot;

namespace Topics.Radical.Windows.Presentation.Boot
{
    public class PuzzleApplicationBootstrapper : ApplicationBootstrapper
    {
        IPuzzleContainer container;

        public PuzzleApplicationBootstrapper()
        {
            
        }

        protected override IServiceProvider CreateServiceProvider()
        {
            this.container = new PuzzleContainer();
            var facade = new PuzzleContainerServiceProviderFacade( this.container );

            this.container.Register( EntryBuilder.For<ApplicationBootstrapper>()
                .UsingInstance( this ) );
            this.container.Register( EntryBuilder.For<IPuzzleContainer>()
                .UsingInstance( this.container ) );
            this.container.Register( EntryBuilder.For<IServiceProvider>()
                .UsingInstance( facade ) );
            this.container.Register( EntryBuilder.For<BootstrapConventions>()
                .UsingInstance( new BootstrapConventions() ) );

            //var dispatcher = System.Windows.Application.Current.RootVisual.Dispatcher;
            //this.container.Register(
            //        EntryBuilder.For<System.Windows.Threading.Dispatcher>()
            //            .UsingInstance( dispatcher )
            //);

            //this.container.AddFacility<SubscribeToMessageFacility>();

            return facade;
        }
    }
}

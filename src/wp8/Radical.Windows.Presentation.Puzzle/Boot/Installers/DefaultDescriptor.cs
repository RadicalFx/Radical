using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Linq;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
    public class DefaultDescriptor : IPuzzleSetupDescriptor
    {
        public void Setup( IPuzzleContainer container, Func<IEnumerable<Type>> knownTypesProvider )
        {
            var conventions = container.Resolve<BootstrapConventions>();
            var allTypes = knownTypesProvider();

            allTypes.Where( t => conventions.IsMessageHandler( t ) && !conventions.IsExcluded( t ) )
                .Select( t => new
                {
                    Contract = conventions.SelectMessageHandlerContracts( t ),
                    Implementation = t
                } )
                .ForEach( descriptor =>
                {
                    container.Register(
                        EntryBuilder.For( descriptor.Contract.First() )
                            .ImplementedBy( descriptor.Implementation )
                    );
                } );

            container.Register(
                EntryBuilder.For<Application>()
                    .UsingFactory( () => Application.Current )
                    .WithLifestyle( Lifestyle.Singleton )
            );

            container.Register(
                EntryBuilder.For<IMessageBroker>()
                    .ImplementedBy<MessageBroker>()
                    .WithLifestyle( Lifestyle.Singleton )
                //.Overridable()
            );
        }
    }
}

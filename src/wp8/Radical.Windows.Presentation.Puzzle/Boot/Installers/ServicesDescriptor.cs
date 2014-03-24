using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
    public class ServicesDescriptor : IPuzzleSetupDescriptor
    {
        public void Setup( IPuzzleContainer container, Func<IEnumerable<Type>> knownTypesProvider )
        {
            var conventions = container.Resolve<BootstrapConventions>();
            
            knownTypesProvider()
                .Where( t => conventions.IsService( t ) && !conventions.IsExcluded( t ) )
                .Select( t => new
                {
                    Contract = conventions.SelectServiceContracts( t ),
                    Implementation = t
                } )
                .ForEach( descriptor =>
                {
                    container.Register(
                        EntryBuilder.For( descriptor.Contract.First() )
                            .ImplementedBy( descriptor.Implementation )
                        //.Overridable()
                    );
                } );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
    public class PresentationDescriptor : IPuzzleSetupDescriptor
    {
        public void Setup( IPuzzleContainer container, System.Func<IEnumerable<Type>> knownTypesProvider )
        {
            var conventions = container.Resolve<BootstrapConventions>();
            var allTypes = knownTypesProvider();

            allTypes.Where( t => conventions.IsViewModel( t ) && !conventions.IsExcluded( t ) )
                .Select( t => new
                {
                    Contract = conventions.SelectViewModelContracts( t ),
                    Implementation = t,
                    Lifestyle = conventions.IsShellViewModel( new List<Type>() { t }, t ) ?
                        Lifestyle.Singleton :
                        Lifestyle.Transient
                } )
                .ForEach( descriptor =>
                {
                    container.Register(
                        EntryBuilder.For( descriptor.Contract.First() )
                            .ImplementedBy( descriptor.Implementation )
                            .WithLifestyle( descriptor.Lifestyle )
                    );
                } );

            allTypes.Where( t => conventions.IsView( t ) && !conventions.IsExcluded( t ) )
                .Select( t => new
                {
                    Contract = conventions.SelectViewContracts( t ),
                    Implementation = t,
                    Lifestyle = conventions.IsShellView( new List<Type>() { t }, t ) ?
                        Lifestyle.Singleton :
                        Lifestyle.Transient
                } )
                .ForEach( descriptor =>
                {
                    container.Register(
                        EntryBuilder.For( descriptor.Contract.First() )
                            .ImplementedBy( descriptor.Implementation )
                            .WithLifestyle( descriptor.Lifestyle )
                    );
                } );
        }
    }
}

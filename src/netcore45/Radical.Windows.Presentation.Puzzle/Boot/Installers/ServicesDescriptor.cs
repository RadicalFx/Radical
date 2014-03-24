using System;
using System.Linq;
using System.Collections.Generic;
using System.Composition;
using System.Reflection;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	[Export( typeof( IPuzzleSetupDescriptor ) )]
	public class ServicesDescriptor : IPuzzleSetupDescriptor
	{
		public async Task Setup( IPuzzleContainer container, Func<IEnumerable<TypeInfo>> knownTypesProvider )
		{
			await Task.Run( () =>
			{
				var conventions = container.Resolve<BootstrapConventions>();
				knownTypesProvider()
                    .Where( t => conventions.IsService( t ) && !conventions.IsExcluded( t ) )
					.Select( t => new
					{
						Contract = conventions.SelectServiceContract( t ),
						Implementation = t
					} )
					.ForEach( descriptor =>
					{
						container.Register( 
							EntryBuilder.For( descriptor.Contract )
								.ImplementedBy( descriptor.Implementation )
                                .Overridable()
						);
					} );
			} );
		}
	}
}
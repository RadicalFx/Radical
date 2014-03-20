using System.Linq;
using System.Composition;
using Topics.Radical.ComponentModel;
using Topics.Radical.Linq;
using System.Threading.Tasks;

namespace Topics.Radical.Windows.Presentation.Boot.Installers
{
	[Export( typeof( IPuzzleSetupDescriptor ) )]
	public class PresentationDescriptor : IPuzzleSetupDescriptor
	{
		public async Task Setup( IPuzzleContainer container, System.Func<System.Collections.Generic.IEnumerable<System.Reflection.TypeInfo>> knownTypesProvider )
		{
			await Task.Run( () =>
			{
				var conventions = container.Resolve<BootstrapConventions>();
				var allTypes = knownTypesProvider();

                allTypes.Where( t => conventions.IsViewModel( t ) && !conventions.IsExcluded( t ) )
					.Select( t => new
					{
						Contract = conventions.SelectViewModelContract( t ),
						Implementation = t,
						Lifestyle = conventions.IsShellViewModel( t ) ?
							Lifestyle.Singleton :
							Lifestyle.Transient
					} )
					.ForEach( descriptor =>
					{
						container.Register(
							EntryBuilder.For( descriptor.Contract )
								.ImplementedBy( descriptor.Implementation )
								.WithLifestyle( descriptor.Lifestyle )
						);
					} );

                allTypes.Where( t => conventions.IsView( t ) && !conventions.IsExcluded( t ) )
					.Select( t => new
					{
						Contract = conventions.SelectViewContract( t ),
						Implementation = t,
						Lifestyle = conventions.IsShellView( t ) ?
							Lifestyle.Singleton :
							Lifestyle.Transient
					} )
					.ForEach( descriptor =>
					{
						container.Register(
							EntryBuilder.For( descriptor.Contract )
								.ImplementedBy( descriptor.Implementation )
								.WithLifestyle( descriptor.Lifestyle ) 
						);
					} );
			} );
		}
	}
}
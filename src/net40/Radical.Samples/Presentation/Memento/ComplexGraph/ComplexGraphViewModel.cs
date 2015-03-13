using System.Windows.Input;
using System.Linq;
using Topics.Radical.ChangeTracking;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.Observers;
using Topics.Radical.Windows.Input;

namespace Topics.Radical.Presentation.Memento.ComplexGraph
{
	[Sample( Title = "Complex Graph", Category = Categories.Memento )]
	public sealed class ComplexGraphViewModel : SampleViewModel
	{
		readonly IChangeTrackingService service = new ChangeTrackingService();

		public ComplexGraphViewModel()
		{
			var observer = MementoObserver.Monitor( this.service );

			this.UndoCommand = DelegateCommand.Create()
				.OnCanExecute( o => this.service.CanUndo )
				.OnExecute( o => this.service.Undo() )
				.AddMonitor( observer );

			this.RedoCommand = DelegateCommand.Create()
				.OnCanExecute( o => this.service.CanRedo )
				.OnExecute( o => this.service.Redo() )
				.AddMonitor( observer );

			this.CreateNewAddressCommand = DelegateCommand.Create()
				.OnExecute( o =>
				{
					var address = this.Entity.Addresses.AddNew();
					this.SelectedAddress = address;
				} );

			this.DeleteAddressCommand = DelegateCommand.Create()
				.OnCanExecute( o => this.SelectedAddress != null )
				.OnExecute( o =>
				{
					this.SelectedAddress.Delete();
					this.SelectedAddress = this.Entity.Addresses.FirstOrDefault();
				} )
				.AddMonitor
				(
					PropertyObserver.For( this )
						.Observe( v => v.SelectedAddress )
				);

			var person = new Person()
			{
				FirstName = "Mauro",
				LastName = "Servienti"
			};

			person.Addresses.Add( new Address( person )
			{
				City = "Treviglio",
				Number = "11",
				Street = "Where I live",
				ZipCode = "20100"
			} );

			person.Addresses.Add( new Address( person )
			{
				City = "Daolasa",
				Number = "2",
				Street = "Pierino",
				ZipCode = "20100"
			} );

			var entity = new PersonViewModel();
			entity.Initialize( person, false );

			this.service.Attach( entity );

			this.Entity = entity;
		}

		public ICommand UndoCommand { get; private set; }

		public ICommand RedoCommand { get; private set; }

		public ICommand CreateNewAddressCommand { get; private set; }

		public ICommand DeleteAddressCommand { get; private set; }

		public PersonViewModel Entity
		{
			get { return this.GetPropertyValue( () => this.Entity ); }
			private set { this.SetPropertyValue( () => this.Entity, value ); }
		}

		public IEntityItemView<AddressViewModel> SelectedAddress
		{
			get { return this.GetPropertyValue( () => this.SelectedAddress ); }
			private set { this.SetPropertyValue( () => this.SelectedAddress, value ); }
		}
	}
}

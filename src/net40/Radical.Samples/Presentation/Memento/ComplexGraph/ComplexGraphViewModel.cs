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

		private PersonViewModel _entity = null;
		public PersonViewModel Entity
		{
			get { return this._entity; }
			private set
			{
				if( value != this.Entity )
				{
					this._entity = value;
					this.OnPropertyChanged( () => this.Entity );
				}
			}
		}

		public ICommand UndoCommand { get; private set; }

		public ICommand RedoCommand { get; private set; }

		public ICommand CreateNewAddressCommand { get; private set; }

		public ICommand DeleteAddressCommand { get; private set; }

		private IEntityItemView<AddressViewModel> _selectedAddress = null;
		public IEntityItemView<AddressViewModel> SelectedAddress
		{
			get { return this._selectedAddress; }
			set
			{
				if( value != this.SelectedAddress )
				{
					this._selectedAddress = value;
					this.OnPropertyChanged( () => this.SelectedAddress );
				}
			}
		}
	}
}

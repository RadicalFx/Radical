using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.Conversions;
using Topics.Radical.Model;

namespace Topics.Radical.Presentation.Memento.ComplexGraph
{
	public class PersonViewModel : MementoEntity
	{
		MementoEntityCollection<AddressViewModel> addressesDataSource;

		public void Initialize( Person person, Boolean registerAsTransient )
		{
			if( registerAsTransient )
			{
				this.RegisterTransient();
			}

			this.SetInitialPropertyValue( () => this.FirstName, person.FirstName );
			this.SetInitialPropertyValue( () => this.LastName, person.LastName );

			this.addressesDataSource = new MementoEntityCollection<AddressViewModel>();
			this.addressesDataSource.BulkLoad( person.Addresses, a =>
				{
					var vm = this.CreateAddressViewModel( a, registerAsTransient );
					return vm;
				} );

			this.Addresses = this.addressesDataSource.DefaultView;
			this.Addresses.AddingNew += ( s, e ) =>
			{
				var vm = this.CreateAddressViewModel( null, true );

				e.NewItem = vm;
				e.AutoCommit = true;
			};
		}

		AddressViewModel CreateAddressViewModel( Address a, Boolean registerAsTransient )
		{
			var vm = new AddressViewModel();

			vm.Initialize( a, registerAsTransient );

			return vm;
		}

		protected override void OnMementoChanged( ComponentModel.ChangeTracking.IChangeTrackingService newMemento, ComponentModel.ChangeTracking.IChangeTrackingService oldMemento )
		{
			base.OnMementoChanged( newMemento, oldMemento );

			if( oldMemento != null )
			{
				oldMemento.Detach( this.addressesDataSource );
			}

			if( newMemento != null )
			{
				newMemento.Attach( this.addressesDataSource );
			}
		}

		public String FirstName
		{
			get { return this.GetPropertyValue( () => this.FirstName ); }
			set { this.SetPropertyValue( () => this.FirstName, value ); }
		}

		public String LastName
		{
			get { return this.GetPropertyValue( () => this.LastName ); }
			set { this.SetPropertyValue( () => this.LastName, value ); }
		}

		public IEntityView<AddressViewModel> Addresses
		{
			get;
			private set;
		}
	}
}

using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.Conversions;
using Topics.Radical.Model;

namespace Topics.Radical.Presentation.Memento.ComplexGraph
{
	public class PersonViewModel : MementoEntity
	{
		private Person SourceEntity;

		public void Initialize( Person person, Boolean registerAsTransient )
		{
			this.SourceEntity = person;

			if( registerAsTransient )
			{
				this.RegisterTransient();
			}

			this.SetInitialPropertyValue( () => this.FirstName, person.FirstName );
			this.SetInitialPropertyValue( () => this.LastName, person.LastName );

			this.Addresses = new MementoEntityCollection<AddressViewModel>().DefaultView;
			this.Addresses.DataSource
				.CastTo<IEntityCollection<AddressViewModel>>()
				.BulkLoad( this.SourceEntity.Addresses, a =>
				{
					var vm = this.CreateAddressViewModel( a, registerAsTransient );
					return vm;
				} );

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

			vm.ParentViewModel = this;
			vm.Parent = this.SourceEntity;

			vm.Initialize( a, registerAsTransient );

			return vm;
		}

		protected override void OnMementoChanged( ComponentModel.ChangeTracking.IChangeTrackingService newMemento, ComponentModel.ChangeTracking.IChangeTrackingService oldMemmento )
		{
			base.OnMementoChanged( newMemento, oldMemmento );

			var im = ( IMemento )this.Addresses.DataSource;

			if( oldMemmento != null )
			{
				oldMemmento.Detach( im );
			}

			if( newMemento != null )
			{
				newMemento.Attach( im );
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

using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.ChangeTracking;
using Topics.Radical.Model;

namespace Topics.Radical.Presentation.Memento.SimpleGraph
{
	public class PersonViewModel : MementoEntity
	{
		public void Initialize( Person person, Boolean registerAsTransient )
		{
			if( registerAsTransient )
			{
				this.RegisterTransient();
			}

			this.SetInitialPropertyValue( () => this.FirstName, person.FirstName );
			this.SetInitialPropertyValue( () => this.LastName, person.LastName );



			//var bookmark = ( ( IMemento )this ).Memento.CreateBookmark();


			//( ( IMemento )this ).Memento.Revert( bookmark );

			//( ( IMemento )this ).Memento.AcceptChanges();

			//using ( var op = ( ( IMemento )this ).Memento.BeginAtomicOperation() ) 
			//{
			//	this.FirstName = "Mauro";
			//	this.LastName = "Servienti";
			//}

			//( ( IMemento )this ).Memento.Undo();

			//this.Addresses = new MementoEntityCollection<AddressViewModel>();
			//this.Addresses.AddingNew += ( s, e ) =>
			//{
			//    var vm = this.entityViewModelFactory.Create<IPublicationViewModel>();

			//    vm.ParentViewModel = this;
			//    vm.Parent = this.SourceEntity;

			//    vm.Initialize( null, true );

			//    e.NewItem = vm;
			//    e.AutoCommit = true;
			//};

			//if( sourceEntity == null )
			//{
			//    this.SetInitialPropertyValue( () => this.Year, DateTime.Now.Year );
			//    this.SetInitialPropertyValue( () => this.CalendarName, "Calendario pubblicazioni" );

			//    //Qui dobbiamo anche generare le uscite di default
			//    this.InitializeCalendar();
			//}
			//else
			//{
			//    this.SetInitialPropertyValue( () => this.Year, sourceEntity.Year );
			//    this.SetInitialPropertyValue( () => this.CalendarName, sourceEntity.CalendarName );

			//    this.Publications.DataSource
			//        .CastTo<IEntityCollection<IPublicationViewModel>>()
			//        .BulkLoad( sourceEntity.Publications, si =>
			//        {
			//            var vm = this.entityViewModelFactory.Create<IPublicationViewModel>();

			//            vm.ParentViewModel = this;
			//            vm.Parent = this.SourceEntity;

			//            vm.Initialize( si, registerAsTransient );

			//            return vm;
			//        } );
			//}






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

		//public IEntityView<AddressViewModel> Addresses
		//{
		//    get;
		//    private set;
		//}
	}
}

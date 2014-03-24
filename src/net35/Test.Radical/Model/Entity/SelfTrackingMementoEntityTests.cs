//extern alias tpx;

namespace Test.Radical.Model.Entity
{
	using System;
	using System.Linq.Expressions;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using SharpTestsEx;
	using Topics.Radical.Model;
	using Topics.Radical.ChangeTracking;
	using Topics.Radical.ComponentModel.ChangeTracking;

	public class MementoMockEntity : Topics.Radical.Model.MementoEntity, IMockEntity
	{
		public MementoMockEntity()
		{
			this.OnInitialize();
		}

		public MementoMockEntity( String firstName )
		{
			this.SetInitialPropertyValue( () => this.FirstName, firstName );

			this.OnInitialize();
		}

		void OnInitialize()
		{
			var firstNameMetadata = this.GetPropertyMetadata<String>( "FirstName" );
			( ( MementoPropertyMetadata<String> )firstNameMetadata ).EnableChangesTracking();

			//this.SetPropertyMetadata( new MementoPropertyMetadata<String>( () => this.FirstName ) { TrackChanges = true } );

			var metadata = new PropertyMetadata<String>( this, () => this.MainProperty );
			metadata.AddCascadeChangeNotifications( () => this.SubProperty );

			this.SetPropertyMetadata( metadata );
		}

		public void SetInitialValue<T>( Expression<Func<T>> property, T value )
		{
			this.SetInitialPropertyValue( property, value );
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

		public Int32 Number
		{
			get { return this.GetPropertyValue( () => this.Number ); }
			set { this.SetPropertyValue( () => this.Number, value ); }
		}


		public string MainProperty
		{
			get { return this.GetPropertyValue( () => this.MainProperty ); }
			set { this.SetPropertyValue( () => this.MainProperty, value ); }
		}

		public string SubProperty
		{
			get { return this.GetPropertyValue( () => this.SubProperty ); }
		}
	}

	[TestClass]
	public class SelfTrackingMementoEntityTests : SelfTrackingEntityTests
	{
		protected override Topics.Radical.Model.Entity CreateMock()
		{
			return new MementoMockEntity();
		}

		protected override Topics.Radical.Model.Entity CreateMock( string firstName )
		{
			return new MementoMockEntity( firstName );
		}

		[TestMethod]
		public void mementoEntity_using_trackingService_should_undo_a_single_change()
		{
			var memento = new ChangeTrackingService();

			var target = ( IMockEntity )this.CreateMock();
			( ( IMemento )target ).Memento = memento;

			target.FirstName = "Mauro";

			memento.Undo();

			target.FirstName.Should().Be.Null();
		}

		[TestMethod]
		public void mementoEntity_using_trackingService_should_undo_and_redo_a_single_change()
		{
			var expected = "Mauro";

			var memento = new ChangeTrackingService();

			var target = ( IMockEntity )this.CreateMock();
			( ( IMemento )target ).Memento = memento;

			target.FirstName = expected;

			memento.Undo();
			memento.Redo();

			target.FirstName.Should().Be.EqualTo( expected );
		}
	}
}

//extern alias tpx;

namespace Radical.Tests.Model.Entity
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    using Radical.Model;
    using SharpTestsEx;
    using System;
    using System.Linq.Expressions;

    public class MementoMockEntity : MementoEntity, IMockEntity
    {
        public MementoMockEntity()
        {
            this.OnInitialize();
        }

        public MementoMockEntity(string firstName)
        {
            this.SetInitialPropertyValue(() => this.FirstName, firstName);

            this.OnInitialize();
        }

        void OnInitialize()
        {
            var firstNameMetadata = this.GetPropertyMetadata<string>("FirstName");
            ((MementoPropertyMetadata<string>)firstNameMetadata).EnableChangesTracking();

            //this.SetPropertyMetadata( new MementoPropertyMetadata<string>( () => this.FirstName ) { TrackChanges = true } );

            var metadata = new PropertyMetadata<string>(this, () => this.MainProperty);
            metadata.AddCascadeChangeNotifications(() => this.SubProperty);

            this.SetPropertyMetadata(metadata);
        }

        public void SetInitialValue<T>(Expression<Func<T>> property, T value)
        {
            this.SetInitialPropertyValue(property, value);
        }

        public string FirstName
        {
            get { return this.GetPropertyValue(() => this.FirstName); }
            set { this.SetPropertyValue(() => this.FirstName, value); }
        }

        public string LastName
        {
            get { return this.GetPropertyValue(() => this.LastName); }
            set { this.SetPropertyValue(() => this.LastName, value); }
        }

        public int Number
        {
            get { return this.GetPropertyValue(() => this.Number); }
            set { this.SetPropertyValue(() => this.Number, value); }
        }


        public string MainProperty
        {
            get { return this.GetPropertyValue(() => this.MainProperty); }
            set { this.SetPropertyValue(() => this.MainProperty, value); }
        }

        public string SubProperty
        {
            get { return this.GetPropertyValue(() => this.SubProperty); }
        }
    }

    [TestClass]
    public class SelfTrackingMementoEntityTests : SelfTrackingEntityTests
    {
        [TestMethod]
        public void mementoEntity_using_trackingService_should_undo_a_single_change()
        {
            var memento = new ChangeTrackingService();

            var target = new MementoMockEntity();
            ((IMemento)target).Memento = memento;

            target.FirstName = "Mauro";

            memento.Undo();

            target.FirstName.Should().Be.Null();
        }

        [TestMethod]
        public void mementoEntity_using_trackingService_should_undo_and_redo_a_single_change()
        {
            var expected = "Mauro";

            var memento = new ChangeTrackingService();

            var target = new MementoMockEntity();
            ((IMemento)target).Memento = memento;

            target.FirstName = expected;

            memento.Undo();
            memento.Redo();

            target.FirstName.Should().Be.EqualTo(expected);
        }
    }
}

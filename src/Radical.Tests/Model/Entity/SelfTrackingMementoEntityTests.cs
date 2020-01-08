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
            OnInitialize();
        }

        public MementoMockEntity(string firstName)
        {
            SetInitialPropertyValue(() => FirstName, firstName);

            OnInitialize();
        }

        void OnInitialize()
        {
            var firstNameMetadata = GetPropertyMetadata<string>("FirstName");
            ((MementoPropertyMetadata<string>)firstNameMetadata).EnableChangesTracking();

            //this.SetPropertyMetadata( new MementoPropertyMetadata<string>( () => this.FirstName ) { TrackChanges = true } );

            var metadata = new PropertyMetadata<string>(this, () => MainProperty);
            metadata.AddCascadeChangeNotifications(() => SubProperty);

            SetPropertyMetadata(metadata);
        }

        public void SetInitialValue<T>(Expression<Func<T>> property, T value)
        {
            SetInitialPropertyValue(property, value);
        }

        public string FirstName
        {
            get { return GetPropertyValue(() => FirstName); }
            set { SetPropertyValue(() => FirstName, value); }
        }

        public string LastName
        {
            get { return GetPropertyValue(() => LastName); }
            set { SetPropertyValue(() => LastName, value); }
        }

        public int Number
        {
            get { return GetPropertyValue(() => Number); }
            set { SetPropertyValue(() => Number, value); }
        }


        public string MainProperty
        {
            get { return GetPropertyValue(() => MainProperty); }
            set { SetPropertyValue(() => MainProperty, value); }
        }

        public string SubProperty
        {
            get { return GetPropertyValue(() => SubProperty); }
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

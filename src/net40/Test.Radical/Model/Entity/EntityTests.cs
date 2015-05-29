//extern alias tpx;

namespace Test.Radical.Model.Entity
{
    using Topics.Radical.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Topics.Radical.ComponentModel;
    using Rhino.Mocks;
    using Topics.Radical.ComponentModel.ChangeTracking;
    using System.ComponentModel;
    using System;
    using System.Linq.Expressions;

    [TestClass()]
    public class EntityTests
    {
        class TestMetadata<T> : PropertyMetadata<T>
        {
            public TestMetadata( Object propertyOwner, String propertyName )
                : base( propertyOwner, propertyName )
            {

            }

            public Boolean IsDisposed { get; private set; }

            protected override void Dispose( bool disposing )
            {
                base.Dispose( disposing );

                this.IsDisposed = true;
            }
        }

        class SutEntity : Entity
        {
            public TestMetadata<T> For<T>( Expression<Func<T>> property )
            {
                return ( TestMetadata<T> )this.GetPropertyMetadata<T>( property );
            }

            protected override PropertyMetadata<T> GetDefaultMetadata<T>( string propertyName )
            {
                return new TestMetadata<T>( this, propertyName );
            }

            public String MyProperty
            {
                get { return this.GetPropertyValue( () => this.MyProperty ); }
                set { this.SetPropertyValue( () => this.MyProperty, value ); }
            }
        }

        protected virtual Entity CreateMock()
        {
            MockRepository mocks = new MockRepository();
            var entity = mocks.PartialMock<Entity>();
            entity.Replay();

            return entity;
        }

        [TestMethod]
        public void entity_propertyChanged_subscription_should_not_fail()
        {
            var target = this.CreateMock();
            target.PropertyChanged += ( s, e ) => { };
        }

        [TestMethod]
        public void entity_propertyChanged_unsubscription_should_not_fail()
        {
            PropertyChangedEventHandler h = ( s, e ) => { };
            var target = this.CreateMock();
            target.PropertyChanged += h;
            target.PropertyChanged -= h;
        }

        [TestMethod]
        public void entity_dispose_normal_should_not_fail()
        {
            using( var target = this.CreateMock() )
            {

            }
        }

        [TestMethod]
        public void entity_dispose_multiple_calls_should_not_fail()
        {
            using( var target = this.CreateMock() )
            {
                target.Dispose();
                target.Dispose();
            }
        }

        [TestMethod]
        public void entity_dispose_using_metadata_should_not_fail()
        {
            TestMetadata<String> metadata;

            using( var target = new SutEntity() )
            {
                target.MyProperty = "Sample";

                metadata = target.For( () => target.MyProperty );
            }

            Assert.IsTrue( metadata.IsDisposed );
        }

        [TestMethod]
        public void entity_non_disposed_should_not_dispose_metadata()
        {
            var target = new SutEntity();
            target.MyProperty = "Sample";
            var metadata = target.For( () => target.MyProperty );

            Assert.IsFalse( metadata.IsDisposed );
        }

        [TestMethod]
        public void entity_dispose_with_events_subscribed_should_dispose_eventHandlerList()
        {
            using( var target = this.CreateMock() )
            {
                target.PropertyChanged += ( s, e ) => { };
            }
        }
    }
}

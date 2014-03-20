//extern alias tpx;

namespace Test.Radical.Model.Entity
{
	using Topics.Radical.Model;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Topics.Radical.ComponentModel;
	using Rhino.Mocks;
	using Topics.Radical.ComponentModel.ChangeTracking;
	using System.ComponentModel;

	[TestClass()]
	public class EntityTests
	{
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
		public void entity_dispose_with_events_subscribed_should_dispose_eventHandlerList()
		{
			using( var target = this.CreateMock() )
			{
				target.PropertyChanged += ( s, e ) => { };
			}
		}
	}
}

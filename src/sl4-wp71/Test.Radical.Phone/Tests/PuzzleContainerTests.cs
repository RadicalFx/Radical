using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical;
using Topics.Radical.ComponentModel;
using System.Collections.Generic;

namespace Test.Radical.Phone.Tests
{
	[TestClass]
	public class PuzzleContainerTests : SilverlightTest
	{
		class MyFacility : IPuzzleContainerFacility
		{
			public Boolean InitializeCalled { get; set; }
			public Boolean TeardownCalled { get; set; }

			public void Initialize( IPuzzleContainer container )
			{
				this.InitializeCalled = true;
			}

			public void Teardown( IPuzzleContainer container )
			{
				this.TeardownCalled = true;
			}
		}

		[TestMethod]
		public void puzzleContainer_addFacility_using_generic_method_and_valid_facility_should_behave_as_expected()
		{
			var container = new PuzzleContainer();
			container.AddFacility<MyFacility>();
			var facility = container.GetFacilities()
				.OfType<MyFacility>()
				.SingleOrDefault();

			Assert.IsNotNull( facility );
		}

		[TestMethod]
		public void puzzleContainer_addFacility_using_generic_method_and_valid_facility_should_initialize_the_new_facility()
		{
			var container = new PuzzleContainer();
			container.AddFacility<MyFacility>();
			var facility = container.GetFacilities()
				.OfType<MyFacility>()
				.Single();

			Assert.IsTrue( facility.InitializeCalled );
		}

		[TestMethod]
		public void puzzleContainer_on_dispose_should_teardown_facilities()
		{
			MyFacility facility;

			using( var container = new PuzzleContainer() )
			{
				container.AddFacility<MyFacility>();
				facility = container.GetFacilities()
					.OfType<MyFacility>()
					.Single();
			}

			Assert.IsTrue( facility.TeardownCalled );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void puzzleContainer_addFacility_using_null_reference_should_raise_ArgumentNullException()
		{
			var container = new PuzzleContainer();
			container.AddFacility( null );
		}

		[TestMethod]
		public void puzzleContainer_addFacility_using_non_generic_method_and_valid_facility_should_behave_as_expected()
		{
			var container = new PuzzleContainer();

			var facility = new MyFacility();
			container.AddFacility( facility );
		}

		[TestMethod]
		public void puzzleContainer_addFacility_using_non_generic_method_and_valid_facility_should_initialize_the_new_facility()
		{
			var container = new PuzzleContainer();

			var facility = new MyFacility();
			container.AddFacility( facility );

			Assert.IsTrue( facility.InitializeCalled );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_using_valid_entry_should_not_fail()
		{
			var container = new PuzzleContainer();
			container.Register( EntryBuilder.For<String>() );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void puzzleContainer_register_entry_using_null_reference_should_raise_ArgumentNullException()
		{
			var container = new PuzzleContainer();
			container.Register( ( IContainerEntry )null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void puzzleContainer_register_entries_using_null_reference_should_raise_ArgumentNullException()
		{
			var container = new PuzzleContainer();
			container.Register( ( IEnumerable<IContainerEntry> )null );
		}

		[TestMethod]
		public void puzzleContainer_register_entries_using_valid_entry_should_resolve_the_given_entries()
		{
			var container = new PuzzleContainer();

			IContainerEntry e1 = EntryBuilder.For<ArgumentException>();
			IContainerEntry e2 = EntryBuilder.For<ArgumentNullException>();

			container.Register( new[] { e1, e2 } );
			var obj1 = container.Resolve<ArgumentException>();
			var obj2 = container.Resolve<ArgumentNullException>();

			Assert.IsNotNull( obj1 );
			Assert.IsNotNull( obj2 );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_using_instance_should_resolve_the_given_instance()
		{
			var expected = new Object();

			var container = new PuzzleContainer();

			container.Register( EntryBuilder.For<Object>().UsingInstance( expected ) );
			var actual = container.Resolve<Object>();

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_should_report_entry_as_registered()
		{
			var container = new PuzzleContainer();

			container.Register( EntryBuilder.For<Object>() );
			var actual = container.IsRegistered<Object>();

			Assert.IsTrue( actual );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_as_transient_should_resolve_instances_as_transient()
		{
			var container = new PuzzleContainer();

			container.Register( EntryBuilder.For<Object>().WithLifestyle( Lifestyle.Transient ) );
			var i1 = container.Resolve<Object>();
			var i2 = container.Resolve<Object>();

			Assert.AreNotEqual( i1, i2 );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_with_factory_as_transient_should_resolve_instances_as_transient()
		{
			var expected = 2;
			var actual = 0;
			var container = new PuzzleContainer();

			container.Register
			(
				EntryBuilder.For<Object>()
					.UsingFactory( () =>
					{
						actual++;
						return new Object();
					} )
					.WithLifestyle( Lifestyle.Transient )
			);

			container.Resolve<Object>();
			container.Resolve<Object>();

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_with_factory_as_singleton_should_call_factory_once()
		{
			var expected = 1;
			var actual = 0;
			var container = new PuzzleContainer();

			container.Register
			(
				EntryBuilder.For<Object>()
					.UsingFactory( () =>
					{
						actual++;
						return new Object();
					} )
			);

			container.Resolve<Object>();
			container.Resolve<Object>();

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		public void puzzleContainer_isRegistered_using_non_registered_type_shuold_return_false()
		{
			var container = new PuzzleContainer();

			var actual = container.IsRegistered<Int32>();

			Assert.IsFalse( actual );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_using_instance_should_resolve_the_given_instance_as_singleton()
		{
			var container = new PuzzleContainer();

			container.Register( EntryBuilder.For<Object>().UsingInstance( new Object() ) );
			var expected = container.Resolve<Object>();
			var actual = container.Resolve<Object>();

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_using_instance_should_resolve_the_given_instance_using_getService()
		{
			var expected = new Object();

			var container = new PuzzleContainer();

			container.Register( EntryBuilder.For<Object>().UsingInstance( expected ) );
			var actual = container.GetService( typeof( Object ) );

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		public void puzzleContainer_register_entry_using_instance_should_resolve_the_given_instance_using_getService_as_singleton()
		{
			var container = new PuzzleContainer();

			container.Register( EntryBuilder.For<Object>().UsingInstance( new Object() ) );
			var expected = container.GetService( typeof( Object ) );
			var actual = container.GetService( typeof( Object ) );

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentException ) )]
		public void puzzleContainer_resolve_using_non_registered_type_should_raise_ArgumentException()
		{
			var container = new PuzzleContainer();
			container.Resolve<Object>();
		}

		[TestMethod]
		public void puzzleContainer_getService_using_non_registered_type_should_return_null()
		{
			var container = new PuzzleContainer();
			var actual = container.GetService( typeof( Object ) );

			Assert.IsNull( actual );
		}

		[TestMethod]
		public void puzzleContainer_should_automatically_register_itself_as_iServiceProvider()
		{
			var container = new PuzzleContainer();
			var actual = container.Resolve<IServiceProvider>();

			Assert.IsNotNull( actual );
		}

		[TestMethod]
		public void puzzleContainer_register_should_raise_componentRegistered_event()
		{
			var expected = 1;
			var actual = 0;

			var container = new PuzzleContainer();
			container.ComponentRegistered += ( s, e ) => actual++;

			container.Register( EntryBuilder.For<Object>() );

			Assert.AreEqual( expected, actual );
		}

		[TestMethod]
		public void puzzleContainer_register_should_raise_componentRegistered_event_with_a_reference_to_the_registered_entry()
		{
			IContainerEntry expected = EntryBuilder.For<Object>();
			IContainerEntry actual = null;

			var container = new PuzzleContainer();
			container.ComponentRegistered += ( s, e ) => actual = e.Entry;

			container.Register( expected );

			Assert.AreEqual( expected, actual );
		}
	}
}
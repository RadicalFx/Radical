//extern alias tpx;

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpTestsEx;
using Topics.Radical.Model;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using Test.Radical.Model.EntityCollection;
using Topics.Radical.ComponentModel;

namespace Test.Radical.Model
{
	[TestClass]
	public class EntityCollectionTests
	{
		protected virtual EntityCollection<T> CreateMock<T>() where T : class
		{
			return new EntityCollection<T>();
		}

		protected virtual EntityCollection<T> CreateMock<T>( int capacity ) where T : class
		{
			return new EntityCollection<T>( capacity );
		}

		protected virtual EntityCollection<T> CreateMock<T>( IEnumerable<T> list ) where T : class
		{
			return new EntityCollection<T>( list );
		}

		[TestMethod]
		public void entityCollection_ctor_default_should_initialize_expected_values()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			target.Count.Should().Be.EqualTo( 0 );
			target.AllowNew.Should().Be.True();
		}

		[TestMethod]
		public void entityCollection_ctor_capacity_should_initialize_expected_values()
		{
			var target = this.CreateMock<GenericParameterHelper>( 10 );

			target.Count.Should().Be.EqualTo( 0 );
			target.AllowNew.Should().Be.True();
		}

		[TestMethod]
		public void entityCollection_ctor_iEnumerable_should_initialize_expected_values()
		{
			var target = this.CreateMock<GenericParameterHelper>( new GenericParameterHelper[ 0 ] );

			target.Count.Should().Be.EqualTo( 0 );
			target.AllowNew.Should().Be.True();
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void entityCollection_ctor_iEnumerable_null_list_should_raise_ArgumentNullException()
		{
			this.CreateMock( ( IEnumerable<GenericParameterHelper> )null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void entityCollection_ctor_int_less_then_zero_should_raise_ArgumentNullException()
		{
			this.CreateMock<GenericParameterHelper>( -1 );
		}

		[TestMethod]
		public void entityCollection_add_normal_should_increment_count_property()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper() );

			target.Count.Should().Be.EqualTo( 1 );
		}

		[TestMethod]
		public void entityCollection_add_null_value_should_increment_count_property()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( null );

			target.Count.Should().Be.EqualTo( 1 );
		}

		[TestMethod]
		public void entityCollection_add_more_then_once_the_same_reference_works_as_expected()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			GenericParameterHelper item = new GenericParameterHelper();
			target.Add( item );
			target.Add( item );
			target.Add( item );

			target.Count.Should().Be.EqualTo( 3 );
		}

		[TestMethod]
		public void entityCollection_add_more_then_one_different_reference_works_as_expected()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			target.Add( new GenericParameterHelper() );
			target.Add( new GenericParameterHelper() );
			target.Add( new GenericParameterHelper() );

			target.Count.Should().Be.EqualTo( 3 );
		}

		[TestMethod]
		public void entityCollection_remove_normal_removes_the_item_from_the_collection()
		{
			var item = new GenericParameterHelper( 2 );
			var data = new GenericParameterHelper[]
			{
				new GenericParameterHelper(0),
				new GenericParameterHelper(1),
				item,
				new GenericParameterHelper(3)
			};

			var target = this.CreateMock( data );

			var actual = target.Remove( item );

			actual.Should().Be.True();
			target.Count.Should().Be.EqualTo( 3 );
			target.IndexOf( item ).Should().Be.EqualTo( -1 );
		}

		[TestMethod]
		public void entityCollection_remove_null_reference_should_return_false()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			var actual = target.Remove( null );

			actual.Should().Be.False();
		}

		[TestMethod]
		public void entityCollection_remove_a_reference_not_in_the_collection_should_return_false()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			var actual = target.Remove( new GenericParameterHelper() );

			actual.Should().Be.False();
		}

		[TestMethod]
		public void entityCollection_serialization_normal_should_work_as_expected()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			using( var ms = new MemoryStream() )
			{
				var formatter = new BinaryFormatter();

				var surrogateSelector = new SurrogateSelector();

				surrogateSelector.AddSurrogate(
					typeof( GenericParameterHelper ),
					new StreamingContext( StreamingContextStates.All ),
					new GenericParameterHelperSerializationSurrogate() );

				formatter.SurrogateSelector = surrogateSelector;
				formatter.Serialize( ms, target );
			}
		}

		[TestMethod]
		public void entityCollection_deserialization_normal_should_deserialize_the_same_structure()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			using( var ms = new MemoryStream() )
			{
				var formatter = new BinaryFormatter();

				var surrogateSelector = new SurrogateSelector();

				surrogateSelector.AddSurrogate(
					typeof( GenericParameterHelper ),
					new StreamingContext( StreamingContextStates.All ),
					new GenericParameterHelperSerializationSurrogate() );

				formatter.SurrogateSelector = surrogateSelector;
				formatter.Serialize( ms, target );

				ms.Position = 0;

				var actual = formatter.Deserialize( ms ) as EntityCollection<GenericParameterHelper>;

				target.Should().Have.SameSequenceAs( actual );
			}
		}

		[TestMethod]
		public void entityCollection_normal_isInitializing_should_be_false()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.IsInitializing.Should().Be.False();
		}

		[TestMethod]
		public void entityCollection_beginInit_normal_should_set_isInitializing_to_true()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.BeginInit();
			target.IsInitializing.Should().Be.True();
		}

		[TestMethod]
		public void entityCollection_endInit_normal_should_set_isInitializing_to_false()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.BeginInit();
			target.EndInit();
			target.IsInitializing.Should().Be.False();
		}

		[TestMethod]
		public void entityCollection_endInit_without_calling_beginInit_should_not_fail_and_set_isInitializing_to_false()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.EndInit();
			target.IsInitializing.Should().Be.False();
		}

		[TestMethod]
		public void entityCollection_endInit_normal_should_raise_collectionChanged_event()
		{
			var expected = 1;
			var actual = 0;

			var target = this.CreateMock<GenericParameterHelper>();
			target.CollectionChanged += ( s, e ) => { actual++; };

			target.BeginInit();
			target.EndInit();

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void entityCollection_endInit_without_calling_beginInit_should_not_raise_collectionChanged_event()
		{
			var expected = 0;
			var actual = 0;

			var target = this.CreateMock<GenericParameterHelper>();
			target.CollectionChanged += ( s, e ) => { actual++; };

			target.EndInit();

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void entityCollection_endInit_false_should_not_raise_collectionChanged_event()
		{
			var expected = 0;
			var actual = 0;

			var target = this.CreateMock<GenericParameterHelper>();
			target.CollectionChanged += ( s, e ) => { actual++; };

			target.BeginInit();
			target.EndInit( false );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void entityCollection_endInit_true_should_raise_collectionChanged_event()
		{
			var expected = 1;
			var actual = 0;

			var target = this.CreateMock<GenericParameterHelper>();
			target.CollectionChanged += ( s, e ) => { actual++; };

			target.BeginInit();
			target.EndInit( true );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void entityCollection_bulkLoad_should_not_raise_collectionChanged_event()
		{
			var expected = 1;
			var actual = 0;

			var target = this.CreateMock<GenericParameterHelper>();
			target.CollectionChanged += ( s, e ) => { actual++; };

			target.BeginInit();

			target.Add( new GenericParameterHelper() );
			target.Add( new GenericParameterHelper() );
			target.Add( new GenericParameterHelper() );
			target.Add( new GenericParameterHelper() );

			target.EndInit( true );

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		public void entityCollection_set_Item_normal_should_set_correct_item()
		{
			var expected = new GenericParameterHelper( 0 );

			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );
			target.Add( new GenericParameterHelper( 3 ) );

			target[ 1 ] = expected;
			var actual = target[ 1 ];

			actual.Should().Be.EqualTo( expected );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void entityCollection_set_Item_invalid_index_should_raise_ArgumentOutOfRangeException()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			target[ 2 ] = new GenericParameterHelper();
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void entityCollection_get_Item_invalid_index_should_raise_ArgumentOutOfRangeException()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			var actual = target[ 2 ];
		}

		[TestMethod]
		public void entityCollection_add_handler_to_collectionChanged_normal_should_work_as_expected()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.CollectionChanged += ( s, e ) => { };
		}

		[TestMethod]
		public void entityCollection_revove_handler_to_collectionChanged_normal_should_work_as_expected()
		{
			EventHandler<CollectionChangedEventArgs<GenericParameterHelper>> h = ( s, e ) => { };

			var target = this.CreateMock<GenericParameterHelper>();
			target.CollectionChanged += h;
			target.CollectionChanged -= h;
		}

		[TestMethod]
		public void entityCollection_revove_handler_to_collectionChanged_even_if_add_handler_has_not_been_called_should_work_as_expected()
		{
			EventHandler<CollectionChangedEventArgs<GenericParameterHelper>> h = ( s, e ) => { };

			var target = this.CreateMock<GenericParameterHelper>();
			target.CollectionChanged -= h;
		}

		[TestMethod]
		public void entityCollection_addRange_normal_should_add_items()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			var range = new[] 
			{ 
				new GenericParameterHelper( 0 ), 
				new GenericParameterHelper( 1 ), 
				new GenericParameterHelper( 2 ) 
			};

			target.AddRange( range );

			target.Should().Have.SameSequenceAs( range );
		}

		[TestMethod]
		public void entityCollection_addRange_normal_should_increment_count()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			var range = new[] 
			{ 
				new GenericParameterHelper( 0 ), 
				new GenericParameterHelper( 1 ), 
				new GenericParameterHelper( 2 ) 
			};

			target.AddRange( range );

			target.Count.Should().Be.EqualTo( 3 );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void entityCollection_addRange_null_reference_should_raise_ArgumentNullException()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			target.AddRange( null );
		}

		[TestMethod]
		public void entityCollection_clear_normal_should_empty_the_collection()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			target.Clear();
			target.Count.Should().Be.EqualTo( 0 );
		}

		[TestMethod]
		public void entityCollection_clear_empty_collection_should_behave_as_expected()
		{
			var target = this.CreateMock<GenericParameterHelper>();

			target.Clear();
			target.Count.Should().Be.EqualTo( 0 );
		}

		[TestMethod]
		public void entityCollection_copyTo_array_normal_should_copy_elements_to_given_array()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			var destination = new GenericParameterHelper[ 3 ];
			target.CopyTo( destination );

			destination.Should().Have.SameSequenceAs( target );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void entityCollection_copyTo_array_null_reference_destination_array_should_raise_ArgumentNullException()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			target.CopyTo( null );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentException ) )]
		public void entityCollection_copyTo_array_destination_array_smaller_then_source_should_raise_ArgumentException()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			var destination = new GenericParameterHelper[ 1 ];
			target.CopyTo( destination );
		}

		[TestMethod]
		public void entityCollection_copyTo_array_destination_array_greater_then_source_should_copy_elements_to_given_array()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			var destination = new GenericParameterHelper[ 5 ];
			target.CopyTo( destination );

			target[ 0 ].Should().Be.EqualTo( destination[ 0 ] );
			target[ 1 ].Should().Be.EqualTo( destination[ 1 ] );
			target[ 2 ].Should().Be.EqualTo( destination[ 2 ] );
		}

		[TestMethod]
		public void entityCollection_copyTo_array_index_should_copy_elements_starting_at_the_given_index()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			target.Add( new GenericParameterHelper( 0 ) );
			target.Add( new GenericParameterHelper( 1 ) );
			target.Add( new GenericParameterHelper( 2 ) );

			var destination = new GenericParameterHelper[ 5 ];
			target.CopyTo( destination, 2 );

			destination[ 0 ].Should().Be.Null();
			destination[ 1 ].Should().Be.Null();
			destination[ 2 ].Should().Be.EqualTo( target[ 0 ] );
			destination[ 3 ].Should().Be.EqualTo( target[ 1 ] );
			destination[ 4 ].Should().Be.EqualTo( target[ 2 ] );
		}

		[TestMethod]
		public void entityCollection_createView_normal_should_create_non_null_view()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			var actual = target.CreateView();

			actual.Should().Not.Be.Null();
		}

		[TestMethod]
		public void entityCollection_createView_normal_should_create_different_views()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			var first = target.CreateView();
			var second = target.CreateView();

			first.Should().Not.Be.Equals( second );
		}

		[TestMethod]
		public void entityCollection_defaultView_normal_should_return_non_null_view()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			var actual = target.DefaultView;

			actual.Should().Not.Be.Null();
		}

		[TestMethod]
		public void entityCollection_defaultView_normal_should_return_singleton_view()
		{
			var target = this.CreateMock<GenericParameterHelper>();
			var first = target.DefaultView;
			var second = target.DefaultView;

			first.Should().Be.Equals( second );
		}
	}
}
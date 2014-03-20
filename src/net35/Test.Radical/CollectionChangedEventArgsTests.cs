using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Topics.Radical.ComponentModel;
using SharpTestsEx;

namespace Test.Radical
{
	[TestClass]
	public class CollectionChangedEventArgsTests
	{
		[TestMethod]
		public void collectionChangedEventArgs_ctor_changeType_should_set_default_values()
		{
			var cType = CollectionChangeType.Reset;

			var target = new CollectionChangedEventArgs<Object>( cType );

			target.ChangeType.Should().Be.EqualTo( cType );
			target.Index.Should().Be.EqualTo( -1 );
			target.Item.Should().Be.Null();
			target.OldIndex.Should().Be.EqualTo( -1 );
		}

		[TestMethod]
		public void collectionChangedEventArgs_ctor_changeType_index_should_set_default_values()
		{
			var cType = CollectionChangeType.Reset;
			var index = 10;

			var target = new CollectionChangedEventArgs<Object>( cType, index );

			target.ChangeType.Should().Be.EqualTo( cType );
			target.Index.Should().Be.EqualTo( index );
			target.Item.Should().Be.Null();
			target.OldIndex.Should().Be.EqualTo( -1 );
		}

		[TestMethod]
		public void collectionChangedEventArgs_ctor_changeType_index_oldIndex_item_should_set_default_values()
		{
			var item = new Object();
			var cType = CollectionChangeType.ItemMoved;
			var index = 10;
			var oldIndex = 1;

			var target = new CollectionChangedEventArgs<Object>( cType, index, oldIndex, item );

			target.ChangeType.Should().Be.EqualTo( cType );
			target.Index.Should().Be.EqualTo( index );
			target.Item.Should().Be.EqualTo( item );
			target.OldIndex.Should().Be.EqualTo( oldIndex );
		}
	}
}

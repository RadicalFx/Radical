//extern alias tpx;

namespace Test.Radical
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Topics.Radical;

	[TestClass()]
	public class EnumItemDescriptionAttributeTest
	{
		private TestContext testContextInstance;

		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		protected virtual EnumItemDescriptionAttribute CreateMock( String caption )
		{
			return new EnumItemDescriptionAttribute( caption );
		}

		protected virtual EnumItemDescriptionAttribute CreateMock( String caption, Int32 index )
		{
			return new EnumItemDescriptionAttribute( caption, index );
		}

		[TestMethod()]
		public void EnumItemDescriptionAttribute_ctor_caption()
		{
			String caption = "fake caption";
			EnumItemDescriptionAttribute target = this.CreateMock( caption );

			Assert.IsNotNull( target );
		}

		[TestMethod()]
		public void EnumItemDescriptionAttribute_ctor_caption_index()
		{
			String caption = "fake caption";
			Int32 index = 0;
			EnumItemDescriptionAttribute target = this.CreateMock( caption, index );

			Assert.IsNotNull( target );
		}

		[TestMethod()]
		public void EnumItemDescriptionAttribute_caption()
		{
			String caption = "fake caption";
			EnumItemDescriptionAttribute target = this.CreateMock( caption );

			Assert.AreEqual<String>( caption, target.Caption );
		}

		[TestMethod()]
		public void EnumItemDescriptionAttribute_valid_caption()
		{
			String expectedCaption = "fake caption";
			Int32 expectedIndex = 0;
			EnumItemDescriptionAttribute target = this.CreateMock( expectedCaption, expectedIndex );

			Assert.AreEqual<String>( expectedCaption, target.Caption );
		}

		[TestMethod()]
		public void EnumItemDescriptionAttribute_valid_index()
		{
			String expectedcaption = "fake caption";
			Int32 expectedIndex = 0;
			EnumItemDescriptionAttribute target = this.CreateMock( expectedcaption, expectedIndex );

			Assert.AreEqual<Int32>( expectedIndex, target.Index );
		}

		[TestMethod()]
		public void EnumItemDescriptionAttribute_Index()
		{
			EnumItemDescriptionAttribute target = this.CreateMock( "fake caption" );
			Int32 actual = target.Index;

			Assert.AreEqual<Int32>( -1, actual );
		}

		[TestMethod()]
		[ExpectedException( typeof( ArgumentNullException ) )]
		public void EnumItemDescriptionAttribute_ctor_null_caption()
		{
			EnumItemDescriptionAttribute target = this.CreateMock( null );
		}
	}
}

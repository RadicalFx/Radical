namespace Radical.Tests.ChangeTracking
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using Radical.ChangeTracking;
    using Radical.ComponentModel.ChangeTracking;
    using SharpTestsEx;

    [TestClass]
    public class BookmarkTests
    {
        //[TestMethod]
        //public void bookmark_iChangeTrackingService_ctor()
        //{
        //    IChangeTrackingService mock = MockRepository.GenerateStub<IChangeTrackingService>();

        //    Bookmark bmk = new Bookmark( mock );

        //    Assert.AreEqual( mock, bmk.Owner );
        //}

        //[TestMethod]
        //[ExpectedException( typeof( ArgumentNullException ) )]
        //public void bookmark_null_owner_iChangeTrackingService_ctor_raise_argumentNullException_on_owner()
        //{
        //    Bookmark bmk = new Bookmark( null );
        //}

        //[TestMethod]
        //[ExpectedException( typeof( ArgumentNullException ) )]
        //public void bookmark_null_owner_null_iChange_iChangeTrackingService_ctor_raise_argumentNullException_on_owner()
        //{
        //    Bookmark bmk = new Bookmark( ( IChangeTrackingService )null, ( IChange )null );
        //}

        //[TestMethod]
        //[ExpectedException( typeof( ArgumentNullException ) )]
        //public void bookmark_null_owner_null_transientEntities_iChangeTrackingService_ctor_raise_argumentNullException_on_owner()
        //{
        //    Bookmark bmk = new Bookmark( ( IChangeTrackingService )null, ( IEnumerable<Object> )null );
        //}

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        [TestCategory( "ChangeTracking" )]
        public void bookmark_null_owner__null_change_null_transientEntities_iChangeTrackingService_ctor_raise_argumentNullException_on_owner()
        {
            Bookmark bmk = new Bookmark( ( IChangeTrackingService )null, ( IChange )null, ( IEnumerable<Object> )null );
        }

        //[TestMethod]
        //public void bookmark_valid_owner_null_iChange_iChangeTrackingService_ctor()
        //{
        //    IChangeTrackingService mock = MockRepository.GenerateStub<IChangeTrackingService>();
        //    Bookmark bmk = new Bookmark( mock, ( IChange )null );

        //    Assert.IsNull( bmk.Position );
        //}

        //[TestMethod]
        //public void bookmark_valid_owner_null_transientEntities_iChangeTrackingService_ctor_valid_transientEntities()
        //{
        //    IChangeTrackingService mock = MockRepository.GenerateStub<IChangeTrackingService>();
        //    Bookmark bmk = new Bookmark( mock, ( IEnumerable<Object> )null );

        //    bmk.TransientEntities.MustBeNonNull();
        //    bmk.TransientEntities.Count().MustBeEqualTo(0 );
        //}

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void bookmark_valid_owner_null_change_null_transientEntities_iChangeTrackingService_ctor_valid_transientEntities()
        {
            IChangeTrackingService mock = MockRepository.GenerateStub<IChangeTrackingService>();
            Bookmark bmk = new Bookmark( mock, ( IChange )null, ( IEnumerable<Object> )null );

            bmk.TransientEntities.Should().Not.Be.Null();
            bmk.TransientEntities.Count().Should().Be.EqualTo( 0 );
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void bookmark_valid_owner_null_iChange_valid_transientEntities_iChangeTrackingService_ctor()
        {
            IChangeTrackingService mock = MockRepository.GenerateStub<IChangeTrackingService>();
            Bookmark bmk = new Bookmark( mock, ( IChange )null, new Object[ 0 ] );

            bmk.Position.Should().Be.Null();
        }

        [TestMethod]
        [TestCategory( "ChangeTracking" )]
        public void bookmark_valid_owner_valid_iChange_valid_transientEntities_iChangeTrackingService_ctor()
        {
            IChangeTrackingService owner = MockRepository.GenerateStub<IChangeTrackingService>();
            IChange expected = MockRepository.GenerateStub<IChange>();

            Object[] entities = new Object[] { new Object(), new Object() };

            Bookmark bmk = new Bookmark( owner, expected, entities );

            bmk.Position.Should().Be.EqualTo( expected );
            bmk.TransientEntities.Should().Not.Be.Null();
            bmk.TransientEntities.Should().Have.SameSequenceAs( entities );
        }
    }
}
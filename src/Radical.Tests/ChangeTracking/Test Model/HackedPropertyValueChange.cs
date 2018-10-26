//extern alias tpx;

namespace Radical.Tests.ChangeTracking
{
    using System;
    using Radical.ChangeTracking.Specialized;
    using Radical.ComponentModel.ChangeTracking;

    class HackedPropertyValueChange : PropertyValueChange<String>
    {
        public HackedPropertyValueChange( Object owner, String value, RejectCallback<String> rc, CommitCallback<String> cc )
            : base( owner, "property-name", value, rc, cc, String.Empty )
        {

        }

        public CommitReason HackedCommitReason
        {
            get;
            set;
        }

        public RejectReason HackedRejectReason
        {
            get;
            set;
        }

        protected override void OnCommitted( CommittedEventArgs args )
        {
            base.OnCommitted( new CommittedEventArgs( this.HackedCommitReason ) );
        }

        protected override void OnRejected( RejectedEventArgs args )
        {
            base.OnRejected( new RejectedEventArgs( this.HackedRejectReason ) );
        }
    }
}

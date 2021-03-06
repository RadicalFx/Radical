﻿//extern alias tpx;

namespace Radical.Tests.ChangeTracking
{
    using Radical.ChangeTracking.Specialized;
    using Radical.ComponentModel.ChangeTracking;

    class HackedPropertyValueChange : PropertyValueChange<string>
    {
        public HackedPropertyValueChange(object owner, string value, RejectCallback<string> rc, CommitCallback<string> cc)
            : base(owner, "property-name", value, rc, cc, string.Empty)
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

        protected override void OnCommitted(CommittedEventArgs args)
        {
            base.OnCommitted(new CommittedEventArgs(HackedCommitReason));
        }

        protected override void OnRejected(RejectedEventArgs args)
        {
            base.OnRejected(new RejectedEventArgs(HackedRejectReason));
        }
    }
}

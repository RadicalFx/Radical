//extern alias tpx;

namespace Test.Radical.ChangeTracking
{
	using System;
	using Topics.Radical.ChangeTracking.Specialized;
	using Topics.Radical.ComponentModel.ChangeTracking;

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

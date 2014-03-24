namespace Topics.Radical.ChangeTracking
{
	using System;
	using System.Collections.Generic;
	using Topics.Radical;
	using Topics.Radical.Collections;
	using Topics.Radical.ComponentModel.ChangeTracking;
	using Topics.Radical.Validation;

	/// <summary>
	/// Represents a change occurred to an object.
	/// </summary>
	public abstract class Change<T> : IChange<T>
	{
		readonly RejectCallback<T> _rejectCallback;

		/// <summary>
		/// The callback to invoke in order to 
		/// reject the cached value.
		/// </summary>
		protected RejectCallback<T> RejectCallback
		{
			get { return this._rejectCallback; }
		}

		readonly CommitCallback<T> _commitCallback;

		/// <summary>
		/// The callback to invoke in order to
		/// commit the cached value.
		/// </summary>
		protected CommitCallback<T> CommitCallback
		{
			get { return this._commitCallback; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Change&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="valueToCache">The value to cache.</param>
		/// <param name="rejectCallback">The reject callback.</param>
		/// <param name="commitCallback">The commit callback.</param>
		/// <param name="description">The description.</param>
		protected Change( Object owner, T valueToCache, RejectCallback<T> rejectCallback, CommitCallback<T> commitCallback, String description )
		{
			Ensure.That( owner ).Named( "owner" ).IsNotNull();
			Ensure.That( rejectCallback ).Named( "rejectCallback" ).IsNotNull();

			this.Owner = owner;
			this.CachedValue = valueToCache;
			this._rejectCallback = rejectCallback;
			this._commitCallback = commitCallback;
			this.Description = description;
		}

		#region IChange Members

		/// <summary>
		/// Gets the owner of this change.
		/// </summary>
		/// <value>The owner.</value>
		public Object Owner
		{
			get;
			private set;
		}

		/// <summary>
		/// Commits this change.
		/// </summary>
		public void Commit( CommitReason reason )
		{
			Ensure.That( reason )
				.Named( "reason" )
				.IsDefined()
				.If( v => v == CommitReason.None )
				.Then( ( v, n ) => 
				{ 
					throw new ArgumentException( "Unsupported CommitReason value.", n ); 
				} );

			this.OnCommit( reason );
			this.OnCommitted( new CommittedEventArgs( reason ) );
		}

		/// <summary>
		/// Called in order to commit this change.
		/// </summary>
		protected virtual void OnCommit( CommitReason reason )
		{
			if( this.CommitCallback != null )
			{
				ChangeCommittedEventArgs<T> args = new ChangeCommittedEventArgs<T>( this.Owner, this.CachedValue, this, reason );
				this.CommitCallback( args );
			}
		}

		/// <summary>
		/// Occurs when this IChange has been committed.
		/// </summary>
		public event EventHandler<CommittedEventArgs> Committed;

		/// <summary>
		/// Called when Committed event.
		/// </summary>
		protected virtual void OnCommitted( CommittedEventArgs args )
		{
			var h = this.Committed;
			if( h != null )
			{
				h( this, args );
			}
		}

		/// <summary>
		/// Rejects this change.
		/// </summary>
		public void Reject( RejectReason reason )
		{
			Ensure.That( reason )
				.Named( "reason" )
				.IsDefined()
				.If( v => v == RejectReason.None )
				.Then( ( v, n ) => { throw new ArgumentException( "Unsupported RejectReason value.", n ); } );

			this.OnReject( reason );
			this.OnRejected( new RejectedEventArgs( reason ) );
		}

		/// <summary>
		/// Called in order to reject this change.
		/// </summary>
		protected virtual void OnReject( RejectReason reason )
		{
			ChangeRejectedEventArgs<T> args = new ChangeRejectedEventArgs<T>( this.Owner, this.CachedValue, this, reason );
			this.RejectCallback( args );
		}

		/// <summary>
		/// Occurs when this IChange has been rejected.
		/// </summary>
		public event EventHandler<RejectedEventArgs> Rejected;

		/// <summary>
		/// Raises the Rejected event.
		/// </summary>
		protected virtual void OnRejected( RejectedEventArgs args )
		{
			var h = this.Rejected;
			if( h != null )
			{
				h( this, args );
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance supports commit.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance supports commit; otherwise, <c>false</c>.
		/// </value>
		public virtual Boolean IsCommitSupported
		{
			get { return this.CommitCallback != null; }
		}

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the advised action for this IChange.
		/// </summary>
		/// <value>The advised action.</value>
		public abstract ProposedActions GetAdvisedAction( Object changedItem);

		/// <summary>
		/// Gets the changed entities.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<Object> GetChangedEntities()
		{
			return new ReadOnlyCollection<Object>( new[] { this.Owner } );
		}

		/// <summary>
		/// Clones this IChange instance.
		/// </summary>
		/// <returns>A clone of this instance.</returns>
		public abstract IChange Clone();

		#endregion

		#region IChange<T> Members

		/// <summary>
		/// Gets the cached value.
		/// </summary>
		/// <value>The cached value.</value>
		public T CachedValue
		{
			get;
			private set;
		}

		#endregion
	}
}

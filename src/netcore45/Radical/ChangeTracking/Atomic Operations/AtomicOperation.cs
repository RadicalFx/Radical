using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel.ChangeTracking;

namespace Topics.Radical.ChangeTracking
{
	sealed class AtomicOperation : IAtomicOperation
	{
		void OnCompleted( AtomicChange change )
		{
			if( this.completed != null )
			{
				this.completed( change );
			}
		}

		void OnDisposed()
		{
			if( this.disposed != null )
			{
				this.disposed();
			}
		}

		readonly Action<AtomicChange> completed;
		readonly Action disposed;

		AtomicChange change = new AtomicChange();

		public AtomicOperation( Action<AtomicChange> completed, Action disposed )
		{
			this.completed = completed;
			this.disposed = disposed;
		}

		public void Add( IChange change, AddChangeBehavior behavior )
		{
			this.change.Add( change, behavior );
		}

		public void RegisterTransient( Object entity, Boolean autoRemove )
		{
			this.change.RegisterTransient( entity, autoRemove );
		}

		public void Complete()
		{
			this.OnCompleted( this.change );
		}

		public void Dispose()
		{
			//clear changes list if any
			this.change = null;
		}
	}
}

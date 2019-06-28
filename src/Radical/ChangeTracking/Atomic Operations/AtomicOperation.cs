using Radical.ComponentModel.ChangeTracking;
using System;

namespace Radical.ChangeTracking
{
    sealed class AtomicOperation : IAtomicOperation
    {
        bool isCompleted = false;

        void OnCompleted( AtomicChange change )
        {
            if( this.completed != null )
            {
                this.completed( change );
            }

            this.isCompleted = true;
        }

        void OnDisposed()
        {
            if( !this.isCompleted && this.disposed != null )
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

        public void RegisterTransient( Object entity, bool autoRemove )
        {
            this.change.RegisterTransient( entity, autoRemove );
        }

        public void Complete()
        {
            this.OnCompleted( this.change );
        }

        public void Dispose()
        {
            if ( !this.isCompleted )
            {
                this.change.Reject( RejectReason.RejectChanges );
            }

            //clear changes list if any
            this.change = null;
            this.OnDisposed();
        }
    }
}

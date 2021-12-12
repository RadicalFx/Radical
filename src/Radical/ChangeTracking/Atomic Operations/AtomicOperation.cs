using Radical.ComponentModel.ChangeTracking;
using System;

namespace Radical.ChangeTracking
{
    sealed class AtomicOperation : IAtomicOperation
    {
        bool isCompleted;

        void OnCompleted(AtomicChange change)
        {
            completed?.Invoke(change);

            isCompleted = true;
        }

        void OnDisposed()
        {
            if (!isCompleted)
            {
                disposed?.Invoke();
            }
        }

        readonly Action<AtomicChange> completed;
        readonly Action disposed;
        AtomicChange change = new AtomicChange();

        public AtomicOperation(Action<AtomicChange> completed, Action disposed)
        {
            this.completed = completed;
            this.disposed = disposed;
        }

        public void Add(IChange change, AddChangeBehavior behavior)
        {
            this.change.Add(change, behavior);
        }

        public void RegisterTransient(object entity, bool autoRemove)
        {
            change.RegisterTransient(entity, autoRemove);
        }

        public void Complete()
        {
            OnCompleted(change);
        }

        public void Dispose()
        {
            if (!isCompleted)
            {
                change.Reject(RejectReason.RejectChanges);
            }

            //clear changes list if any
            change = null;
            OnDisposed();
        }
    }
}

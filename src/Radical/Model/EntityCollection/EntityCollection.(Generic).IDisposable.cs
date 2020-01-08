using System;

namespace Radical.Model
{
    partial class EntityCollection<T>
    {
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="EntityCollection&lt;T&gt;"/> is reclaimed by garbage collection.
        /// </summary>
        ~EntityCollection()
        {
            Dispose(false);
        }

        private bool isDisposed;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><collection>true</collection> to release both managed and unmanaged resources; <collection>false</collection> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                /*
                 * Se disposing è 'true' significa che dispose
                 * è stato invocato direttamentente dall'utente
                 * è quindi lecito accedere ai 'field' e ad 
                 * eventuali reference perchè sicuramente Finalize
                 * non è ancora stato chiamato su questi oggetti
                 */
                if (site != null && site.Container != null)
                {
                    site.Container.Remove(this);
                }

                if (_events != null)
                {
                    Events.Dispose();
                }
            }

            _events = null;

            OnDisposed();
            isDisposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void EnsureNotDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }
    }
}

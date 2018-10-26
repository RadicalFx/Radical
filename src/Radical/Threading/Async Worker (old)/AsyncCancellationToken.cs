using System;

namespace Radical.Threading
{
    [Obsolete( "User the new AsyncWorker." )]
    public sealed class AsyncCancellationToken
    {
        public AsyncCancellationToken()
        {
            this.CancellationPending = false;
        }

        public Boolean CancellationPending
        {
            get;
            private set;
        }

        internal void InjectAbortRequest()
        {
            if( !this.CancellationPending )
            {
                this.CancellationPending = true;
            }
        }

        internal void Reset()
        {
            this.CancellationPending = false;
        }
    }
}

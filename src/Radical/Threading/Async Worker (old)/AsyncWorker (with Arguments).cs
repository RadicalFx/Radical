namespace Radical.Threading
{
    using System;

    [Obsolete("User the new AsyncWorker.")]
    public sealed class AsyncWorker<TArgument>
    {
        readonly TArgument argument;

        internal AsyncWorker( TArgument argument )
        {
            this.argument = argument;
        }

        [Obsolete( "User the new AsyncWorker." )]
        public AsyncWorker<TArgument, TResult> AndExpectingAsResult<TResult>()
        {
            return new AsyncWorker<TArgument, TResult>( this.argument );
        }
    }
}

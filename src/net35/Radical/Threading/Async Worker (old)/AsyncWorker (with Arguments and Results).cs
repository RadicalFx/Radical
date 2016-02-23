using System;
namespace Topics.Radical.Threading
{
    [Obsolete( "User the new AsyncWorker." )]
    public sealed class AsyncWorker<TArgument, TResult>
    {
        readonly TArgument argument;

        internal AsyncWorker( TArgument argument )
        {
            this.argument = argument;
        }

        [Obsolete( "User the new AsyncWorker." )]
        public AsyncEngine<TArgument, TResult> WhenExecutedDo( ExecutionAction<TArgument, TResult> onExecution )
        {
            return new AsyncEngine<TArgument, TResult>( this.argument, onExecution );
        }
    }
}

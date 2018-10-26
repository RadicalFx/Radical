namespace Radical.Threading
{
    using System;

    [Obsolete( "User the new AsyncWorker." )]
    public static class Worker
    {
        [Obsolete( "User the new AsyncWorker." )]
        public static AsyncWorker<TArgument> UsingAsArgument<TArgument>( TArgument argument )
        {
            return new AsyncWorker<TArgument>( argument );
        }

        [Obsolete( "User the new AsyncWorker." )]
        public static AsyncWorker<Object, TResult> ExpectingAsResult<TResult>()
        {
            return new AsyncWorker<Object, TResult>( null );
        }
    }
}

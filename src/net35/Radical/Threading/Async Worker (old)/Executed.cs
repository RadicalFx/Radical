namespace Topics.Radical.Threading
{
    using System;

    [Obsolete( "User the new AsyncWorker." )]
    public delegate void ExecutedAction<TArgument, TResult>( ExecutedArgs<TArgument, TResult> result );

    [Obsolete( "User the new AsyncWorker." )]
    public class ExecutedArgs<TArgument, TResult> : EventArgs
    {
        public ExecutedArgs( TArgument argument, TResult result, Boolean cancelled )
        {
            this.Argument = argument;
            this.Result = result;
            this.Cancelled = cancelled;
        }

        public TArgument Argument { get; private set; }
        public TResult Result { get; private set; }
        public Boolean Cancelled { get; private set; }
    }
}

namespace Radical.Threading
{
    using System;
    using System.ComponentModel;

    [Obsolete( "User the new AsyncWorker." )]
    public delegate void ExecutingAction<T>( ExecutingArgs<T> argument );

    [Obsolete( "User the new AsyncWorker." )]
    public class ExecutingArgs<T> : CancelEventArgs
    {
        public ExecutingArgs( T argument )
        {
            this.Argument = argument;
        }

        public T Argument { get; private set; }
    }
}

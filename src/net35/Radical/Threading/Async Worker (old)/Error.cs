namespace Topics.Radical.Threading
{
    using System;

    [Obsolete( "User the new AsyncWorker." )]
    public delegate void ErrorAction( ErrorArgs args );

    [Obsolete( "User the new AsyncWorker." )]
    public class ErrorArgs : EventArgs
    {
        public ErrorArgs( Exception error )
        {
            this.Error = error;
        }

        public Exception Error { get; private set; }
    }
}

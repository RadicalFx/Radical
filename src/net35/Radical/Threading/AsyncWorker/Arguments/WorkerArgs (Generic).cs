using System;

namespace Topics.Radical.Threading
{
    class WorkerArgs<T, TResult> :
        IBeforeArgs<T>,
        IAsyncArgs<T>,
        IAsyncArgs<T, TResult>,
        IAfterArgs<T>,
        IAfterArgs<T, TResult>,
        IOutputAsyncArgs<TResult>,
        IOutputAfterArgs<TResult>
    {
        public WorkerArgs( T argument )
        {
            this.Argument = argument;
        }

        public bool Cancel
        {
            get;
            set;
        }

        public T Argument
        {
            get;
            private set;
        }

        TResult _result;

        TResult IAfterArgs<T, TResult>.Result
        {
            get { return this._result; }
        }

        TResult IAsyncArgs<T, TResult>.Result
        {
            get { return this._result; }
            set { this._result = value;}
        }

        TResult IOutputAfterArgs<TResult>.Result
        {
            get { return this._result; }
        }

        TResult IOutputAsyncArgs<TResult>.Result
        {
            get { return this._result; }
            set { this._result = value; }
        }

        public void InjectCancelRequest() 
        {
            this.IsCancellationPending = true;
        }

        public bool IsCancellationPending
        {
            get;
            private set;
        }
    }
}

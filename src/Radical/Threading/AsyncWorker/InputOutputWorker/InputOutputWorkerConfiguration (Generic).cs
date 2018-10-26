using System;
using Radical.Validation;

namespace Radical.Threading
{
    sealed class InputOutputWorkerConfiguration<T, TResult> : WorkerConfiguration<T, TResult>, IInputOutputWorkerConfiguration<T, TResult>
    {
        public InputOutputWorkerConfiguration( WorkerArgs<T, TResult> arguments )
            : base( arguments )
        {

        }

        public Action<IBeforeArgs<T>> Before
        {
            get;
            set;
        }

        public override bool ExecuteBefore()
        {
            if( this.Before != null )
            {
                this.Before( this.WorkerArguments );
                return this.WorkerArguments.Cancel;
            }

            return false;
        }

        //public Action<IAsyncArgs<T, TResult>> Async
        //{
        //    get;
        //    set;
        //}

        //public override void ExecuteAsync()
        //{
        //    this.Async( this.WorkerArguments );
        //}

        public override void ExecuteAsync()
        {
            //this.Async( this.WorkerArguments );
            ( ( Action<IAsyncArgs<T, TResult>> )this.AsyncHandler )( this.WorkerArguments );
        }

        public Action<IAfterArgs<T, TResult>> After
        {
            get;
            set;
        }

        public override void ExecuteAfter()
        {
            if( this.After != null )
            {
                this.After( this.WorkerArguments );
            }
        }
    }
}

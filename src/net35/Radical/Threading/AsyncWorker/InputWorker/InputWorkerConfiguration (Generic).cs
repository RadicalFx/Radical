using System;
using Topics.Radical.Validation;

namespace Topics.Radical.Threading
{
    sealed class InputWorkerConfiguration<T> : WorkerConfiguration<T, Object>, IInputWorkerConfiguration<T>
    {
        public InputWorkerConfiguration( WorkerArgs<T, Object> arguments )
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

        public override void ExecuteAsync()
        {
            ( ( Action<IAsyncArgs<T>> )this.AsyncHandler )( this.WorkerArguments );
        }

        public Action<IAfterArgs<T>> After
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

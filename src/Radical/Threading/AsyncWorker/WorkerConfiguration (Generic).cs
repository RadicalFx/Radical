using System;
using Radical.Validation;

namespace Radical.Threading
{
    abstract class WorkerConfiguration<T, TResult> : 
        IRunnableWorkerConfiguration, 
        IWorkerConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerConfiguration&lt;T, TResult&gt;"/> class.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        protected WorkerConfiguration( WorkerArgs<T, TResult> arguments )
        {
            this.WorkerArguments = arguments;
        }

        public WorkerArgs<T, TResult> WorkerArguments { get; private set; }

        public Action<IAsyncErrorArgs> Error
        {
            get;
            set;
        }

        public WarningThreshold WarningThreshold
        {
            get;
            set;
        }

        public void ThresholdReached()
        {
            this.WarningThreshold.Handler();
        }

        public void Validate()
        {
            if( this.WarningThreshold != null )
            {
                Ensure.That( this.WarningThreshold.Handler )
                    .Named( "WarningThreshold.Handler" )
                    .IsNotNull();
            }

            Ensure.That( this.AsyncHandler ).IsNotNull();

            this.OnValidate();
        }

        public Delegate AsyncHandler { get; set; }

        protected virtual void OnValidate()
        {
 
        }

        public abstract bool ExecuteBefore();

        public abstract void ExecuteAsync();

        public abstract void ExecuteAfter();

        public virtual bool ExecuteError( Exception exception )
        {
            var ea = new WorkerErrorArgs( exception );

            if( this.Error != null )
            {
                this.Error( ea );
            }

            return ea.Handled;
        }
    }
}

using System;
using System.ComponentModel;
using Radical.Validation;
using System.Timers;
using Radical.Helpers;
//using log4net;

namespace Radical.Threading
{
    [Obsolete( "User the new AsyncWorker." )]
    public sealed class AsyncEngine<TArgument, TResult>
    {
        //static readonly ILog logger = LogManager.GetLogger( typeof( AsyncEngine<TArgument, TResult> ) );

        readonly BackgroundWorker worker;
        readonly TArgument argument;
        readonly ExecutionAction<TArgument, TResult> onExecution;
        readonly AsyncCancellationToken cancellationToken;

        ExecutingAction<TArgument> onExecuting;
        ExecutedAction<TArgument, TResult> onExecuted;
        ErrorAction onError;
        Action<ReportProgressArgs> onReportProgress;
        Action onWarningThresholdReached;
        TimeSpan warningThreshold = TimeSpan.MaxValue;

        internal AsyncEngine( TArgument argument, ExecutionAction<TArgument, TResult> onExecution )
        {
            this.argument = argument;
            this.onExecution = onExecution;
            this.worker = new BackgroundWorker();
            this.cancellationToken = new AsyncCancellationToken();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public Boolean IsBusy
        {
            get { return this.worker.IsBusy; }
        }

        public void Cancel()
        {
            if( this.IsBusy )
            {
                this.cancellationToken.InjectAbortRequest();
            }
        }

        public AsyncEngine<TArgument, TResult> ButBeforeDo( ExecutingAction<TArgument> onExecuting )
        {
            this.onExecuting = onExecuting;
            return this;
        }

        public AsyncEngine<TArgument, TResult> AndAfterDo( ExecutedAction<TArgument, TResult> onExecuted )
        {
            this.onExecuted = onExecuted;
            return this;
        }

        /// <summary>
        /// Allow the caller to intercept, at the end of the execution,
        /// exceptions generated during the async execution, if no error
        /// handler is registered and an async error occurs an exception
        /// is automatically raised at the end of the execution.
        /// </summary>
        /// <param name="onError">The error interceptor.</param>
        /// <returns>This instance for fluent interface programming.</returns>
        public AsyncEngine<TArgument, TResult> IfAnErrorOccurs( ErrorAction onError )
        {
            this.onError = onError;
            return this;
        }

        public AsyncEngine<TArgument, TResult> WhenProgressChanges( Action<ReportProgressArgs> onReportProgress )
        {
            this.onReportProgress = onReportProgress;
            return this;
        }

        public AsyncEngine<TArgument, TResult> OnWarningThresholdReached( TimeSpan warningThreshold, Action action )
        {
            Ensure.That( action ).Named( "action" ).IsNotNull();

            this.warningThreshold = warningThreshold;
            this.onWarningThresholdReached = action;

            return this;
        }

        public AsyncEngine<TArgument, TResult> Execute()
        {
            if( this.worker.IsBusy )
            {
                throw new NotSupportedException( "Cannot run multiple async operation using the same AsyncWorker, wait for current operation top complete." );
            }

            Timer warningThresholdTimer = null;
            if( warningThreshold > TimeSpan.Zero && warningThreshold < TimeSpan.MaxValue )
            {
                ElapsedEventHandler h = null;
                h = ( s, e ) =>
                {
                    this.onWarningThresholdReached();

                    var t = ( Timer )s;

                    t.Elapsed -= h;
                    t.Dispose();
                };

                warningThresholdTimer = new Timer()
                {
                    Interval = warningThreshold.TotalMilliseconds,
                    AutoReset = false,
                    Enabled = false
                };

                warningThresholdTimer.Elapsed += h;
            }

            this.worker.WorkerSupportsCancellation = true;// this.onCancelRequest != null;
            this.worker.WorkerReportsProgress = this.onReportProgress != null;

            if( this.worker.WorkerReportsProgress )
            {
                this.worker.ProgressChanged += ( s, e ) => this.onReportProgress( new ReportProgressArgs( e.ProgressPercentage ) );
            }

            this.worker.DoWork += ( s, e ) =>
            {
                this.cancellationToken.Reset();

                var progress = this.worker.WorkerReportsProgress ? ( ReportProgressAction )( p => this.worker.ReportProgress( p.Progress ) ) : ( ReportProgressAction )( p => { } );
                var executionArgs = new ExecutionArgs<TArgument, TResult>( ( TArgument )e.Argument, progress, this.cancellationToken );

                this.onExecution( executionArgs );

#if DEBUG
                var cl = CommandLine.GetCurrent();

                Int32 delay;
                if( cl.TryGetValue( "asyncdelay", out delay ) )
                {
                    //logger.WarnFormat( "Adding delay to async execution: {0}", delay );
                    System.Threading.Thread.Sleep( delay );
                }
#endif

                e.Result = new Values<TArgument, TResult>( ( TArgument )e.Argument, executionArgs.Result );
                e.Cancel = executionArgs.Cancelled;
            };

            this.worker.RunWorkerCompleted += ( s, e ) =>
            {
                if( warningThresholdTimer != null )
                {
                    warningThresholdTimer.Enabled = false;
                    warningThresholdTimer.Dispose();
                }

                if( e.Error != null )
                {
                    if( this.onError != null )
                    {
                        this.onError( new ErrorArgs( e.Error ) );
                    }
                    else
                    {
                        throw e.Error;
                    }
                }
                else
                {
                    var data = ( Values<TArgument, TResult> )e.Result;
                    this.onExecuted( new ExecutedArgs<TArgument, TResult>( data.Value1, data.Value2, e.Cancelled ) );
                }
            };

            try
            {
                var obj = new ExecutingArgs<TArgument>( this.argument );
                if( this.onExecuting != null )
                {
                    this.onExecuting( obj );
                }

                if( !obj.Cancel )
                {
                    if( warningThresholdTimer != null )
                    {
                        warningThresholdTimer.Enabled = true;
                    }

                    this.worker.RunWorkerAsync( this.argument );
                }
                else
                {
                    this.onExecuted( new ExecutedArgs<TArgument, TResult>( this.argument, default( TResult ), true ) );
                }

                return this;
            }
            catch
            {
                if( warningThresholdTimer != null )
                {
                    warningThresholdTimer.Enabled = false;
                    warningThresholdTimer.Dispose();
                }

                throw;
            }
        }
    }
}

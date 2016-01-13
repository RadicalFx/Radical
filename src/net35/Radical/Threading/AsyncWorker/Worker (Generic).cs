#if !SILVERLIGHT
//using log4net;
using Topics.Radical.Diagnostics;
#endif

using System;
using System.Diagnostics;
using Topics.Radical.Helpers;
using System.ComponentModel;
using System.Threading;
using Topics.Radical.ComponentModel;
using System.Collections.Generic;


namespace Topics.Radical.Threading
{
    sealed class Worker<T, TResult> :
        IInputWorker<T>,
        IConfiguredInputWorker<T>,
        IOuputWorker<TResult>,
        IConfiguredOuputWorker<TResult>,
        IInputOutputWorker<T, TResult>,
        IConfiguredInputOutputWorker<T, TResult>,
        IWorker,
        IExecutableWorker
    {
#if !SILVERLIGHT
        static readonly TraceSource logger = new TraceSource( typeof( IWorker ).FullName );
//        static readonly ILog logger = LogManager.GetLogger( typeof( Worker<T, TResult> ) );
#endif

        readonly IRunnableWorkerConfiguration configuration;

        public Worker( IRunnableWorkerConfiguration configuration )
        {
            this.configuration = configuration;
        }

#pragma warning disable 0693

        public IInputOutputWorker<T, TResult> AndExpecting<TResult>()
        {
            return this.AndExpecting<TResult>( default( TResult ) );
        }

        public IInputOutputWorker<T, TResult> AndExpecting<TResult>( TResult sample )
        {
            var actualConfig = ( WorkerConfiguration<T, Object> )this.configuration;

            var wArgs = new WorkerArgs<T, TResult>( actualConfig.WorkerArguments.Argument );
            var wConfig = new InputOutputWorkerConfiguration<T, TResult>( wArgs );

            return new Worker<T, TResult>( wConfig );
        }

#pragma warning restore 0693

        [Conditional( "DEBUG" )]
        void AddDelayIfAny()
        {
#if !WINDOWS_PHONE
            var cl = CommandLine.GetCurrent();

            Int32 delay;
            if( cl.TryGetValue( "asyncdelay", out delay ) )
            {
#if !SILVERLIGHT
                //logger.WarnFormat( "Adding delay to async execution: {0}", delay );
                logger.Warning( "Adding delay to async execution: {0}", delay );
#endif
                System.Threading.Thread.Sleep( delay );
            }
#endif
        }

        IConfiguredInputWorker<T> IInputWorker<T>.Configure( Action<IInputWorkerConfiguration<T>> userCfg )
        {
            var wConfig = ( IInputWorkerConfiguration<T> )this.configuration;
            userCfg( wConfig );

            return this;
        }

        IWorker IConfiguredInputWorker<T>.Execute( Action<IAsyncArgs<T>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this.ExecuteCore();
        }

        IExecutableWorker IConfiguredInputWorker<T>.OnExecute( Action<IAsyncArgs<T>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this;
        }

        IWorker IInputWorker<T>.Execute( Action<IAsyncArgs<T>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this.ExecuteCore();
        }

        IExecutableWorker IInputWorker<T>.OnExecute( Action<IAsyncArgs<T>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this;
        }

        IConfiguredOuputWorker<TResult> IOuputWorker<TResult>.Configure( Action<IOutputWorkerConfiguration<TResult>> userCfg )
        {
            var wConfig = ( IOutputWorkerConfiguration<TResult> )this.configuration;
            userCfg( wConfig );

            return this;
        }

        IWorker IConfiguredOuputWorker<TResult>.Execute( Action<IOutputAsyncArgs<TResult>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this.ExecuteCore();
        }

        IExecutableWorker IConfiguredOuputWorker<TResult>.OnExecute( Action<IOutputAsyncArgs<TResult>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this;
        }

        IWorker IOuputWorker<TResult>.Execute( Action<IOutputAsyncArgs<TResult>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this.ExecuteCore();
        }

        IExecutableWorker IOuputWorker<TResult>.OnExecute( Action<IOutputAsyncArgs<TResult>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this;
        }

        IConfiguredInputOutputWorker<T, TResult> IInputOutputWorker<T, TResult>.Configure( Action<IInputOutputWorkerConfiguration<T, TResult>> userCfg )
        {
            var wConfig = ( IInputOutputWorkerConfiguration<T, TResult> )this.configuration;
            userCfg( wConfig );

            return this;
        }

        IWorker IConfiguredInputOutputWorker<T, TResult>.Execute( Action<IAsyncArgs<T, TResult>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this.ExecuteCore();
        }

        IExecutableWorker IConfiguredInputOutputWorker<T, TResult>.OnExecute( Action<IAsyncArgs<T, TResult>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this;
        }

        IWorker IInputOutputWorker<T, TResult>.Execute( Action<IAsyncArgs<T, TResult>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this.ExecuteCore();
        }

        IExecutableWorker IInputOutputWorker<T, TResult>.OnExecute( Action<IAsyncArgs<T, TResult>> asyncHandler )
        {
            this.configuration.AsyncHandler = asyncHandler;
            return this;
        }

        IWorker ExecuteCore()
        {
            var wConfig = this.configuration;
            wConfig.Validate();

            this.IsBusy = true;
            this.HasCompleted = false;
            this._asyncWaitHandle.Reset();

            if( !wConfig.ExecuteBefore() )
            {
                var worker = new BackgroundWorker();

                //async execution
                worker.DoWork += ( s, e ) =>
                {
                    this.AddDelayIfAny();

                    var tmp = ( WorkerConfiguration<T, TResult> )e.Argument;
                    tmp.ExecuteAsync();

                    e.Result = e.Argument;
                };

                //after execution
                worker.RunWorkerCompleted += ( s, e ) =>
                {
                    try
                    {
                        if( e.Error != null )
                        {
                            var handledInConfig = wConfig.ExecuteError( e.Error );
                            if( !handledInConfig )
                            {
                                var args = new AsyncErrorEventArgs( e.Error );
                                this.OnAsyncError( args );
                                if( !args.Handled )
                                {
                                    //No error handler or error not handled
                                    //raise the async error here.
                                    throw e.Error;
                                }
                            }
                        }
                        else
                        {
                            var tmp = ( WorkerConfiguration<T, TResult> )e.Result;
                            tmp.ExecuteAfter();
                        }
                    }
                    finally
                    {
                        this.IsBusy = false;
                        this.HasCompleted = true;
                        this._asyncWaitHandle.Set();

                        this.OnCompleted( new WorkCompletedEventArgs( false ) );

#if !SILVERLIGHT
                        worker.Dispose();
#endif
                        worker = null;
                    }
                };

                this.SetupThresholdBarrier( ( WorkerConfiguration<T, TResult> )wConfig );

                worker.RunWorkerAsync( wConfig );
            }
            else
            {
                this.HasCompleted = true;
                this.IsBusy = false;
            }

            return this;
        }

        void SetupThresholdBarrier( WorkerConfiguration<T, TResult> config )
        {
            Timer warningThresholdTimer = null;
            if( config.WarningThreshold != null && config.WarningThreshold.Threshold > TimeSpan.Zero && config.WarningThreshold.Threshold < TimeSpan.MaxValue )
            {
                TimerCallback tcb = o =>
                {
                    config.ThresholdReached();
                    warningThresholdTimer.Dispose();
                };

                warningThresholdTimer = new Timer
                (
                    tcb,
                    null,
                    ( long )config.WarningThreshold.Threshold.TotalMilliseconds,
                    Timeout.Infinite
                );
            }
        }

        public bool IsBusy
        {
            get;
            private set;
        }

        public bool HasCompleted
        {
            get;
            private set;
        }

        void OnAsyncError( AsyncErrorEventArgs args )
        {
            this.HasCompleted = true;
            var h = this.AsyncError;
            if( h != null )
            {
                h( this, args );
            }
        }

        void OnCompleted( WorkCompletedEventArgs args )
        {
            var h = this.Completed;
            if( h != null )
            {
                h( this, args );
            }
        }

        public event EventHandler<AsyncErrorEventArgs> AsyncError;

        public event EventHandler<WorkCompletedEventArgs> Completed;

        public void Cancel()
        {
            throw new NotImplementedException();
            //if( this.IsBusy )
            //{

            //}
        }

        IWorker IExecutableWorker.Execute()
        {
            return this.ExecuteCore();
        }

#if SILVERLIGHT
        List<WeakReference> triggers = new List<WeakReference>();
#else
        List<WeakReference<IMonitor>> triggers = new List<WeakReference<IMonitor>>();
#endif
        IExecutableWorker IExecutableWorker.AddTrigger( IMonitor trigger )
        {
            trigger.Changed += new EventHandler( OnTriggerChanged );

#if SILVERLIGHT
            triggers.Add( new WeakReference( trigger ) );
#else
            triggers.Add( new WeakReference<IMonitor>( trigger ) );
#endif

            var c = trigger as IComponent;
            if( c != null )
            {
                c.Disposed += new EventHandler( OnTriggerDisposed );
            }

            return this;
        }

        void OnTriggerDisposed( object sender, EventArgs e )
        {
            ( ( IMonitor )sender ).Changed -= new EventHandler( OnTriggerChanged );
            ( ( IComponent )sender ).Disposed -= new EventHandler( OnTriggerDisposed );
        }

        void CleanupTriggers()
        {
            foreach( var trigger in this.triggers )
            {
                if( trigger.IsAlive )
                {
#if SILVERLIGHT
                    ( ( IMonitor )trigger.Target ).Changed -= new EventHandler( OnTriggerChanged );
#else
                    trigger.Target.Changed -= new EventHandler( OnTriggerChanged );
#endif
                }
            }

            this.triggers.Clear();
        }

        void OnTriggerChanged( object sender, EventArgs e )
        {
            if( !this.IsBusy )
            {
                /*
                 * qui potrebbe aver senso fare l'enqueue 
                 * per accodare le esecuzioni
                 */
                this.ExecuteCore();
            }
        }

        ManualResetEvent _asyncWaitHandle = new ManualResetEvent( false );

        /// <summary>
        /// Gets the async wait handle that the calling thread can use to wait for the async work completition.
        /// </summary>
        /// <value>The async wait handle.</value>
        public WaitHandle AsyncWaitHandle
        {
            get { return this._asyncWaitHandle; }
        }
    }
}

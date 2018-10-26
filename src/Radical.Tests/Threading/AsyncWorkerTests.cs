namespace Radical.Tests.Threading
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpTestsEx;
    using Radical.Threading;
    using Radical.ComponentModel;

    [TestClass]
    public class AsyncWorkerTests
    {
        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_normal_should_work_as_expected()
        {
            //var wa = new ManualResetEvent( false );

            var mainThreadId = Thread.CurrentThread.ManagedThreadId;
            var asyncThreadId = Thread.CurrentThread.ManagedThreadId;

            var inArgument = "Foo";
            var beforeArgument = "";
            var asyncArgument = "";

            var worker = AsyncWorker.Using( inArgument )
                .Configure( cfg =>
                {
                    cfg.Before = e =>
                    {
                        beforeArgument = e.Argument;
                    };
                } )
                .Execute( e =>
                {
                    asyncThreadId = Thread.CurrentThread.ManagedThreadId;
                    asyncArgument = e.Argument;
                } );

            //worker.Completed += ( s, e ) => wa.Set();

            worker.AsyncWaitHandle.WaitOne();
            //wa.WaitOne();

            asyncThreadId.Should().Not.Be.EqualTo( mainThreadId );
            beforeArgument.Should().Be.EqualTo( inArgument );
            asyncArgument.Should().Be.EqualTo( inArgument );
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_raising_async_exception_should_return_exception_to_after_handler()
        {
            //var wa = new ManualResetEvent( false );
            Exception actual = null;

            var worker = AsyncWorker.Using( "foo" )
                .Configure( cfg =>
                {
                    cfg.Error = e =>
                    {
                        e.Handled = true;
                        actual = e.Error;
                        //wa.Set();
                    };
                } )
                .Execute( e =>
                {
                    throw new ArgumentException();
                } );

            //wa.WaitOne();
            worker.AsyncWaitHandle.WaitOne();
            actual.Should().Not.Be.Null();
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_using_warning_thresold_should_invoke_elapsed_threshold_action_on_reached_threshold()
        {
            var actual = false;
            //var wa = new ManualResetEvent( false );

            var worker = AsyncWorker.Using( 0 )
                .Configure( cfg =>
                {
                    cfg.WarningThreshold = new WarningThreshold()
                    {
                        Threshold = TimeSpan.FromMilliseconds( 500 ),
                        Handler = () => actual = true
                    };

                    //cfg.After = e => e.Argument.Set();
                } )
                .Execute( e => Thread.Sleep( 1000 ) );

            //wa.WaitOne();
            worker.AsyncWaitHandle.WaitOne();

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_using_warning_thresold_should_invoke_elapsed_threshold_only_once()
        {
            var actual = 0;
            var expected = 1;
            //var wa = new ManualResetEvent( false );

            var worker = AsyncWorker.Using( 0 )
                .Configure( cfg =>
                {
                    cfg.WarningThreshold = new WarningThreshold()
                    {
                        Threshold = TimeSpan.FromMilliseconds( 500 ),
                        Handler = () => actual++
                    };

                    //cfg.After = e => e.Argument.Set();
                } )
                .Execute( e => Thread.Sleep( 1500 ) );

            //wa.WaitOne();
            worker.AsyncWaitHandle.WaitOne();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_expecting_result_should_correctly_set_result()
        {
            //var wa = new ManualResetEvent( false );

            var expected = "result";
            var actual = "";

            var worker = AsyncWorker.Using( expected )
                .AndExpecting<String>()
                .Configure( cfg =>
                {
                    cfg.After = e => actual = e.Result;
                } )
                .Execute( e => e.Result = e.Argument );

            //worker.Completed += ( s, e ) => wa.Set();
            //wa.WaitOne();
            worker.AsyncWaitHandle.WaitOne();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_expecting_anonymous_type_as_result_result_should_correctly_set_result()
        {
            //var wa = new ManualResetEvent( false );

            var expected = "result";
            var actual = "";

            var worker = AsyncWorker.Using( expected )
                .AndExpecting( new { Sample = "" } )
                .Configure( cfg =>
                {
                    cfg.After = e => actual = e.Result.Sample;
                } )
                .Execute( e => e.Result = new { Sample = e.Argument } );

            //worker.Completed += ( s, e ) => wa.Set();
            //wa.WaitOne();
            worker.AsyncWaitHandle.WaitOne();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_using_results_only_worker_should_return_expected_value()
        {
            //var wa = new ManualResetEvent( false );

            var expected = "result";
            var actual = "";

            var worker = AsyncWorker.Expecting( new { Sample = "" } )
                .Configure( cfg =>
                {
                    cfg.After = e => actual = e.Result.Sample;
                } )
                .Execute( e => e.Result = new { Sample = expected } );

            //worker.Completed += ( s, e ) => wa.Set();
            //wa.WaitOne();

            worker.AsyncWaitHandle.WaitOne();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_using_no_configuration_should_not_fail()
        {
            //var wa = new ManualResetEvent( false );

            var expected = true;
            var actual = false;

            Action callback = () => { actual = true; };

            var worker = AsyncWorker.Using( callback )
                .Execute( e => e.Argument() );

            //worker.Completed += ( s, e ) => wa.Set();
            //wa.WaitOne();

            worker.AsyncWaitHandle.WaitOne();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [Ignore]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_normal_should_be_able_to_cancel_async_operation()
        {
            //var wa = new ManualResetEvent( false );
            var args = new { LBound = 0, UBound = 10 };
            var executedCycles = 0;

            var worker = AsyncWorker.Using( args )
                .Execute( e =>
                {
                    for( var i = e.Argument.LBound; i < e.Argument.UBound; i++ )
                    {
                        executedCycles++;
                        Thread.Sleep( 5 );

                        if( e.IsCancellationPending )
                        {
                            return;
                        }
                    }
                } );

            //worker.Completed += ( s, e ) =>
            //{
            //    wa.Set();
            //};

            Thread.Sleep( 15 );
            worker.Cancel();

            worker.AsyncWaitHandle.WaitOne();
            //wa.WaitOne();

            executedCycles.Should().Be.LessThan( args.UBound );
        }

        [TestMethod]
        [Ignore]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_if_async_work_has_been_cancelled_should_report_cancellation()
        {
            //var wa = new ManualResetEvent( false );
            var actual = false;

            var worker = AsyncWorker.Using( new { LBound = 0, UBound = 10 } )
                .Execute( e =>
                {
                    for( var i = e.Argument.LBound; i < e.Argument.UBound; i++ )
                    {
                        Thread.Sleep( 5 );
                        if( e.IsCancellationPending )
                        {
                            return;
                        }
                    }
                } );

            worker.Completed += ( s, e ) =>
            {
                actual = e.Cancelled;
                //wa.Set();
            };

            Thread.Sleep( 15 );
            worker.Cancel();

            worker.AsyncWaitHandle.WaitOne();
            //wa.WaitOne();

            actual.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_if_async_work_has_not_been_cancelled_should_not_report_cancellation()
        {
            //var wa = new ManualResetEvent( false );
            var actual = false;

            var worker = AsyncWorker.Using( new { LBound = 0, UBound = 10 } )
                .Execute( e =>
                {
                    for( var i = e.Argument.LBound; i < e.Argument.UBound; i++ )
                    {
                        Thread.Sleep( 5 );
                        if( e.IsCancellationPending )
                        {
                            return;
                        }
                    }
                } );

            worker.Completed += ( s, e ) =>
            {
                actual = e.Cancelled;
                //wa.Set();
            };

            //wa.WaitOne();
            worker.AsyncWaitHandle.WaitOne();

            actual.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_asAsyncAction_should_return_an_executable_asyncAction()
        {
            //var wa = new ManualResetEvent( false );

            var asyncAction = AsyncWorker.Using( 10 )
                .OnExecute( e =>
                {
                    Thread.Sleep( 5 );
                } );

            //asyncAction.Completed += ( s, e ) =>
            //{
            //    wa.Set();
            //};

            var worker = asyncAction.Execute();

            //wa.WaitOne();
            worker.AsyncWaitHandle.WaitOne();
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_hasCompleted_should_report_true_if_before_args_cancels_execution()
        {
            var worker = AsyncWorker.Using( 10 )
                .Configure( cfg =>
                {
                    cfg.Before = e => e.Cancel = true;
                } )
                .Execute( e =>
                {
                    Thread.Sleep( 5 );
                } );

            worker.HasCompleted.Should().Be.True();
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_isBusy_should_report_false_if_before_args_cancels_execution()
        {
            var worker = AsyncWorker.Using( 10 )
                .Configure( cfg =>
                {
                    cfg.Before = e => e.Cancel = true;
                } )
                .Execute( e =>
                {
                    Thread.Sleep( 5 );
                } );

            worker.IsBusy.Should().Be.False();
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_asAsyncAction_should_return_an_executable_asyncAction_that_can_be_executed_more_than_once()
        {
            var expected = 2;
            var actual = 0;

            //var wa = new ManualResetEvent( false );

            var asyncAction = AsyncWorker.Using( 10 )
                .OnExecute( e =>
                {
                    Thread.Sleep( 5 );
                    actual++;
                } );

            //asyncAction.Completed += ( s, e ) =>
            //{
            //    wa.Set();
            //};

            var worker = asyncAction.Execute();

            worker.AsyncWaitHandle.WaitOne();
            //wa.WaitOne();
            //wa.Reset();

            asyncAction.Execute();

            worker.AsyncWaitHandle.WaitOne();
            //wa.WaitOne();

            actual.Should().Be.EqualTo( expected );
        }

        [TestMethod]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_raising_async_exception_should_return_exception_using_iworker_asyncError_event()
        {
            //var wa = new ManualResetEvent( false );

            Exception actual = null;

            var worker = AsyncWorker.Using( "foo" )
                .Execute( e =>
                {
                    throw new ArgumentException();
                } );

            worker.AsyncError += ( s, e ) =>
            {
                e.Handled = true;
                actual = e.Error;
                //wa.Set();
            };

            worker.AsyncWaitHandle.WaitOne();
            //wa.WaitOne();
            actual.Should().Not.Be.Null();
        }

        [TestMethod]
        [Ignore]
        [TestCategory( "AsyncWorker" ), TestCategory( "Threading" )]
        public void asyncWorker_asAsyncAction_should_return_an_executable_asyncAction_with_support_for_triggers()
        {
            var wa = new ManualResetEvent( false );

            var asyncAction = AsyncWorker.Using( 5 )
                .OnExecute( e =>
                {
                    Thread.Sleep( e.Argument );
                } );

            var timerTrigger = new TimerTrigger( 5, TimerTriggerMode.Once );
            asyncAction.AddTrigger( timerTrigger );
            timerTrigger.Start();

            asyncAction.Completed += ( s, e ) => wa.Set();

            var actual = wa.WaitOne( 500 );

            //            var actual = asyncAction.AsyncWaitHandle.WaitOne( 15 );

            actual.Should().Be.True();
        }
    }
}

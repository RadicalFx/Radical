namespace Topics.Radical.Threading
{
	using System;
	using System.ComponentModel;

	[Obsolete( "User the new AsyncWorker." )]
	public delegate void ExecutionAction<TArgument, TResult>( ExecutionArgs<TArgument, TResult> argument );

	[Obsolete( "User the new AsyncWorker." )]
	public class ExecutionArgs<TArgument, TResult> : EventArgs
	{
		internal ExecutionArgs( TArgument argument, ReportProgressAction reportProgress, AsyncCancellationToken cancellationToken )
		{
			this.Argument = argument;
			this.ReportProgress = reportProgress;
			this.CancellationToken = cancellationToken;
		}

		public TArgument Argument { get; private set; }
		public TResult Result { get; set; }
		public Boolean Cancelled { get; set; }

		public ReportProgressAction ReportProgress { get; private set; }
		public AsyncCancellationToken CancellationToken { get; private set; }
	}
}

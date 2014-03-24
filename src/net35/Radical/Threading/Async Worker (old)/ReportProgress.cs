namespace Topics.Radical.Threading
{
	using System;

	[Obsolete( "User the new AsyncWorker." )]
	public delegate void ReportProgressAction( ReportProgressArgs args );

	[Obsolete( "User the new AsyncWorker." )]
	public class ReportProgressArgs : EventArgs
	{
		public ReportProgressArgs( Int32 progress )
		{
			this.Progress = progress;
		}

		public Int32 Progress { get; private set; }
	}
}

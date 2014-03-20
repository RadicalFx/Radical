using System;
using System.Threading;

namespace Topics.Radical.Threading
{
	/// <summary>
	/// Defines the basic contract for an async running work.
	/// </summary>
	public interface IWorker : IWorkerStatus
	{
		/// <summary>
		/// Cancels any pending async task.
		/// </summary>
		void Cancel();
	}
}

using System;

namespace Topics.Radical.Threading
{
	internal interface IRunnableWorkerConfiguration
	{
		Delegate AsyncHandler { get; set; }

		void ThresholdReached();

		void Validate();

		bool ExecuteBefore();

		void ExecuteAsync();

		void ExecuteAfter();

		bool ExecuteError( Exception exception );
	}
}

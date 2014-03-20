using System;

namespace Topics.Radical.Threading
{
	/// <summary>
	/// Defines arguments passed to the caller 
	/// before the async operation starts.
	/// </summary>
	/// <typeparam name="T">The type of the argument.</typeparam>
	public interface IBeforeArgs<T> : ICancelArgs
	{
		/// <summary>
		/// Gets the argument given to the AsyncWorker by the caller.
		/// </summary>
		/// <value>The argument.</value>
		T Argument { get; }
	}
}

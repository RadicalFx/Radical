using System;

namespace Topics.Radical.Threading
{
	/// <summary>
	/// Defines async arguments returned to the caller after the execution.
	/// </summary>
	/// <typeparam name="T">The type of the incoming argument.</typeparam>
	public interface IAfterArgs<T> : IAsyncArgs<T>
	{

	}

	/// <summary>
	/// Defines async arguments returned to the caller after the execution.
	/// </summary>
	/// <typeparam name="T">The type of the incoming argument.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	public interface IAfterArgs<T, TResult> : IAfterArgs<T>
	{
		/// <summary>
		/// Gets the result of the async operation.
		/// </summary>
		/// <value>The result.</value>
		TResult Result { get; }
	}

	/// <summary>
	/// Defines async arguments returned to the caller after the execution.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	public interface IOutputAfterArgs<TResult>
	{
		/// <summary>
		/// Gets the result of the async operation.
		/// </summary>
		/// <value>The result.</value>
		TResult Result { get; }
	}
}

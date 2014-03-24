using System;

namespace Topics.Radical.Threading
{
	/// <summary>
	/// Provides support for the canceling of an async operation.
	/// </summary>
	public interface ICancelArgs
	{
		/// <summary>
		/// Gets or sets a value indicating whether the async 
		/// operation should be canceled. The default value is
		/// <c>false</c>.
		/// </summary>
		/// <value><c>true</c> if the async operation should be canceled; otherwise, <c>false</c>.</value>
		Boolean Cancel { get; set; }
	}
}

using System;

namespace Topics.Radical.ServiceModel
{
	/// <summary>
	/// An <see cref="IMessageHeaderManager"/> is responsbile to
	/// bridge the gap between che user code and the complexty
	/// of WCF header low level management.
	/// </summary>
	public interface IMessageHeaderManager : IDisposable
	{
		/// <summary>
		/// Gets the header of the given type first searching in the 
		/// incoming headers and then, if no header can be found, in
		/// the outgoing headers.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <returns>The header if any; otherwise null.</returns>
		T GetHeader<T>();

		/// <summary>
		/// Gets the header of the given type first searching in the
		/// incoming headers and then, if no header can be found, in
		/// the outgoing headers.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="header">The header if any; otherwise null.</param>
		/// <returns><c>True</c> if the requested header has been found; otherwise false.</returns>
		Boolean TryGetHeader<T>( out T header );

		/// <summary>
		/// Gets the header of the given type first searching in the 
		/// incoming headers and then, if no header can be found, in
		/// the outgoing headers.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="searchBehavior">if set to <c>Mandatory</c> then the 
		/// header is considered a mandatory header and if no header 
		/// of the given type can be found an exception will be raised.</param>
		/// <returns>The header if any; otherwise null.</returns>
		T GetHeader<T>( HeaderSearchBehavior searchBehavior );

		/// <summary>
		/// Sets the header in current operation scope.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="header">The header to add to the operation context.</param>
		void SetHeader<T>( T header );

		/// <summary>
		/// Gets the outgoing header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <returns>The header if any; otherwise null.</returns>
		T GetOutgoingHeader<T>();

		/// <summary>
		/// Gets the outgoing header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="header">The header if any; otherwise null.</param>
		/// <returns><c>True</c> if the requested header has been found; otherwise false.</returns>
		Boolean TryGetOutgoingHeader<T>( out T header );

		/// <summary>
		/// Gets the outgoing header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="searchBehavior">if set to <c>Mandatory</c> then the 
		/// header is considered a mandatory header and if no header 
		/// of the given type can be found an exception will be raised.</param>
		/// <returns>The header if any; otherwise null.</returns>
		T GetOutgoingHeader<T>( HeaderSearchBehavior searchBehavior );

		/// <summary>
		/// Gets the incoming header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <returns>The header if any; otherwise null.</returns>
		T GetIncomingHeader<T>();

		/// <summary>
		/// Gets the incoming header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="header">The header if any; otherwise null.</param>
		/// <returns><c>True</c> if the requested header has been found; otherwise false.</returns>
		Boolean TryGetIncomingHeader<T>(out T header );

		/// <summary>
		/// Gets the incoming header of the given type.
		/// </summary>
		/// <typeparam name="T">The type of the header.</typeparam>
		/// <param name="searchBehavior">if set to <c>Mandatory</c> then the 
		/// header is considered a mandatory header and if no header 
		/// of the given type can be found an exception will be raised.</param>
		/// <returns>The header if any; otherwise null.</returns>
		T GetIncomingHeader<T>( HeaderSearchBehavior searchBehavior );
	}
}

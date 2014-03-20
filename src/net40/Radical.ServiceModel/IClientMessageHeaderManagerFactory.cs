using System;
using System.ServiceModel;

namespace Topics.Radical.ServiceModel
{
	/// <summary>
	/// An <see cref="IClientMessageHeaderManagerFactory"/> is responsbile
	/// for the creation of <see cref="IMessageHeaderManager"/> instances at
	/// the client side.
	/// </summary>
	public interface IClientMessageHeaderManagerFactory
	{
		/// <summary>
		/// Creates a new <see cref="IMessageHeaderManager"/> using the specified channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <returns>The new IMessageHeaderManager.</returns>
		IMessageHeaderManager Create( IContextChannel channel );
	}
}

using System;

namespace Topics.Radical.ServiceModel
{
	/// <summary>
	/// An <see cref="IServerMessageHeaderManagerFactory"/> is responsbile
	/// for the creation of <see cref="IMessageHeaderManager"/> instances at
	/// the server side.
	/// </summary>
	public interface IServerMessageHeaderManagerFactory
	{
		/// <summary>
		/// Creates a new <see cref="IMessageHeaderManager"/>.
		/// </summary>
		/// <returns>The new IMessageHeaderManager.</returns>
		IMessageHeaderManager Create();
	}
}

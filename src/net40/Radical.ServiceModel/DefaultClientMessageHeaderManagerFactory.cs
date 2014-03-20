using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Topics.Radical.ServiceModel
{
	/// <summary>
	/// Implements the <see cref="IClientMessageHeaderManagerFactory"/> providing
	/// a default working implementation.
	/// </summary>
	public class DefaultClientMessageHeaderManagerFactory : IClientMessageHeaderManagerFactory
	{
		/// <summary>
		/// Creates a new <see cref="IMessageHeaderManager"/> using the specified channel.
		/// </summary>
		/// <param name="channel">The channel.</param>
		/// <returns>The new IMessageHeaderManager.</returns>
		public IMessageHeaderManager Create( IContextChannel channel )
		{
			return new DefaultMessageHeaderManager( channel );
		}
	}
}

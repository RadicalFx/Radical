using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Topics.Radical.ServiceModel
{
	/// <summary>
	/// Implements the <see cref="IServerMessageHeaderManagerFactory"/> providing
	/// a default working implementation.
	/// </summary>
	public class DefaultServerMessageHeaderManagerFactory : IServerMessageHeaderManagerFactory
	{
		/// <summary>
		/// Creates a new <see cref="IMessageHeaderManager"/>.
		/// </summary>
		/// <returns>The new IMessageHeaderManager.</returns>
		public IMessageHeaderManager Create()
		{
			return new DefaultMessageHeaderManager();
		}
	}
}

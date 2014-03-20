using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Topics.Radical.ServiceModel
{
	/// <summary>
	/// Determines if the requested header is Mandatory or Optional.
	/// </summary>
	public enum HeaderSearchBehavior
	{
		/// <summary>
		/// The header is optional, this is the default value.
		/// </summary>
		Optional = 0,

		/// <summary>
		/// The header is mandatory, an exception should be raised if a mandatory header cannot be found.
		/// </summary>
		Mandatory = 1
	}
}

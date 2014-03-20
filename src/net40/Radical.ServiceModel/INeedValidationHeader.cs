using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using Topics.Radical.Validation;
using System.ServiceModel.Channels;
using System.ComponentModel;
using System.Reflection;

namespace Topics.Radical.ServiceModel
{
	/// <summary>
	/// Identifies that an header requires to be validated once
	/// extracted from the message.
	/// </summary>
	public interface INeedValidationHeader
	{
		/// <summary>
		/// Validates this header instance.
		/// </summary>
		void Validate();
	}
}

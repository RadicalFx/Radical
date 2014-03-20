namespace Topics.Radical
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// An <c>EnumValueOutOfRangeException</c> is raised whenever the validation
	/// process of an enumeration value determines that the supplied value is not
	/// defined by the enum type that has been validated.
	/// </summary>
	public class EnumValueOutOfRangeException : ArgumentException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueOutOfRangeException"/> class.
		/// </summary>
    	public EnumValueOutOfRangeException()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueOutOfRangeException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
    	public EnumValueOutOfRangeException( String message )
			: base( message )
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumValueOutOfRangeException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		public EnumValueOutOfRangeException( String message, Exception innerException )
			: base( message, innerException )
		{

		}
	}
}

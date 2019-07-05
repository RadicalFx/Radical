namespace Radical
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An <c>EnumValueOutOfRangeException</c> is raised whenever the validation
    /// process of an enumeration value determines that the supplied value is not
    /// defined by the enum type that has been validated.
    /// </summary>
    [Serializable]
    public class EnumValueOutOfRangeException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValueOutOfRangeException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected EnumValueOutOfRangeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

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
        public EnumValueOutOfRangeException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValueOutOfRangeException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EnumValueOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

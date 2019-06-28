namespace Radical
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception raised during the conversion to an <c>IKey</c> instance when the
    /// supplied data does not represents a valid <c>IKey</c>.
    /// </summary>
    [Serializable]
    public class InvalidKeyFormatException : RadicalException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidKeyFormatException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InvalidKeyFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidKeyFormatException"/> class.
        /// </summary>
        public InvalidKeyFormatException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidKeyFormatException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidKeyFormatException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidKeyFormatException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidKeyFormatException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

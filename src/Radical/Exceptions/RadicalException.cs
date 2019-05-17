namespace Radical
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// A base exception class for all Radical exceptions.
    /// </summary>
    [Serializable] 
    public class RadicalException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RadicalException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected RadicalException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {

        } 

        /// <summary>
        /// Initializes a new instance of the <see cref="RadicalException"/> class.
        /// </summary>
        public RadicalException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadicalException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public RadicalException( String message )
            : base( message )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadicalException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RadicalException( String message, Exception innerException )
            : base( message, innerException )
        {

        }
    }
}

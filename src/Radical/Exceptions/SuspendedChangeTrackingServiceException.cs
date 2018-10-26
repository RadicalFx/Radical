namespace Radical
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The <c>SuspendedChangeTrackingServiceException</c> is raised whenever a request
    /// to a change tracking service is performed but the service is in suspended state.
    /// In order to prevent the exception the caller should check the <c>IsSuspended</c>
    /// property of the service.
    /// </summary>
#if !SILVERLIGHT
    [Serializable] 
#endif
    public class SuspendedChangeTrackingServiceException : RadicalException
    {
#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the <see cref="SuspendedChangeTrackingServiceException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SuspendedChangeTrackingServiceException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {

        } 
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="SuspendedChangeTrackingServiceException"/> class.
        /// </summary>
        public SuspendedChangeTrackingServiceException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuspendedChangeTrackingServiceException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SuspendedChangeTrackingServiceException( String message )
            : base( message )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuspendedChangeTrackingServiceException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SuspendedChangeTrackingServiceException( String message, Exception innerException )
            : base( message, innerException )
        {

        }
    }
}

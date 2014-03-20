//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ServiceModel.Description;
//using System.ServiceModel.Dispatcher;
//using System.ServiceModel;
//using System.Collections.ObjectModel;
//using System.ServiceModel.Channels;
//using log4net;

//namespace Topics.Radical.ServiceModel.Behaviors
//{
//    /// <summary>
//    /// Defines a service behavior that intercepts and log using log4net
//    /// all the unhandled execeptions.
//    /// </summary>
//    public class Log4NetErrorLoggingServiceBehavior : IServiceBehavior, IErrorHandler
//    {
//        static readonly ILog logger = LogManager.GetLogger( typeof( Log4NetErrorLoggingServiceBehavior ) );

//        /// <summary>
//        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
//        /// </summary>
//        /// <param name="serviceDescription">The service description.</param>
//        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
//        public void Validate( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase ) { }

//        /// <summary>
//        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
//        /// </summary>
//        /// <param name="serviceDescription">The service description of the service.</param>
//        /// <param name="serviceHostBase">The host of the service.</param>
//        /// <param name="endpoints">The service endpoints.</param>
//        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
//        public void AddBindingParameters( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters ) { }

//        /// <summary>
//        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
//        /// </summary>
//        /// <param name="serviceDescription">The service description.</param>
//        /// <param name="serviceHostBase">The host that is currently being built.</param>
//        public void ApplyDispatchBehavior( ServiceDescription serviceDescription, ServiceHostBase serviceHostBase )
//        {
//            foreach( ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers )
//            {
//                var channelDispatcher = channelDispatcherBase as ChannelDispatcher;
//                if( channelDispatcher != null )
//                {
//                    channelDispatcher.ErrorHandlers.Add( this );
//                }
//            }
//        }

//        /// <summary>
//        /// Enables the creation of a custom <see cref="T:System.ServiceModel.FaultException`1"/> that is returned from an exception in the course of a service method.
//        /// </summary>
//        /// <param name="error">The <see cref="T:System.Exception"/> object thrown in the course of the service operation.</param>
//        /// <param name="version">The SOAP version of the message.</param>
//        /// <param name="fault">The <see cref="T:System.ServiceModel.Channels.Message"/> object that is returned to the client, or service, in the duplex case.</param>
//        public void ProvideFault( Exception error, MessageVersion version, ref Message fault )
//        {

//        }

//        /// <summary>
//        /// Enables error-related processing and returns a value that indicates whether the dispatcher aborts the session and the instance context in certain cases.
//        /// </summary>
//        /// <param name="error">The exception thrown during processing.</param>
//        /// <returns>
//        /// true if  should not abort the session (if there is one) and instance context if the instance context is not <see cref="F:System.ServiceModel.InstanceContextMode.Single"/>; otherwise, false. The default is false.
//        /// </returns>
//        public bool HandleError( Exception error )
//        {
//            logger.Fatal( error );
//            return false;
//        }
//    }
//}

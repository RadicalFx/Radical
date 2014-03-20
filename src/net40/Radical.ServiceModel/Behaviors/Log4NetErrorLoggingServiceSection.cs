//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ServiceModel.Description;
//using System.ServiceModel.Channels;
//using System.ServiceModel.Dispatcher;
//using System.Xml.Schema;
//using System.ServiceModel;
//using System.ServiceModel.Activation;
//using WsdlDescription = System.Web.Services.Description.ServiceDescription;
//using System.ServiceModel.Configuration;

//namespace Topics.Radical.ServiceModel.Behaviors
//{
//    /// <summary>
//    /// Defines the configuration extension to add the <see cref="Log4NetErrorLoggingServiceBehavior"/>
//    /// to the Wcf configuration directly via config file.
//    /// </summary>
//    public class Log4NetErrorLoggingServiceSection : BehaviorExtensionElement
//    {
//        /// <summary>
//        /// Creates a behavior extension based on the current configuration settings.
//        /// </summary>
//        /// <returns>The behavior extension.</returns>
//        protected override object CreateBehavior()
//        {
//            return new Log4NetErrorLoggingServiceBehavior();
//        }

//        /// <summary>
//        /// Gets the type of behavior.
//        /// </summary>
//        /// <value></value>
//        /// <returns>A <see cref="T:System.Type"/>.</returns>
//        public override Type BehaviorType
//        {
//            get { return typeof( Log4NetErrorLoggingServiceBehavior ); }
//        }
//    }

//}
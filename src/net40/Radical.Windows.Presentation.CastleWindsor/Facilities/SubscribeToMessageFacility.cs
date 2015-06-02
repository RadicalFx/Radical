using System;
using System.Linq;
using Castle.MicroKernel.Facilities;
using System.Reflection;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Reflection;
using System.Collections.Generic;
using Castle.MicroKernel;
using Topics.Radical;
using Topics.Radical.Diagnostics;
using System.Diagnostics;
using Topics.Radical.Linq;

namespace Castle.Facilities
{
#pragma warning disable 0618

    /// <summary>
    /// A facility to auto register and invoke message handlers within a message broker.
    /// </summary>
    public sealed class SubscribeToMessageFacility : AbstractFacility
    {
        static readonly TraceSource logger = new TraceSource( typeof( SubscribeToMessageFacility ).FullName );

		IList<Tuple<String, IHandler>> buffer = new List<Tuple<String, IHandler>>();
        Boolean isMessageBrokerRegistered = false;

        void Attach( String key, IHandler h )
        {
            var broker = this.Kernel.Resolve<IMessageBroker>();

            var invocationModel = h.ComponentModel.Implementation.Is<INeedSafeSubscription>() ? InvocationModel.Safe : InvocationModel.Default;
            h.ComponentModel.Implementation.GetAttributes<SubscribeToMessageAttribute>()
                .Return( attributes =>
                {
                    return attributes.Select( a => typeof( IMessageHandler<> ).MakeGenericType( a.MessageType ) )
                    .Where( gh => this.IsInterestingHandler( h, gh ) );
                }, () =>
                {
                    return h.ComponentModel.Implementation.GetInterfaces()
                                        .Where( i => ( i.Is<IMessageHandler>() || i.Is<IHandleMessage>() ) && i.IsGenericType );
                }, attributes =>
                {
                    return !attributes.Any();
                } )
                .ForEach( t => this.Subscribe( broker, key, h, t, invocationModel ) );
        }

        void Subscribe( IMessageBroker broker, String key, IHandler h, Type genericHandler, InvocationModel invocationModel )
        {
            /*
             * Qui abbiamo un problema di questo tipo: quando in Castle viene
             * registrato un componente per più di un servizio, ergo per più 
             * interfacce vogliamo venga risolto lo stesso tipo, abbiamo l'inghippo
             * che Castle registra n componenti che risolvono verso lo stesso tipo
             * tante quante sono le interfacce. Quindi l'evento qui registrato viene
             * scatenato n volte e siccome il primo test che noi facciamo è verificare
             * che il servizio sia IMessageHandler se un componente gestisce n messaggi
             * arriviamo qui n volte. Se il componente inoltre richiede la Subscribe
             * automatica per quei messaggi, ha quindi più di un SubscribeToMessageAttribute
             * dobbiamo assicurarci che la subscribe venga fatta una ed una sola volta.
             * per ogni tipo di messaggio.
             */
            var messageType = genericHandler.GetGenericArguments().Single(); // attribute.MessageType;

            logger.Debug
            (
                "\tSubscribing to message: {0}",
                messageType.ToString( "SN" )
            );

            if ( genericHandler.Is<IMessageHandler>() )
            {
                broker.Subscribe( this, messageType, invocationModel, msg =>
                {
                    if ( this.Kernel != null )
                    {
                        var handler = this.Kernel.Resolve( key, h.ComponentModel.Services.First() ) as IMessageHandler;
                        if ( handler != null )
                        {
                            logger.Debug
                            (
                                "Dispatching message {0} to IMessageHandler {1}",
                                msg.GetType().ToString( "SN" ),
                                handler.GetType().ToString( "SN" )
                            );

                            if ( handler.ShouldHandle( msg ) )
                            {
                                handler.Handle( msg );
                            }
                        }
                        else
                        {
                            logger.Debug
                            (
                                "IMessageHandler for {0} is null.",
                                msg.GetType().ToString( "SN" )
                            );
                        }
                    }
                    else
                    {
                        logger.Debug( "Kernel is null." );
                        logger.Debug( "Kernel is null." );
                    }
                } );
            }
            else if ( genericHandler.Is<IHandleMessage>() )
            {
                broker.Subscribe( this, messageType, invocationModel, ( s, msg ) =>
                {
                    if ( this.Kernel != null )
                    {
                        var handler = this.Kernel.Resolve( key, h.ComponentModel.Services.First() ) as IHandleMessage;
                        if ( handler != null )
                        {
                            logger.Debug
                            (
                                "Dispatching message {0} to IHandleMessage {1}",
                                msg.GetType().ToString( "SN" ),
                                handler.GetType().ToString( "SN" )
                            );

                            if ( handler.ShouldHandle( s, msg ) )
                            {
                                handler.Handle( s, msg );
                            }
                        }
                        else
                        {
                            logger.Debug
                            (
                                "IHandleMessage for {0} is null.",
                                msg.GetType().ToString( "SN" )
                            );
                        }
                    }
                    else
                    {
                        logger.Debug( "Kernel is null." );
                    }
                } );
            }
        }

        Boolean IsInterestingHandler( IHandler h, Type t )
        {
            return h.ComponentModel.Services.Any( s => s.Is( t ) ) || h.ComponentModel.Implementation.Is( t );
        }

        /// <summary>
        /// The custom initialization for the Facility.
        /// </summary>
        /// <remarks>It must be overriden.</remarks>
        protected override void Init()
        {
            this.Kernel.ComponentRegistered += ( s, h ) =>
            {
                if ( h.ComponentModel.Services.Any( mb => mb.Is<IMessageBroker>() ) )
                {
                    logger.Debug( "Registered component is IMessageBroker. Ready to empty the buffer." );

                    this.isMessageBrokerRegistered = true;

                    if ( this.buffer.Any() )
                    {
                        for ( var i = buffer.Count; i > 0; i-- )
                        {
                            var cmp = this.buffer[ i - 1 ];

#if FX40
                            this.Attach( cmp.Item1, cmp.Item2 );
#else
							this.Attach( cmp.Value1, cmp.Value2 );
#endif



                            this.buffer.Remove( cmp );
                        }

                        logger.Debug( "All handlers in the buffer attached." );
                    }
                }
                else if ( this.IsInterestingHandler( h, typeof( IMessageHandler ) ) || this.IsInterestingHandler( h, typeof( IHandleMessage ) ) )
                {
                    if ( !this.isMessageBrokerRegistered )
                    {
                        logger.Debug
                        (
                            "Registered component is IMessageHandler/IHandleMessage: {0}/{1}, but no broker yet registered. buffering...",
                            h.ComponentModel.Services.First().ToString( "SN" ),
                            h.ComponentModel.Implementation.ToString( "SN" )
                        );

#if FX40
                        this.buffer.Add( new Tuple<String, IHandler>( s, h ) );
#else
						this.buffer.Add( new Values<String, IHandler>( s, h ) );
#endif

                    }
                    else
                    {
                        logger.Debug
                        (
                            "Registered component is IMessageHandler/IHandleMessage: {0}/{1}",
                            h.ComponentModel.Services.First().ToString( "SN" ),
                            h.ComponentModel.Implementation.ToString( "SN" )
                        );

                        this.Attach( s, h );
                    }
                }
            };
        }
    }

#pragma warning restore 0618

}

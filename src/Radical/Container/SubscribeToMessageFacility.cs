using System;
using System.Linq;
using System.Reflection;
using Radical.ComponentModel.Messaging;
using Radical.Reflection;
using System.Collections.Generic;
using Radical;
using System.Diagnostics;
using Radical.Linq;
using Radical.ComponentModel;

namespace Radical
{
    public sealed class SubscribeToMessageFacility : IPuzzleContainerFacility
    {
#if FX35
        List<Values<String, IContainerEntry>> buffer = new List<Values<String, IContainerEntry>>();
#else
        List<Tuple<String, IContainerEntry>> buffer = new List<Tuple<String, IContainerEntry>>();
#endif
        Boolean isMessageBrokerRegistered = false;

        void Attach( IPuzzleContainer container, String key, IContainerEntry entry )
        {
            var invocationModel = entry.Component.Is<INeedSafeSubscription>() ?
                InvocationModel.Safe :
                InvocationModel.Default;

            entry.Component.GetAttributes<SubscribeToMessageAttribute>()
                .Return( attributes =>
                {
                    return attributes.Select( a => typeof( IHandleMessage<> ).MakeGenericType( a.MessageType ) )
                                        .Where( gh => this.IsInterestingHandler( entry, gh ) );
                }, () =>
                {
                    return entry.Component.GetInterfaces()
                                        .Where( i => (i.Is<IMessageHandler>() || i.Is<IHandleMessage>()) && i.IsGenericType );
                }, attributes =>
                {
                    return !attributes.Any();
                } )
                .ForEach( t => this.Subscribe( container, key, entry, t, invocationModel ) );
        }

        void Subscribe( IPuzzleContainer container, String key, IContainerEntry entry, Type genericHandler, InvocationModel invocationModel )
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
            var broker = container.Resolve<IMessageBroker>();
            var messageType = genericHandler.GetGenericArguments().Single(); // attribute.MessageType;

            //logger.Verbose
            //(
            //    "\tSubscribing to message: {0}",
            //    messageType.ToString( "SN" )
            //);

            if(genericHandler.Is<IMessageHandler>())
            {
                broker.Subscribe(this, messageType, invocationModel, msg =>
                {
                    var handler = container.Resolve(key, entry.Services.First()) as IMessageHandler;

                    //logger.Verbose
                    //(
                    //    "Dispatching message {0} to IMessageHandler {1}",
                    //    msg.GetType().ToString( "SN" ),
                    //    handler.GetType().ToString( "SN" )
                    //);

                    if(handler.ShouldHandle(msg))
                    {
                        handler.Handle(msg);
                    }
                });
            }
            else if(genericHandler.Is<IHandleMessage>())
            {
                broker.Subscribe(this, messageType, invocationModel, (s, msg) =>
                {
                    var handler = container.Resolve(key, entry.Services.First()) as IHandleMessage;

                    //logger.Verbose
                    //(
                    //    "Dispatching message {0} to IMessageHandler {1}",
                    //    msg.GetType().ToString( "SN" ),
                    //    handler.GetType().ToString( "SN" )
                    //);

                    if(handler.ShouldHandle(s, msg))
                    {
                        handler.Handle(s, msg);
                    }
                });
            }
        }

        Boolean IsInterestingHandler( IContainerEntry entry, Type t )
        {
            return entry.Services.Any( s => s.Is( t ) ) || entry.Component.Is( t );
        }

        public void Initialize( IPuzzleContainer container )
        {
            container.ComponentRegistered += ( s, e ) =>
            {
                if ( e.Entry.Services.Any( svc => svc.Is<IMessageBroker>() ) )
                {
                    //logger.Verbose( "Registered component is IMessageBroker. Ready to empty the buffer." );

                    this.isMessageBrokerRegistered = true;

                    if ( this.buffer.Any() )
                    {
                        for ( var i = buffer.Count; i > 0; i-- )
                        {
                            var cmp = this.buffer[ i - 1 ];
#if FX35
                            this.Attach( container, cmp.Value1, cmp.Value2 );
#else
                            this.Attach( container, cmp.Item1, cmp.Item2 );
#endif
                            this.buffer.Remove( cmp );
                        }

                        //logger.Verbose( "All handlers in the buffer attached." );
                    }
                }
                else if (this.IsInterestingHandler(e.Entry, typeof(IMessageHandler)) || this.IsInterestingHandler( e.Entry, typeof( IHandleMessage ) ) )
                {
                    if ( !this.isMessageBrokerRegistered )
                    {
                        //logger.Verbose
                        //(
                        //    "Registered component is IMessageHandler: {0}/{1}, but no broker yet registered. buffering...",
                        //    h.Service.ToString( "SN" ),
                        //    h.ComponentModel.Implementation.ToString( "SN" )
                        //);

#if FX35
                        this.buffer.Add( new Values<String, IContainerEntry>( e.Entry.Key, e.Entry ) );
#else
                        this.buffer.Add( new Tuple<String, IContainerEntry>( e.Entry.Key, e.Entry ) );
#endif
                    }
                    else
                    {
                        //logger.Verbose
                        //(
                        //    "Registered component is IMessageHandler: {0}/{1}",
                        //    h.Service.ToString( "SN" ),
                        //    h.ComponentModel.Implementation.ToString( "SN" )
                        //);

                        this.Attach( container, e.Entry.Key, e.Entry );
                    }
                }
            };
        }

        public void Teardown( IPuzzleContainer container )
        {

        }
    }
}

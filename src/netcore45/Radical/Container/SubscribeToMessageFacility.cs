using System;
using System.Linq;
using System.Reflection;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Reflection;
using System.Collections.Generic;
using Topics.Radical;
using System.Diagnostics;
using Topics.Radical.Linq;
using Topics.Radical.ComponentModel;

namespace Topics.Radical
{
    public sealed class SubscribeToMessageFacility : IPuzzleContainerFacility
    {
        List<Tuple<String, IContainerEntry>> buffer = new List<Tuple<String, IContainerEntry>>();
        Boolean isMessageBrokerRegistered = false;

        void Attach( IPuzzleContainer container, String key, IContainerEntry entry )
        {
            var invocationModel = entry.Component.Is<INeedSafeSubscription>() ?
                InvocationModel.Safe :
                InvocationModel.Default;

            entry.Component.GetCustomAttributes<SubscribeToMessageAttribute>()
                .Return( attributes =>
                {
                    return attributes.Select( a => typeof( IHandleMessage<> ).MakeGenericType( a.MessageType ) )
                                        .Where( gh => this.IsInterestingHandler( entry, gh.GetTypeInfo() ) );
                }, () =>
                {
                    return entry.Component.ImplementedInterfaces
                                        .Where( i => i.Is<IHandleMessage>() && i.GetTypeInfo().IsGenericType );
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
            var messageType = genericHandler.GetTypeInfo().GenericTypeArguments.Single(); // attribute.MessageType;

            //logger.Verbose
            //(
            //	"\tSubscribing to message: {0}",
            //	messageType.ToString( "SN" )
            //);

            broker.Subscribe( this, messageType, invocationModel, ( sender, msg ) =>
            {
                var handler = container.Resolve( key, entry.Service ) as IHandleMessage;

                //logger.Verbose
                //(
                //	"Dispatching message {0} to IMessageHandler {1}",
                //	msg.GetType().ToString( "SN" ),
                //	handler.GetType().ToString( "SN" )
                //);

                if ( handler.ShouldHandle( sender, msg ) )
                {
                    handler.Handle( sender, msg );
                }
            } );
        }

        Boolean IsInterestingHandler( IContainerEntry entry, TypeInfo t )
        {
            return entry.Service.Is( t ) || entry.Component.Is( t );
        }

        public void Initialize( IPuzzleContainer container )
        {
            container.ComponentRegistered += ( s, e ) =>
            {
                if ( e.Entry.Service.Is<IMessageBroker>() )
                {
                    //logger.Verbose( "Registered component is IMessageBroker. Ready to empty the buffer." );

                    this.isMessageBrokerRegistered = true;

                    if ( this.buffer.Any() )
                    {
                        for ( var i = buffer.Count; i > 0; i-- )
                        {
                            var cmp = this.buffer[ i - 1 ];
                            this.Attach( container, cmp.Item1, cmp.Item2 );
                            this.buffer.Remove( cmp );
                        }

                        //logger.Verbose( "All handlers in the buffer attached." );
                    }
                }
                else if ( this.IsInterestingHandler( e.Entry, typeof( IHandleMessage ).GetTypeInfo() ) )
                {
                    if ( !this.isMessageBrokerRegistered )
                    {
                        //logger.Verbose
                        //(
                        //	"Registered component is IMessageHandler: {0}/{1}, but no broker yet registered. buffering...",
                        //	h.Service.ToString( "SN" ),
                        //	h.ComponentModel.Implementation.ToString( "SN" )
                        //);

                        this.buffer.Add( new Tuple<String, IContainerEntry>( e.Entry.Key, e.Entry ) );
                    }
                    else
                    {
                        //logger.Verbose
                        //(
                        //	"Registered component is IMessageHandler: {0}/{1}",
                        //	h.Service.ToString( "SN" ),
                        //	h.ComponentModel.Implementation.ToString( "SN" )
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

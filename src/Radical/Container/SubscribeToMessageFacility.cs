using Radical.ComponentModel;
using Radical.ComponentModel.Messaging;
using Radical.Linq;
using Radical.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Radical
{
    public sealed class SubscribeToMessageFacility : IPuzzleContainerFacility
    {
        List<Tuple<String, IContainerEntry>> buffer = new List<Tuple<String, IContainerEntry>>();
        Boolean isMessageBrokerRegistered = false;

        void Attach(IPuzzleContainer container, String key, IContainerEntry entry)
        {
            var invocationModel = entry.Component.Is<INeedSafeSubscription>() ?
                InvocationModel.Safe :
                InvocationModel.Default;

            entry.Component.GetInterfaces()
                .Where(i => i.Is<IHandleMessage>() && i.IsGenericType)
                .ForEach(t => this.Subscribe(container, key, entry, t, invocationModel));
        }

        void Subscribe(IPuzzleContainer container, String key, IContainerEntry entry, Type genericHandler, InvocationModel invocationModel)
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
            var messageType = genericHandler.GetGenericArguments().Single();

            if (genericHandler.Is<IHandleMessage>())
            {
                broker.Subscribe(this, messageType, invocationModel, (s, msg) =>
                {
                    var handler = container.Resolve(key, entry.Services.First()) as IHandleMessage;

                    if (handler.ShouldHandle(s, msg))
                    {
                        handler.Handle(s, msg);
                    }
                });
            }
        }

        Boolean IsInterestingHandler(IContainerEntry entry, Type t)
        {
            return entry.Services.Any(s => s.Is(t)) || entry.Component.Is(t);
        }

        public void Initialize(IPuzzleContainer container)
        {
            container.ComponentRegistered += (s, e) =>
            {
                if (e.Entry.Services.Any(svc => svc.Is<IMessageBroker>()))
                {
                    this.isMessageBrokerRegistered = true;

                    if (this.buffer.Any())
                    {
                        for (var i = buffer.Count; i > 0; i--)
                        {
                            var cmp = this.buffer[i - 1];
                            this.Attach(container, cmp.Item1, cmp.Item2);
                            this.buffer.Remove(cmp);
                        }
                    }
                }
                else if (this.IsInterestingHandler(e.Entry, typeof(IHandleMessage)))
                {
                    if (!this.isMessageBrokerRegistered)
                    {
                        this.buffer.Add(new Tuple<String, IContainerEntry>(e.Entry.Key, e.Entry));
                    }
                    else
                    {
                        this.Attach(container, e.Entry.Key, e.Entry);
                    }
                }
            };
        }

        public void Teardown(IPuzzleContainer container)
        {

        }
    }
}

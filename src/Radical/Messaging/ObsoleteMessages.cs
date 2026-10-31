namespace Radical.Messaging
{
    static class ObsoleteMessages
    {
        const string UpgradeGuide = " See https://docs.radicalframework.com/upgrade-guides/async-message-broker. This member will be removed in version 4.0.0.";

        public const string SynchronousSubscribe = "The message broker is async-only: subscribe with a callback returning a Task instead." + UpgradeGuide;

        public const string AsynchronousCallbackFilter = "Callback filters are evaluated synchronously: subscribe with a Func<object, T, bool> filter instead." + UpgradeGuide;

        public const string Dispatch = "The message broker is async-only, and a synchronous Dispatch would have to block on asynchronous subscribers, which can deadlock: await DispatchAsync instead." + UpgradeGuide;

        public const string Handle = "Message handlers are async-only: override HandleAsync instead." + UpgradeGuide;

        public const string Broadcast = "The message broker is async-only: use BroadcastAsync instead, awaiting the returned task or deliberately discarding it." + UpgradeGuide;
    }
}

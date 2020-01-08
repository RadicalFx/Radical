using Radical.ComponentModel;
using System.ComponentModel;

namespace Radical.Observers
{
    public class EntityViewListChangedMonitor : AbstractMonitor<IEntityView>
    {
        ListChangedEventHandler handler = null;

        public EntityViewListChangedMonitor(IEntityView source)
            : base(source)
        {

        }

        public EntityViewListChangedMonitor()
            : base()
        {

        }

        public EntityViewListChangedMonitor(IEntityView source, IDispatcher dispatcher)
            : base(source, dispatcher)
        {

        }

        public EntityViewListChangedMonitor(IDispatcher dispatcher)
            : base(dispatcher)
        {

        }

        protected override void StartMonitoring(object source)
        {
            base.StartMonitoring(source);

            handler = (s, e) => OnChanged();
            Source.ListChanged += handler;
        }

        public void Observe(IEntityView source)
        {
            StopMonitoring();
            StartMonitoring(source);
        }

        protected override void OnStopMonitoring(bool targetDisposed)
        {
            if (!targetDisposed && WeakSource != null && WeakSource.IsAlive)
            {
                Source.ListChanged -= handler;
            }

            handler = null;
        }
    }
}

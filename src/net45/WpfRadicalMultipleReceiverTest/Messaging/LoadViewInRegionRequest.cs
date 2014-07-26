using System;
using WpfRadicalMultipleReceiver.Presentation;

namespace WpfRadicalMultipleReceiver.Messaging
{
    public class LoadViewInRegionRequest
    {
        public Type ViewType { get; set; }
        public MasterViewRegion DestinationRegion { get; set; }

        public LoadViewInRegionRequest(Type viewType, MasterViewRegion destinationRegion)
        {
            this.ViewType = viewType;
            this.DestinationRegion = destinationRegion;
        }
    }
}

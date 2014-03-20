using System;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Windows.Presentation.Messaging.Handlers
{
    class ViewLoadedHandler: AbstractMessageHandler<ViewLoaded>, INeedSafeSubscription
    {
        readonly IRegionInjectionHandler autoMappingHandler;
        readonly IViewResolver viewProvider;
        readonly IConventionsHandler conventions;
        readonly IRegionService regionService;

        public ViewLoadedHandler(IViewResolver viewProvider, IConventionsHandler conventions, IRegionService regionService, IRegionInjectionHandler autoMappingHandler)
        {
            this.viewProvider = viewProvider;
            this.conventions = conventions;
            this.regionService = regionService;
            this.autoMappingHandler = autoMappingHandler;
        }

        public override void Handle( Object sender, ViewLoaded message )
        {
            var view = message.View;
            if ( this.regionService.HoldsRegionManager( view ) )
            {
                var manager = this.regionService.GetRegionManager( view );
                var regions = manager.GetAllRegisteredRegions();

                foreach ( var region in regions )
                {
                    var allViewTypes = this.autoMappingHandler.GetViewsInterestedIn( region.Name );

                    foreach ( var viewType in allViewTypes )
                    {
                        this.autoMappingHandler.Inject( 
							()=> viewProvider.GetView( viewType ), 
							region );
                    }
                }
            }
        }
    }


}

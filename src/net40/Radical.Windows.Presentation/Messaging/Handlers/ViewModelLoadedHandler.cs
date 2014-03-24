//using System.Windows.Controls;
//using Topics.Radical.ComponentModel.Messaging;
//using Topics.Radical.Messaging;
//using Topics.Radical.Windows.Input;
//using Topics.Radical.Windows.Presentation.ComponentModel;
//using Topics.Radical.Windows.Presentation.Regions;
//using System.Collections.Generic;
//using System;
//using System.Linq;
//using System.Windows;

//namespace Topics.Radical.Windows.Presentation.Messaging.Handlers
//{
//    class ViewModelLoadedHandler : AbstractMessageHandler<ViewModelLoaded>, INeedSafeSubscription
//    {
//        readonly IRegionInjectionHandler autoMappingHandler;
//        readonly IViewResolver viewProvider;
//        readonly IConventionsHandler conventions;
//        readonly IRegionService regionService;

//        public ViewModelLoadedHandler( IViewResolver viewProvider, IConventionsHandler conventions, IRegionService regionService, IRegionInjectionHandler autoMappingHandler )
//        {
//            this.viewProvider = viewProvider;
//            this.conventions = conventions;
//            this.regionService = regionService;
//            this.autoMappingHandler = autoMappingHandler;
//        }

//        public override void Handle( Object sender, ViewModelLoaded message )
//        {
//            var view = this.conventions.GetViewOfViewModel( message.ViewModel );
//            if ( this.regionService.HoldsRegionManager( view ) )
//            {
//                var manager = this.regionService.GetRegionManager( view );
//                var regions = manager.GetAllRegisteredRegions();

//                foreach ( var region in regions )
//                {
//                    var allViewTypes = this.autoMappingHandler.GetViewsInterestedIn( region.Name );

//                    foreach ( var viewType in allViewTypes )
//                    {
//                        var viewToInject = viewProvider.GetView( viewType );
//                        this.autoMappingHandler.Inject( viewToInject, region );
//                    }
//                }
//            }
//        }
//    }
//}

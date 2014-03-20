using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Presentation.AsyncRegions;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Messaging;
using Topics.Radical.Windows.Presentation.Regions;

namespace Topics.Radical.Messaging.Handlers
{
	class AsyncRegionsViewLoadedHandler : MessageHandler<ViewLoaded>, INeedSafeSubscription
	{
		readonly IViewResolver viewResolver;
		readonly IConventionsHandler conventions;
		readonly IRegionService regionService;

		public AsyncRegionsViewLoadedHandler( IViewResolver viewResolver, IConventionsHandler conventions, IRegionService regionService )
		{
			this.viewResolver = viewResolver;
			this.conventions = conventions;
			this.regionService = regionService;
		}

		protected override bool OnShouldHandle( ViewLoaded message )
		{
			return message.View is Presentation.AsyncRegions.AsyncRegionsView;
		}

		public override void Handle( ViewLoaded message )
		{
			if ( this.regionService.HoldsRegionManager( message.View ) )
			{
				this.regionService.GetRegionManager( message.View )
					.GetRegion<IContentRegion>( "AsyncFoo" )
					.SetContentAsync( () => 
					{
						var foo = this.viewResolver.GetView<FooView>();
						return foo;
					} );
			}
		}
	}
}
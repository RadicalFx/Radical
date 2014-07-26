using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;
using WpfRadicalMultipleReceiver.Presentation;

namespace WpfRadicalMultipleReceiver.Messaging.Handlers
{
    public class LoadViewInShellRequestHandler : AbstractMessageHandler<LoadViewInShellRequest>, INeedSafeSubscription
    {
        private readonly IViewResolver _viewResolver;
        private readonly IRegionService _regionService;

        public LoadViewInShellRequestHandler(IViewResolver viewResolver, IRegionService regionService)
        {
            _viewResolver = viewResolver;
            _regionService = regionService;
        }

        public override void Handle(object sender, LoadViewInShellRequest message)
        {
            var view = _viewResolver.GetView(message.ViewType);
            if (view != null)
            {
                var region = _regionService.GetKnownRegionManager<MainView>()
                                           .GetRegion<ISwitchingElementsRegion>(KnownRegion.MainTabRegion.ToString());
                region.Add(view);
                region.Activate(view);
            }
        }
    }
}
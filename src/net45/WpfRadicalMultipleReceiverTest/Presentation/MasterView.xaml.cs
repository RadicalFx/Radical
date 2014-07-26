using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;
using WpfRadicalMultipleReceiver.Messaging;

namespace WpfRadicalMultipleReceiver.Presentation
{
    /// <summary>
    /// Interaction logic for MasterView.xaml
    /// </summary>
    public partial class MasterView : UserControl
    {
        private readonly IMessageBroker _broker;
        private readonly IConventionsHandler _conventions;
        private readonly IViewResolver _viewResolver;
        private readonly IRegionService _regionService;

        public MasterView(IMessageBroker broker, IConventionsHandler conventions, IViewResolver viewResolver, IRegionService regionService)
        {
            _broker = broker;
            _conventions = conventions;
            _viewResolver = viewResolver;
            _regionService = regionService;

            InitializeComponent();

            _broker.Subscribe<LoadViewInRegionRequest>(this, InvocationModel.Safe, (sender, message) =>
            {
                var view = _viewResolver.GetView(message.ViewType);
                if (view != null)
                {
                    _regionService.GetRegionManager(this)
                                  .GetRegion<IContentRegion>(message.DestinationRegion.ToString())
                                  .Content = view;
                }
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Services;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Presentation
{
	class MainViewModel : AbstractViewModel
	{
		readonly Services.SamplesManager samplesManager;
		readonly IViewResolver viewResolver;
		readonly IRegionService regionService;

		public MainViewModel( Services.SamplesManager samplesManager, IViewResolver viewResolver, IRegionService regionService )
		{
			this.samplesManager = samplesManager;
			this.viewResolver = viewResolver;
			this.regionService = regionService;

			this.Categories = samplesManager.Categories;

			this.Categories
				.SelectMany( c => c.Samples )
				.ForEach( s =>
				{
					s.ViewSampleHandler = sample =>
					{
						this.SelectedSample = sample;
						this.regionService.GetKnownRegionManager<MainView>()
							.GetRegion<IContentRegion>( "SampleContentRegion" )
							.Content = this.viewResolver.GetView( this.SelectedSample.ViewType );
					};
				} );
		}

		public IEnumerable<SamplesManager.SampleCategory> Categories
		{
			get;
			private set;
		}

		public SamplesManager.SampleItem SelectedSample
		{
			get { return this.GetPropertyValue( () => this.SelectedSample ); }
			private set { this.SetPropertyValue( () => this.SelectedSample, value ); }
		}
	}
}

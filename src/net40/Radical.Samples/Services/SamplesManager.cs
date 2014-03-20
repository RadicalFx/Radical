using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.ComponentModel;
using Topics.Radical.Reflection;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Topics.Radical.Services
{
	class SamplesManager
	{
		public class SampleCategory
		{
			public SampleCategory( String name, IEnumerable<SampleItem> samples )
			{
				this.Name = name;
				this.Samples = samples;
			}

			public String Name { get; private set; }

			public IEnumerable<SampleItem> Samples
			{
				get;
				private set;
			}
		}

		public class SampleItem
		{
			public SampleItem( Type viewModelType, Type viewType )
			{
				var attribute = viewModelType.GetAttribute<SampleAttribute>();
				this.ViewType = viewType;

				this.Title = attribute.Title;
				this.Description = attribute.Description;

				this.Order = attribute.Order;
			}

			public String Title { get; private set; }
			public String Description { get; private set; }
			public Int32 Order { get; private set; }

			public Type ViewType { get; private set; }

			public Action<SampleItem> ViewSampleHandler { get; set; }

			public void ViewSample() 
			{
				this.ViewSampleHandler( this );
			}
		}

		public SamplesManager( IConventionsHandler conventions )
		{
			this.Categories = GetAssembly.ThatContains<SamplesManager>()
				.GetTypes()
				.Where( t => t.IsAttributeDefined<SampleAttribute>() )
				.GroupBy( t =>
				{
					var sa = t.GetAttribute<SampleAttribute>();
					return sa.Category;
				} )
				.Select( g =>
				{
					return new SampleCategory
					(
						g.Key,
						g.Select( t => new SampleItem( t, conventions.ResolveViewType( t ) ) )
							.AsReadOnly()
					);
				} )
				.AsReadOnly();
		}

		public IEnumerable<SampleCategory> Categories
		{
			get;
			private set;
		}
	}
}

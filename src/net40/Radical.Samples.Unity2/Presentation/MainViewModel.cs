using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radical.Samples.Unity2.ComponentModel;
using Radical.Samples.Unity2.Services;
using Topics.Radical.Windows.Presentation;

namespace Radical.Samples.Unity2.Presentation
{
	class MainViewModel : AbstractViewModel
	{
		public MainViewModel( IBar b, IFoo f )
		{
			this.MyText = "The view model says: Are they the same ref? " + Object.ReferenceEquals( b, f ).ToString();
		}

		public String MyText
		{
			get { return this.GetPropertyValue( () => this.MyText ); }
			private set { this.SetPropertyValue( () => this.MyText, value ); }
		}
	}
}

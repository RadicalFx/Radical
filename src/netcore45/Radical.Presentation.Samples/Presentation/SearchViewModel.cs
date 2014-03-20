using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.ApplicationModel.Activation;

namespace Radical.Presentation.Samples.Presentation
{
    class SearchViewModel : AbstractViewModel, IExpectNavigatedToCallback
    {
        public String Query
        {
            get { return this.GetPropertyValue( () => this.Query ); }
            set { this.SetPropertyValue( () => this.Query, value ); }
        }

        public void OnNavigatedTo( NavigationEventArgs e )
        {
            var args = e.Arguments as ISearchActivatedEventArgs;
            if ( args != null )
            {
                this.Query = args.QueryText;
            }
        }
    }
}

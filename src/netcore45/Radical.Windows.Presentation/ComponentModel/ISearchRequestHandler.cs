using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;

namespace Topics.Radical.Windows.Presentation.ComponentModel
{
	public interface ISearchRequestHandler
	{
		Task OnSearchRequest( ISearchActivatedEventArgs e );

        Task OnSearchSuggestionsRequest( SearchPane searchPane, SearchPaneSuggestionsRequestedEventArgs e );
    }
}

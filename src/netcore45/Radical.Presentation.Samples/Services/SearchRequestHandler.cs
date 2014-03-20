using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Radical.Presentation.Samples.Services
{
    class SearchRequestHandler : ISearchRequestHandler
    {
        readonly INavigationService navigation;
        readonly CoreDispatcher dispatcher;

        public SearchRequestHandler( INavigationService navigation, CoreDispatcher dispatcher )
        {
            this.navigation = navigation;
            this.dispatcher = dispatcher;
        }

        public async Task OnSearchRequest( ISearchActivatedEventArgs e )
        {
            await this.dispatcher.RunAsync( CoreDispatcherPriority.High, () =>
            {
                var args = e;
                this.navigation.Navigate<Presentation.SearchView>( args );
            } );
        }


        public async Task OnSearchSuggestionsRequest( SearchPane searchPane, SearchPaneSuggestionsRequestedEventArgs e )
        {
            e.Request.SearchSuggestionCollection.AppendQuerySuggestions( new[] { "a", "b", "aa", "bb" } );
        }
    }
}

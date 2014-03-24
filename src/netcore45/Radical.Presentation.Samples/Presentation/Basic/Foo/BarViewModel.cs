using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Topics.Radical.ComponentModel.Windows.Input;
using Topics.Radical.Observers;
using Topics.Radical.Windows.Input;
using Topics.Radical.Windows.Presentation;
using Topics.Radical.Windows.Presentation.ComponentModel;

namespace Radical.Presentation.Samples.Presentation.Basic.Foo
{
	class BarViewModel : AbstractViewModel, IExpectNavigatedToCallback
	{
		readonly INavigationService ns;

		public BarViewModel( INavigationService ns )
		{
			this.ns = ns;

            this.GoBack = DelegateCommand.Create()
                .OnCanExecute( o => !String.IsNullOrWhiteSpace( this.Sample ) && this.ns.CanGoBack )
                .OnExecute( o => this.ns.GoBack() )
                .AddMonitor
                (
                    PropertyObserver.For( this )
                        .Observe( v => v.Sample ) 
                );

            this.SetInitialPropertyValue( () => this.Sample, "delete me..." );
		}

        public String Sample
        {
            get { return this.GetPropertyValue( () => this.Sample ); }
            set { this.SetPropertyValue( () => this.Sample, value ); }
        }

		public IDelegateCommand GoBack { get; private set; }

		void IExpectNavigatedToCallback.OnNavigatedTo( NavigationEventArgs e )
		{
			this.GoBack.EvaluateCanExecute();
		}
	}
}

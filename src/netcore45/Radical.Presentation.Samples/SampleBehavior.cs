using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Radical.Presentation.Samples
{
	public class SampleBehavior : Topics.Radical.Windows.Behaviors.RadicalBehavior<Grid>
	{
        #region Dependency Property: SampleText

        public static readonly DependencyProperty SampleTextProperty = DependencyProperty.Register(
            "SampleText",
            typeof( String ),
            typeof( SampleBehavior ),
            new PropertyMetadata( null, ( s, e ) => 
            {
                var x = e;
                var y = x;
                var z = y;
            } ) );

        public String SampleText
        {
            get { return ( String )this.GetValue( SampleTextProperty ); }
            set { this.SetValue( SampleTextProperty, value ); }
        }

        #endregion

		protected override void OnAttached()
		{
            
		}

		protected override void OnDetached()
		{
			
		}
	}
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Topics.Radical.ComponentModel.Windows.Input;
using System.Reflection;
using Topics.Radical.Linq;
using Topics.Radical.Windows.Input;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Windows.CommandBuilders;

namespace Topics.Radical.Windows.Markup
{
	public class BehaviorAutoCommandBinding : AutoCommandBinding
	{
		protected override void OnProvideValue( IServiceProvider provider, object value )
		{
			DependencyObject fe;
			DependencyProperty dp;

			if( this.TryGetTargetItems( provider, out fe, out dp ) )
			{
				var inab = fe as INotifyAttachedOjectLoaded;
				if( inab != null )
				{
					EventHandler h = null;
					h = ( s, e ) =>
					{
						inab.AttachedObjectLoaded -= h;
						this.OnTargetLoaded( fe, dp );
					};

					inab.AttachedObjectLoaded += h;
				}
			}
		}

		protected override void SetInputBindings( DependencyObject target, ICommandSource source, IDelegateCommand command )
		{
			//Not supported ?
		}

        protected override DelegateCommandBuilder GetCommandBuilder()
        {
            return new BehaviorDelegateCommandBuilder();
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Topics.Radical.Observers;
using System.ComponentModel;
using Topics.Radical.Validation;
using Topics.Radical.ComponentModel.Windows.Input;

namespace Topics.Radical.Windows.Input
{
	/// <summary>
	/// Add behaviors to a DelegateCommand.
	/// </summary>
	public static class DelegateCommandExtensions
	{
		/// <summary>
		/// Monitors the specified properties.
		/// </summary>
		/// <typeparam name="TSource">The type of the source.</typeparam>
		/// <param name="command">The command.</param>
		/// <param name="source">The source.</param>
		/// <param name="properties">The properties to monitor.</param>
		/// <returns>The original command.</returns>
		public static IDelegateCommand Observe<TSource>( this IDelegateCommand command, TSource source, params Expression<Func<TSource, Object>>[] properties )
			where TSource : INotifyPropertyChanged
		{
			Ensure.That( properties ).Named( () => properties ).IsNotNull();

			if( properties.Any() )
			{
				var observer = PropertyObserver.For( source );

				foreach( var prop in properties )
				{
					observer.Observe( prop );
				}

				command.AddMonitor( observer );
			}
			else 
			{
				var observer = PropertyObserver.ForAllPropertiesOf( source );
				command.AddMonitor( observer );
			}

			return command;
		}
	}
}

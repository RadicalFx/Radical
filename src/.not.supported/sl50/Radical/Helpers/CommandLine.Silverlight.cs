using System;
using System.Collections.Generic;
using System.Linq;
using Topics.Radical.Validation;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Topics.Radical.Helpers
{
	public class CommandLine
	{
		static CommandLine _current;

		/// <summary>
		/// Gets the current command line instance.
		/// </summary>
		/// <returns></returns>
		public static CommandLine GetCurrent()
		{
			if( _current == null )
			{
				if( Deployment.Current.Dispatcher != null )
				{
					var wi = new ManualResetEvent( false );
					var op = Deployment.Current.Dispatcher.BeginInvoke( () => 
					{
						_current = new CommandLine( System.Windows.Application.Current.Host.InitParams );
						wi.Set();
					} );

					wi.WaitOne();
				}
				else
				{
					_current = new CommandLine( System.Windows.Application.Current.Host.InitParams );
				}
			}
			return _current;
		}

		//const Char SEPARATOR = '=';

		readonly IDictionary<String, String> initParams;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandLine"/> class.
		/// </summary>
		/// <param name="initParams">The current command line args.</param>
		public CommandLine( IDictionary<String, String> initParams )
		{
			Ensure.That( initParams ).Named( () => initParams ).IsNotNull();

			this.initParams = initParams;
		}

		/// <summary>
		/// Given a command line argument removes leading / or -, and if any,
		/// removes the argument value.
		/// </summary>
		/// <param name="fullArgument">The full argument.</param>
		/// <returns>Just the argument key.</returns>
		static String Normalize( String fullArgument )
		{
			if( fullArgument.StartsWith( "/" ) || fullArgument.StartsWith( "-" ) )
			{
				fullArgument = fullArgument.Substring( 1 );
			}

			return fullArgument;
		}

		/// <summary>
		/// Determines whether the current command contains the specified argument.
		/// </summary>
		/// <param name="arg">The argument to search for.</param>
		/// <returns>
		/// 	<c>true</c> if the current command contains the specified argument; otherwise, <c>false</c>.
		/// </returns>
		public Boolean Contains( String arg )
		{
			return this.initParams.ContainsKey( CommandLine.Normalize( arg ) );
		}

		/// <summary>
		/// Tries to safely get a value given the argument name of 
		/// a key/value command line argument.
		/// </summary>
		/// <typeparam name="T">The expected type of the argument value</typeparam>
		/// <param name="arg">The argument name.</param>
		/// <param name="value">The current argument value.</param>
		/// <returns><c>True</c> if the operation succeded, otherwise <c>false</c>.</returns>
		public Boolean TryGetValue<T>( String arg, out T value )
		{
			var key = CommandLine.Normalize( arg );
			if( this.initParams.ContainsKey( key ) )
			{
				var v = this.initParams[ key ];
				if( !String.IsNullOrEmpty( v ) )
				{
					try
					{
						var tt = typeof( T );
						var isNullable = Nullable.GetUnderlyingType( tt ) != null;
						if( isNullable )
						{
							var t = Nullable.GetUnderlyingType( tt );
							var converted = Convert.ChangeType( v, t, CultureInfo.InvariantCulture );
							value = ( T )converted;
						}
						else
						{
							var converted = Convert.ChangeType( v, tt, CultureInfo.InvariantCulture );
							value = ( T )converted;
						}

						return true;
					}
					catch
					{

					}
				}
			}

			value = default( T );
			return false;
		}
	}
}

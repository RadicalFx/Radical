using System;
using System.Collections.Generic;
using System.Linq;
using Topics.Radical.Validation;
using Topics.Radical.Reflection;
using System.Text;

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
			if ( _current == null )
			{
				_current = new CommandLine( Environment.GetCommandLineArgs() );
			}
			return _current;
		}

		const Char SEPARATOR = '=';

		readonly IEnumerable<String> args;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommandLine"/> class.
		/// </summary>
		/// <param name="args">The current command line args.</param>
		public CommandLine( IEnumerable<String> args )
		{
			Ensure.That( args ).Named( "args" ).IsNotNull();

			this.args = args;
		}

		/// <summary>
		/// Given a command line argument removes leading / or -, and if any,
		/// removes the argument value.
		/// </summary>
		/// <param name="fullArgument">The full argument.</param>
		/// <returns>Just the argument key.</returns>
		static String Normalize( String fullArgument )
		{
			if ( fullArgument.StartsWith( "/" ) || fullArgument.StartsWith( "-" ) )
			{
				fullArgument = fullArgument.Substring( 1 );
			}

			var idx = fullArgument.IndexOf( SEPARATOR );
			if ( idx != -1 )
			{
				fullArgument = fullArgument.Substring( 0, idx );
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
			var query = this.args.Where( s => Normalize( s ).Equals( arg, StringComparison.CurrentCultureIgnoreCase ) );
			return query.Any();
		}

		String GetValue( String argumentValuePair )
		{
			var fullValue = this.args.Where( s =>
			{
				var sc = StringComparison.CurrentCultureIgnoreCase;
				return CommandLine.Normalize( s ).Equals( argumentValuePair, sc );
			} )
			.Single();

			var idx = fullValue.IndexOf( SEPARATOR );

			if ( idx < 0 )
			{
				return null;
			}

			return fullValue.Substring( idx + 1 );
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
			if ( this.Contains( arg ) )
			{
				var v = this.GetValue( arg );
				if ( !String.IsNullOrEmpty( v ) )
				{
					try
					{
						var tt = typeof( T );
						var isNullable = tt.IsGenericType && tt.GetGenericTypeDefinition() == typeof( Nullable<> );
						if ( isNullable )
						{
							tt = Nullable.GetUnderlyingType( tt );
						}

						if ( tt.IsEnum )
						{
							var enumValue = Enum.Parse( tt, v, true );
							value = ( T )enumValue;
						}
						else
						{
							var converted = Convert.ChangeType( v, tt );
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

		public T As<T>() where T : class, new()
		{
			var properties = typeof( T )
				.GetProperties()
				.Where( pi => pi.IsAttributeDefined<CommandLineArgumentAttribute>() )
				.Select( pi =>
				{
					var attribute = pi.GetAttribute<CommandLineArgumentAttribute>();

					return new
					{
						Property = pi,
						Argument = attribute.ArgumentName,
						IsRequired = attribute.IsRequired,
						Aliases = attribute.Aliases
					};
				} );

			var instance = new T();

			foreach ( var property in properties )
			{
				if ( !this.Contains( property.Argument ) && !property.Aliases.Any( alias => this.Contains( alias ) ) && property.IsRequired )
				{
					var msg = String.Format( "The command line argument '{0}' is required.", property.Argument );
					throw new ArgumentException( msg, property.Argument );
				}
				else if ( this.Contains( property.Argument ) || property.Aliases.Any( alias => this.Contains( alias ) ) )
				{
					var lookFor = property.Argument;
					if ( !this.Contains( lookFor ) )
					{
						lookFor = property.Aliases.First( alias => this.Contains( alias ) );
					}

					var value = this.GetValue( lookFor );
					if ( !String.IsNullOrEmpty( value ) )
					{
						var t = property.Property.PropertyType;
						var isNullable = Nullable.GetUnderlyingType( t ) != null;
						if ( isNullable )
						{
							t = Nullable.GetUnderlyingType( t );
						}

						if ( t.IsEnum )
						{
							var enumValue = Enum.Parse( t, value, true );
							property.Property.SetValue( instance, enumValue, null );
						}
						else
						{
							var converted = Convert.ChangeType( value, t );
							if ( t == typeof( String ) )
							{
								var temp = ( String )converted;
								if ( temp.IndexOf( ' ' ) != -1 && temp.StartsWith( "\"" ) && temp.EndsWith( "\"" ) )
								{
									converted = temp.Trim( '"' );
								}
							}

							property.Property.SetValue( instance, converted, null );
						}
					}
					else if ( property.Property.PropertyType.Is<Boolean>() )
					{
						property.Property.SetValue( instance, this.Contains( property.Argument ), null );
					}
				}
			}

			return instance;
		}

		public static String AsArguments<T>( T source )
		{
			var properties = typeof( T )
				.GetProperties()
				.Where( pi => pi.IsAttributeDefined<CommandLineArgumentAttribute>() )
				.Select( pi => new { Property = pi, Argument = pi.GetAttribute<CommandLineArgumentAttribute>().ArgumentName } );

			var builder = new StringBuilder();
			foreach ( var p in properties )
			{
				var value = p.Property.GetValue( source, null ).ToString();
				if ( value.IndexOf( ' ' ) != -1 )
				{
					value = String.Format( "\"{0}\"", value );
				}
				builder.AppendFormat( "-{0}{1}{2}", p.Argument, SEPARATOR, value );
				builder.Append( ' ' );
			}

			var args = builder.ToString().TrimEnd( ' ' );

			return args;
		}
	}
}

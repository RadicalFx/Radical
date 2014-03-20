using System;
using System.Linq;
using System.Text.RegularExpressions;
using Topics.Radical.Validation;
using System.Collections.Generic;
using Topics.Radical.Collections;
using System.Reflection;

namespace Topics.Radical
{
	/// <summary>
	/// Adds behaviors to the <c>String</c> class.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Performs a case insensitive like compare using the specified compare pattern.
		/// </summary>
		/// <param name="value">The source value to compare against the pattern.</param>
		/// <param name="pattern">The pattern to use as search pattern.</param>
		/// <returns><c>True</c> in case of successful match, otherwise <c>false</c>.</returns>
		/// <remarks>The default IsLike is performed using a case insensitive search.</remarks>
		public static Boolean IsLike( this String value, String pattern )
		{
			return IsLike( value, pattern, true );
		}

		/// <summary>
		/// Performs a case insensitive like compare using the specified compare patterns.
		/// </summary>
		/// <param name="value">The source value to compare against the pattern.</param>
		/// <param name="patterns">The patterns to use as search pattern.</param>
		/// <returns>
		/// 	<c>True</c> in case of successful match, otherwise <c>false</c>.
		/// </returns>
		/// <remarks>The default IsLike is performed using a case insensitive search.</remarks>
		public static Boolean IsLike( this String value, params String[] patterns )
		{
			Ensure.That( patterns ).Named( "patterns" )
				.IsNotNull();

			foreach( var pattern in patterns )
			{
				if( value.IsLike( pattern ) )
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Performs a Like compare using the specified compare pattern.
		/// </summary>
		/// <param name="value">The source value to compare against the pattern.</param>
		/// <param name="pattern">The pattern to use a a search pattern.</param>
		/// <param name="ignoreCase"><c>True</c> to perform a case insensitive search, otherwise <c>false</c>.</param>
		/// <returns>
		/// 	<c>True</c> in cas of successful match, otherwise <c>false</c>.
		/// </returns>
		public static Boolean IsLike( this String value, String pattern, Boolean ignoreCase )
		{
			if( value == null && pattern == null )
			{
				return true;
			}
			else if( value == null || pattern == null )
			{
				return false;
			}

			/*
			 * Se nella stringa ci sono delle '\' dobbiamo 
			 * metterci un bell'escape
			 */
			pattern = pattern.Replace( @"\", @"\\" );

			/*
			 * Se nella stringa ci sono dei '.' dobbiamo 
			 * metterci un bell'escape, questa operazione
			 * è da fare dopo la precedente per evitare
			 * di raddoppiare anche queste \
			 */
			pattern = pattern.Replace( ".", "\\." );

			/*
			 * Gli '*' vengono sostituiti con '.*'
			 */
			pattern = pattern.Replace( "*", ".*" );

			/*
			 * I '?' vengono sostituiti con il semplice '.'
			 */
			pattern = pattern.Replace( "?", "." );

			/*
			 * Includiamo il nostro pattern tra
			 * \A e \z per fare in modo che matchi con
			 * l'inizio e la fine della stringa altrimenti
			 * ad es. Beatrice matcha con B*r 
			 */
			pattern = String.Concat( "\\A", pattern, "\\z" );

			var options = RegexOptions.None;
			if( ignoreCase )
			{
				options |= RegexOptions.IgnoreCase;
			}

			return Regex.Match( value, pattern, options ).Success;
		}

		/// <summary>
		/// Appends the specified text to the current string value.
		/// </summary>
		/// <param name="value">The current string value.</param>
		/// <param name="text">The text to append.</param>
		/// <returns>A new string composed by the source value with the given text appended at the end.</returns>
		public static String Append( this String value, String text )
		{
			return String.Concat( value, text );
		}

		/// <summary>
		/// If the input string is null returns the supplied default value.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The input string, if not null, otherwise the supplied default value.</returns>
		public static String ValueOr( this String value, String defaultValue )
		{
			return ValueOr( value, defaultValue, null );
		}

		/// <summary>
		/// If the input string is null returns the supplied default value.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <param name="ifValue">A delegate that will be invoked, if the supplied value is not null, in 
		/// order to give the caller an opportunity to customize the return value; if this delegate is null 
		/// the incoming value is returned, otherwise is returned the return value of the supplied delegate.</param>
		/// <returns>
		/// The input string, if not null, otherwise the supplied default value.
		/// </returns>
		public static String ValueOr( this String value, String defaultValue, Func<String, String> ifValue )
		{
			if( value != null )
			{
				return ifValue != null ? ifValue( value ) : value;
			}

			return defaultValue;
		}

		/// <summary>
		/// If the input string is null returns an empty string.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <returns>The input string, if not null, otherwise an empty string.</returns>
		public static String ValueOrEmpty( this String value )
		{
			return ValueOr( value, String.Empty, null );
		}

		/// <summary>
		/// If the given string is null or empty returns the supplied default value.
		/// </summary>
		/// <param name="value">The value to test against.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The supplied value or default if the supplied one is null or empty.</returns>
		public static String IfNullOrEmptyReturn( this String value, String defaultValue )
		{
			var tmp = value.ValueOrEmpty().Trim();
			return String.IsNullOrEmpty( tmp ) ? defaultValue : value;
		}

		/// <summary>
		/// If the input string is null returns an empty string.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <param name="ifValue">A delegate that will be invoked, if the supplied value is not null, in 
		/// order to give the caller an opportunity to customize the return value; if this delegate is null 
		/// the incoming value is returned, otherwise is returned the return value of the supplied delegate.</param>
		/// <returns>
		/// The input string, if not null, otherwise an empty string.
		/// </returns>
		public static String ValueOrEmpty( this String value, Func<String, String> ifValue )
		{
			return ValueOr( value, String.Empty, ifValue );
		}

		/// <summary>
		/// Indicates whether the specified System.String object is null or an System.String.Empty string.
		/// </summary>
		/// <param name="value">The value to test.</param>
		/// <returns>
		/// 	<c>true</c> if is null or empty; otherwise, <c>false</c>.
		/// </returns>
		public static Boolean IsNullOrEmpty( this String value )
		{
			return String.IsNullOrEmpty( value );
		}

		/// <summary>
		/// Splits the given source string using the supplied chars returning a list of
		/// distinct values.
		/// </summary>
		/// <param name="source">The source string to split.</param>
		/// <param name="separators">The char separators.</param>
		/// <returns>A distinct list of string spliited by the given chars.</returns>
		public static IEnumerable<String> AsKeywords( this String source, params Char[] separators )
		{
			return AsKeywords( source, true, separators );
		}

		/// <summary>
		/// Splits the given source string using the supplied chars returning a list of
		/// distinct values.
		/// </summary>
		/// <param name="source">The source string to split.</param>
		/// <param name="applyWildChardsIfNecessary">if set to <c>true</c> [apply wild chards if necessary].</param>
		/// <param name="separators">The char separators.</param>
		/// <returns>
		/// A distinct list of string spliited by the given chars.
		/// </returns>
		public static IEnumerable<String> AsKeywords( this String source, Boolean applyWildChardsIfNecessary, params Char[] separators )
		{
			if( String.IsNullOrEmpty( source ) )
			{
				return new ReadOnlyCollection<String>( new String[ 0 ] );
			}

			return source.Split( separators, StringSplitOptions.RemoveEmptyEntries )
				.Aggregate( new List<String>(), ( accumulator, word ) =>
				{
					var tmp = word.Trim();
					if( tmp.Length > 0 )
					{
						if( applyWildChardsIfNecessary )
						{
							tmp = ParseWildChars( tmp );
						}

						accumulator.Add( tmp );
					}

					return accumulator;
				} )
				.Distinct();
		}

		static String ParseWildChars( String word )
		{
			if( word.IndexOfAny( new[] { '*', '?' } ) == -1 )
			{
				word = String.Concat( "*", word, "*" );
			}

			return word;
		}

//		/// <summary>
//		/// Returns the relative uri formatted as a pack uri: pack://.
//		/// </summary>
//		/// <param name="resourceRelativeUri">The resource relative URI.</param>
//		/// <returns>The pack uri.</returns>
//		public static String AsPackUri( this String resourceRelativeUri )
//		{
//			Ensure.That( resourceRelativeUri )
//				.Named( "resourceRelativeUri" )
//				.IsNotNullNorEmpty();

//			var assembly = Assembly.GetCallingAssembly();

//#if SILVERLIGHT
//			var assemblyName = new System.Reflection.AssemblyName( assembly.FullName );
//			String aName = assemblyName.Name;
//#else
//			String aName = assembly.GetName().Name;
//#endif

//			return resourceRelativeUri.AsPackUri( aName );
//		}

		/// <summary>
		/// Returns the relative uri formatted as a pack uri: pack://.
		/// </summary>
		/// <param name="resourceRelativeUri">The resource relative URI.</param>
		/// <param name="assemblyName">Name of the assembly containing the resource.</param>
		/// <returns>The pack uri.</returns>
		public static String AsPackUri( this String resourceRelativeUri, String assemblyName )
		{
			Ensure.That( resourceRelativeUri ).Named( "resourceRelativeUri" ).IsNotNullNorEmpty();
			Ensure.That( assemblyName ).Named( "assemblyName" ).IsNotNullNorEmpty();

			var uri = String.Format( "pack://application:,,,/{0};component{1}", assemblyName, resourceRelativeUri );
			return uri;
		}
	}
}
